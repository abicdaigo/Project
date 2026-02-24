<#
.SYNOPSIS
    AutoPartsPM を AGWEB サーバーへデプロイするスクリプト

.DESCRIPTION
    1. dotnet publish でリリースビルド
    2. アプリプールを停止
    3. \\agweb\WebApp へ robocopy でファイルを転送
    4. アプリプールを再起動

.PARAMETER SkipBuild
    ビルドをスキップして転送のみ行う

.PARAMETER RemoteConfig
    IIS の初回セットアップも行う (初回のみ)

.EXAMPLE
    .\Deploy-AGWEB.ps1
    .\Deploy-AGWEB.ps1 -SkipBuild
#>

param(
    [switch]$SkipBuild,
    [switch]$RemoteConfig
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# ===== 設定 =====
$ProjectRoot    = Split-Path $PSScriptRoot -Parent
$WebProject     = Join-Path $ProjectRoot 'src\AutoPartsPM.Web\AutoPartsPM.Web.csproj'
$PublishOutput  = Join-Path $ProjectRoot 'publish'
$DeployTarget   = '\\agweb\WebApp'
$IisServer      = 'AGWEB'
$AppPoolName    = 'AutoPartsPM'
$SiteName       = 'Default Web Site'
$AppAlias       = 'Project'          # アクセス URL: http://AGWEB/Project
# =================

function Write-Step($msg) {
    Write-Host "`n==> $msg" -ForegroundColor Cyan
}

# ===== 1. ビルド & パブリッシュ =====
if (-not $SkipBuild) {
    Write-Step "ビルド & パブリッシュ (Release)"

    if (Test-Path $PublishOutput) {
        Remove-Item $PublishOutput -Recurse -Force
    }

    dotnet publish $WebProject `
        --configuration Release `
        --output $PublishOutput `
        --runtime win-x64 `
        --self-contained false `
        /p:PublishReadyToRun=true

    if ($LASTEXITCODE -ne 0) {
        throw "dotnet publish に失敗しました (exit code: $LASTEXITCODE)"
    }

    Write-Host "  パブリッシュ完了: $PublishOutput" -ForegroundColor Green
}

# ===== 2. IIS アプリプール停止 =====
Write-Step "AGWEB のアプリプール '$AppPoolName' を停止"

try {
    Invoke-Command -ComputerName $IisServer -ScriptBlock {
        param($pool)
        Import-Module WebAdministration -ErrorAction Stop
        $state = (Get-WebAppPoolState -Name $pool).Value
        if ($state -eq 'Started') {
            Stop-WebAppPool -Name $pool
            # 停止完了を待機 (最大 15 秒)
            $timeout = 15
            while ($timeout -gt 0 -and (Get-WebAppPoolState -Name $pool).Value -ne 'Stopped') {
                Start-Sleep -Seconds 1
                $timeout--
            }
            Write-Host "  アプリプール停止済み"
        } else {
            Write-Host "  アプリプールはすでに停止しています"
        }
    } -ArgumentList $AppPoolName
} catch {
    Write-Warning "アプリプール停止中にエラーが発生しました: $_"
    Write-Warning "手動で IIS を停止してから続行してください。"
    Read-Host "続行するには Enter を押してください..."
}

# ===== 3. ファイル転送 (robocopy) =====
Write-Step "ファイルを $DeployTarget へ転送"

if (-not (Test-Path $DeployTarget)) {
    throw "デプロイ先が見つかりません: $DeployTarget`nAGWEB サーバーへのネットワーク接続とフォルダ共有を確認してください。"
}

# robocopy: /MIR = ミラー (差分更新 + 削除)、/NFL = ファイルリスト非表示、/NDL = ディレクトリリスト非表示
robocopy $PublishOutput $DeployTarget /MIR /NP /NFL /NDL /R:3 /W:5

# robocopy の終了コード 0-7 は成功 (8以上がエラー)
if ($LASTEXITCODE -ge 8) {
    throw "robocopy が失敗しました (exit code: $LASTEXITCODE)"
}

Write-Host "  転送完了" -ForegroundColor Green

# ===== 4. IIS アプリプール起動 =====
Write-Step "AGWEB のアプリプール '$AppPoolName' を起動"

Invoke-Command -ComputerName $IisServer -ScriptBlock {
    param($pool)
    Import-Module WebAdministration -ErrorAction Stop
    Start-WebAppPool -Name $pool
    Write-Host "  アプリプール起動済み"
} -ArgumentList $AppPoolName

# ===== 5. (初回のみ) IIS セットアップ =====
if ($RemoteConfig) {
    Write-Step "IIS 初回セットアップ (アプリプール + アプリケーション作成)"

    Invoke-Command -ComputerName $IisServer -ScriptBlock {
        param($pool, $site, $alias, $physPath)
        Import-Module WebAdministration -ErrorAction Stop

        # アプリプール作成 (.NET CLR なし = .NET Core 用)
        if (-not (Test-Path "IIS:\AppPools\$pool")) {
            New-WebAppPool -Name $pool
            Set-ItemProperty "IIS:\AppPools\$pool" managedRuntimeVersion ''
            # アプリプール ID を LocalSystem に変更 (AGDB への Trusted_Connection に必要)
            # ※ セキュリティを重視する場合はサービスアカウントを使用すること
            Set-ItemProperty "IIS:\AppPools\$pool" processModel.userName 'LocalSystem'
            Set-ItemProperty "IIS:\AppPools\$pool" processModel.password ''
            Set-ItemProperty "IIS:\AppPools\$pool" processModel.identityType 'LocalSystem'
            Write-Host "  アプリプール '$pool' を作成しました"
        } else {
            Write-Host "  アプリプール '$pool' は既存です"
        }

        # IIS アプリケーション作成
        $appPath = "$site/$alias"
        if (-not (Get-WebApplication -Name $alias -Site $site -ErrorAction SilentlyContinue)) {
            New-WebApplication -Name $alias -Site $site `
                -PhysicalPath $physPath -ApplicationPool $pool
            Write-Host "  IIS アプリケーション '$appPath' を作成しました"
        } else {
            Write-Host "  IIS アプリケーション '$appPath' は既存です"
        }

        Write-Host "`n  アクセス URL: http://$env:COMPUTERNAME/$alias" -ForegroundColor Yellow
    } -ArgumentList $AppPoolName, $SiteName, $AppAlias, 'C:\inetpub\wwwroot\WebApp'
}

Write-Step "デプロイ完了"
Write-Host @"

  アクセス URL : http://AGWEB/Project
  ログイン画面 : http://AGWEB/Project/account/login

  ※ 初回デプロイの場合は以下のコマンドを実行してください:
     .\Deploy-AGWEB.ps1 -RemoteConfig

  ※ DB マイグレーションが必要な場合:
     dotnet ef database update -p src\AutoPartsPM.Infrastructure -s src\AutoPartsPM.Web
     (AGDB サーバーへの接続が必要)
"@ -ForegroundColor Green
