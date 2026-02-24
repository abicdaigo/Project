# AutoPartsPM 追加機能実装計画

## Context

AutoPartsPM（自動車部品メーカー向けプロジェクト管理システム）の初期実装が完了し、全モジュール（顧客、課題、設備、ゲートレビュー、PPAP、ドキュメント、マイルストーン、管理者ページ）が動作する状態。しかし、プロジェクトの新規作成ページ、プロジェクト詳細の編集機能、APQPフェーズ管理、プロジェクトメンバー管理、成果物管理、ダッシュボード統計、ファイルアップロード、Excelレポート、監査ログなどの重要機能が未実装。本計画はこれらを P1（必須）→ P2（重要）→ P3（拡張）の優先度で実装する。

---

## 技術スタック

| 項目 | 技術 |
|------|------|
| フレームワーク | .NET 9 / ASP.NET Core |
| フロントエンド | Blazor Server + MudBlazor 7.14.0 |
| ORM | Entity Framework Core 9.0 |
| CQRS | MediatR 12.4.1 |
| バリデーション | FluentValidation |
| 認証 | Windows Authentication (Negotiate/Kerberos) |
| DB | SQL Server |

## アーキテクチャ

```
AutoPartsPM.Domain        → エンティティ、列挙型
AutoPartsPM.Application   → CQRS Commands/Queries/Handlers/Validators、インターフェース
AutoPartsPM.Infrastructure → EF Core DbContext、サービス実装
AutoPartsPM.Web           → Blazor Server UI（MudBlazor）
```

---

## P1: 必須機能（プロジェクト CRUD + APQP フェーズ管理）

### 1-1. プロジェクト新規作成ページ

**新規ファイル:**

| ファイル | 説明 |
|---------|------|
| `src/AutoPartsPM.Web/Components/Pages/Projects/ProjectCreate.razor` | プロジェクト新規作成UI |

**実装内容:**
- MudForm + MudStepper で3ステップ入力（基本情報 → 顧客・日程 → 確認）
- 顧客選択: `MudAutocomplete` で `GetCustomerListQuery` から候補取得
- フィールド: ProjectCode, ProjectName, PartNumber, PartName, ModelCode, CustomerId, SOPDate, ProjectManagerName
- 送信: 既存 `CreateProjectCommand` を使用（`IMediator.Send`）
- 成功後 `/projects/{id}` へ遷移

**修正ファイル:**

| ファイル | 変更内容 |
|---------|---------|
| `src/AutoPartsPM.Application/Features/Projects/Commands/CreateProject/CreateProjectCommand.cs` | `init` → `set` に変更（@bind-Value 対応） |

### 1-2. プロジェクト詳細の編集機能

**新規ファイル:**

| ファイル | 説明 |
|---------|------|
| `src/AutoPartsPM.Application/Features/Projects/Commands/UpdateProject/UpdateProjectCommand.cs` | プロジェクト更新コマンド |
| `src/AutoPartsPM.Application/Features/Projects/Commands/UpdateProject/UpdateProjectCommandHandler.cs` | 更新ハンドラー |
| `src/AutoPartsPM.Application/Features/Projects/Commands/UpdateProject/UpdateProjectCommandValidator.cs` | バリデーター |

**実装内容:**
- `UpdateProjectCommand` : record with `{ get; set; }` — Id, ProjectCode, ProjectName, PartNumber, PartName, ModelCode, SOPDate, ProjectManagerName, Status
- Handler: `FindAsync` → プロパティ更新 → `SaveChangesAsync`
- Validator: ProjectCode/ProjectName 必須、重複チェック（自身除外）

**修正ファイル:**

| ファイル | 変更内容 |
|---------|---------|
| `src/AutoPartsPM.Web/Components/Pages/Projects/ProjectDetail.razor` | 「基本情報」タブに編集モード追加（CustomerDetail.razor の編集パターンを踏襲） |

**編集モードの実装パターン（CustomerDetail.razor を参考）:**
- `_isEditing` フラグで表示/編集切替
- `StartEditing()` : DTOからCommandへマッピング
- `CancelEditing()` : フラグリセット
- `SaveChanges()` : MudForm.Validate → Mediator.Send → 再読み込み

### 1-3. APQP フェーズ進捗更新

**新規ファイル:**

| ファイル | 説明 |
|---------|------|
| `src/AutoPartsPM.Application/Features/Projects/Commands/UpdateProjectPhase/UpdateProjectPhaseCommand.cs` | フェーズ更新コマンド |
| `src/AutoPartsPM.Application/Features/Projects/Commands/UpdateProjectPhase/UpdateProjectPhaseCommandHandler.cs` | 更新ハンドラー |
| `src/AutoPartsPM.Application/Features/Projects/Commands/UpdateProjectPhase/UpdateProjectPhaseCommandValidator.cs` | バリデーター |

**実装内容:**
- Command: Id, Progress(decimal), ActualStartDate(DateTime?), ActualEndDate(DateTime?), Notes(string?)
- Handler:
  1. フェーズ取得（Include Project）
  2. プロパティ更新
  3. Progress が 100 で ActualEndDate 未設定なら自動で今日を設定
  4. 同一Projectの全フェーズ平均で `Project.OverallProgress` 再計算
  5. SaveChanges
- Validator: Progress は 0〜100、ActualEndDate は ActualStartDate 以降

**修正ファイル:**

| ファイル | 変更内容 |
|---------|---------|
| `src/AutoPartsPM.Web/Components/Pages/Projects/ProjectDetail.razor` | 「APQPフェーズ」タブのテーブル各行に編集ボタン追加、インライン編集UI |

**フェーズ編集UI:**
- 各行に `MudIconButton`（編集アイコン）
- 編集中の行: `MudNumericField`（進捗率 0-100）、`MudDatePicker`（開始日/終了日）、`MudTextField`（備考）
- 保存/キャンセルボタン
- `_editingPhaseId` で編集中のフェーズを管理

### P1 ファイルサマリー

| 種別 | ファイル数 |
|------|-----------|
| 新規 | 7 ファイル |
| 修正 | 2 ファイル |

---

## P2: 重要機能（メンバー・成果物・ダッシュボード）

### 2-1. プロジェクトメンバー管理

**新規ファイル:**

| ファイル | 説明 |
|---------|------|
| `src/AutoPartsPM.Application/Features/ProjectMembers/Commands/AddProjectMember/AddProjectMemberCommand.cs` | メンバー追加コマンド |
| `src/AutoPartsPM.Application/Features/ProjectMembers/Commands/AddProjectMember/AddProjectMemberCommandHandler.cs` | 追加ハンドラー |
| `src/AutoPartsPM.Application/Features/ProjectMembers/Commands/RemoveProjectMember/RemoveProjectMemberCommand.cs` | メンバー削除コマンド |
| `src/AutoPartsPM.Application/Features/ProjectMembers/Commands/RemoveProjectMember/RemoveProjectMemberCommandHandler.cs` | 削除ハンドラー |
| `src/AutoPartsPM.Application/Features/ProjectMembers/Queries/GetProjectMembers/GetProjectMembersQuery.cs` | メンバー一覧クエリ + DTO |
| `src/AutoPartsPM.Application/Features/ProjectMembers/Queries/GetProjectMembers/GetProjectMembersQueryHandler.cs` | クエリハンドラー |

**実装内容:**
- `AddProjectMemberCommand`: ProjectId, MemberName, Role(string), Email
- `RemoveProjectMemberCommand(int Id)` : record 形式
- `GetProjectMembersQuery(int ProjectId)` → `List<ProjectMemberDto>`
- `ProjectMemberDto`: Id, MemberName, Role, Email, JoinedDate
- Domain の `ProjectMember` エンティティは既存（`IApplicationDbContext.ProjectMembers` DbSet あり）

**修正ファイル:**

| ファイル | 変更内容 |
|---------|---------|
| `src/AutoPartsPM.Web/Components/Pages/Projects/ProjectDetail.razor` | 新しい「メンバー」タブ追加 |
| `src/AutoPartsPM.Web/Components/_Imports.razor` | 新namespace追加 |

**メンバータブUI:**
- MudTable でメンバー一覧（Name, Role, Email, JoinedDate）
- 各行に削除ボタン（`MudIconButton` + 確認ダイアログ）
- テーブル上部に「メンバー追加」フォーム（MudTextField x 3 + 登録ボタン）

### 2-2. 成果物（Deliverable）管理

**新規ファイル:**

| ファイル | 説明 |
|---------|------|
| `src/AutoPartsPM.Application/Features/Deliverables/Commands/CreateDeliverable/CreateDeliverableCommand.cs` | 成果物作成コマンド |
| `src/AutoPartsPM.Application/Features/Deliverables/Commands/CreateDeliverable/CreateDeliverableCommandHandler.cs` | 作成ハンドラー |
| `src/AutoPartsPM.Application/Features/Deliverables/Commands/CreateDeliverable/CreateDeliverableCommandValidator.cs` | バリデーター |
| `src/AutoPartsPM.Application/Features/Deliverables/Commands/UpdateDeliverable/UpdateDeliverableCommand.cs` | 成果物更新コマンド |
| `src/AutoPartsPM.Application/Features/Deliverables/Commands/UpdateDeliverable/UpdateDeliverableCommandHandler.cs` | 更新ハンドラー |
| `src/AutoPartsPM.Application/Features/Deliverables/Commands/DeleteDeliverable/DeleteDeliverableCommand.cs` | 成果物削除コマンド |
| `src/AutoPartsPM.Application/Features/Deliverables/Commands/DeleteDeliverable/DeleteDeliverableCommandHandler.cs` | 削除ハンドラー |
| `src/AutoPartsPM.Application/Features/Deliverables/Queries/GetDeliverableList/GetDeliverableListQuery.cs` | 成果物一覧クエリ + DTO |
| `src/AutoPartsPM.Application/Features/Deliverables/Queries/GetDeliverableList/GetDeliverableListQueryHandler.cs` | クエリハンドラー |
| `src/AutoPartsPM.Application/Features/Deliverables/Queries/GetDeliverableDetail/GetDeliverableDetailQuery.cs` | 成果物詳細クエリ + DTO |
| `src/AutoPartsPM.Application/Features/Deliverables/Queries/GetDeliverableDetail/GetDeliverableDetailQueryHandler.cs` | 詳細クエリハンドラー |

**実装内容:**
- Domain の `Deliverable` エンティティは既存（`IApplicationDbContext.Deliverables` DbSet あり）
- CRUD + フィルタ（ProjectId, PhaseId, Status）
- `DeliverableListDto`: Id, Name, Description, PhaseId, PhaseName, DueDate, Status, AssigneeName
- `DeliverableDetailDto`: 上記 + CreatedAt, UpdatedAt, Notes, FilePath

**修正ファイル:**

| ファイル | 変更内容 |
|---------|---------|
| `src/AutoPartsPM.Web/Components/Pages/Projects/ProjectDetail.razor` | 新しい「成果物」タブ追加 |
| `src/AutoPartsPM.Web/Components/_Imports.razor` | 新namespace追加 |

**成果物タブUI:**
- MudTable で一覧（Name, Phase, DueDate, Status, Assignee）
- インライン作成フォーム
- 各行: 編集（ダイアログ）、削除（確認付き）
- ステータス色分け: Pending=Default, InProgress=Primary, Completed=Success, Overdue=Error

### 2-3. ダッシュボード統計強化

**新規ファイル:**

| ファイル | 説明 |
|---------|------|
| `src/AutoPartsPM.Application/Features/Dashboard/Queries/GetDashboardData/GetDashboardDataQuery.cs` | ダッシュボードデータクエリ + DTO |
| `src/AutoPartsPM.Application/Features/Dashboard/Queries/GetDashboardData/GetDashboardDataQueryHandler.cs` | クエリハンドラー |

**実装内容:**
- `GetDashboardDataQuery` : IRequest<DashboardDataDto>（パラメータなし）
- `DashboardDataDto`:
  - TotalProjects, ActiveProjects, CompletedProjects, OnHoldProjects (int)
  - TotalIssues, OpenIssues, OverdueIssues (int)
  - UpcomingMilestones (List<UpcomingMilestoneDto>) — 7日以内の期限
  - PhaseDistribution (Dictionary<string, int>) — APQPフェーズ別プロジェクト数
  - StatusDistribution (Dictionary<string, int>) — ステータス別プロジェクト数
  - RecentProjects (List<RecentProjectDto>) — 最近更新された5件

**修正ファイル:**

| ファイル | 変更内容 |
|---------|---------|
| `src/AutoPartsPM.Web/Components/Pages/Dashboard/Index.razor` | 統計カード + チャート + リスト表示 |

**ダッシュボードUI:**
```
[プロジェクト数] [進行中] [課題数] [期限超過]  ← MudCard x 4 (統計サマリー)

[フェーズ分布チャート]  [ステータス分布チャート] ← MudChart (Donut)

[直近マイルストーン]                            ← MudSimpleTable (7日以内)
[期限超過課題リスト]                            ← MudSimpleTable (Open + 期限切れ)
[最近更新プロジェクト]                          ← MudSimpleTable (直近5件)
```

### P2 ファイルサマリー

| 種別 | ファイル数 |
|------|-----------|
| 新規 | 19 ファイル |
| 修正 | 3 ファイル |

---

## P3: 拡張機能（ファイル管理・レポート・監査ログ）

### 3-1. ファイルアップロード・管理

**新規ファイル:**

| ファイル | 説明 |
|---------|------|
| `src/AutoPartsPM.Application/Common/Interfaces/IFileStorageService.cs` | ファイルストレージインターフェース |
| `src/AutoPartsPM.Infrastructure/Services/LocalFileStorageService.cs` | ローカルストレージ実装 |

**実装内容:**
- Interface:
  ```csharp
  Task<string> UploadAsync(Stream stream, string fileName, string folder, CancellationToken ct);
  Task DeleteAsync(string filePath, CancellationToken ct);
  Stream? GetFileStream(string filePath);
  ```
- 実装: `wwwroot/uploads/{folder}/{guid}_{fileName}` にローカル保存
- DI登録: `Infrastructure/DependencyInjection.cs` に `AddScoped<IFileStorageService, LocalFileStorageService>`

**修正ファイル:**

| ファイル | 変更内容 |
|---------|---------|
| `src/AutoPartsPM.Infrastructure/DependencyInjection.cs` | サービス登録追加 |
| PPAP 更新コマンド / 成果物更新コマンド | FilePath フィールド追加 |
| `PPAPList.razor` / ProjectDetail成果物タブ | `MudFileUpload` コンポーネント追加 |

### 3-2. Excel レポート出力

**新規ファイル:**

| ファイル | 説明 |
|---------|------|
| `src/AutoPartsPM.Application/Common/Interfaces/IReportService.cs` | レポートサービスインターフェース |
| `src/AutoPartsPM.Infrastructure/Services/ExcelReportService.cs` | ClosedXML による Excel 生成 |
| `src/AutoPartsPM.Web/Components/Pages/Reports/ProjectReport.razor` | レポートダウンロードページ |

**実装内容:**
- ClosedXML パッケージ追加（`Infrastructure.csproj`）
- `IReportService`:
  ```csharp
  Task<byte[]> GenerateProjectReportAsync(int projectId, CancellationToken ct);
  Task<byte[]> GenerateProjectSummaryReportAsync(CancellationToken ct);
  ```
- Excel レポート内容（シート別）:
  1. 「プロジェクト概要」: 基本情報
  2. 「APQPフェーズ」: 5フェーズの計画/実績
  3. 「マイルストーン」: 一覧 + 期限/実績
  4. 「課題」: オープン課題一覧
  5. 「設備」: 設備一覧 + ステータス
- UI: プロジェクト選択ドロップダウン → 「Excelダウンロード」ボタン → `IJSRuntime.InvokeVoidAsync("downloadFile", ...)` でダウンロード

**修正ファイル:**

| ファイル | 変更内容 |
|---------|---------|
| `src/AutoPartsPM.Infrastructure/AutoPartsPM.Infrastructure.csproj` | ClosedXML パッケージ追加 |
| `src/AutoPartsPM.Infrastructure/DependencyInjection.cs` | サービス登録追加 |
| `src/AutoPartsPM.Web/Components/Layout/NavMenu.razor` | 「レポート」メニュー追加 |

### 3-3. 監査ログ

**新規ファイル:**

| ファイル | 説明 |
|---------|------|
| `src/AutoPartsPM.Domain/Entities/AuditLog.cs` | 監査ログエンティティ |
| `src/AutoPartsPM.Infrastructure/Persistence/Configurations/AuditLogConfiguration.cs` | EF Core 設定 |
| `src/AutoPartsPM.Infrastructure/Persistence/Interceptors/AuditSaveChangesInterceptor.cs` | SaveChanges インターセプター |
| `src/AutoPartsPM.Application/Features/AuditLogs/Queries/GetAuditLogs/GetAuditLogsQuery.cs` | 監査ログクエリ + DTO |
| `src/AutoPartsPM.Application/Features/AuditLogs/Queries/GetAuditLogs/GetAuditLogsQueryHandler.cs` | クエリハンドラー |
| `src/AutoPartsPM.Web/Components/Pages/Admin/AuditViewer.razor` | 監査ログビューアページ |

**実装内容:**
- `AuditLog` エンティティ:
  ```csharp
  public class AuditLog
  {
      public int Id { get; set; }
      public string EntityName { get; set; }     // "Project", "Issue" 等
      public int EntityId { get; set; }
      public string Action { get; set; }          // "Created", "Updated", "Deleted"
      public string? UserId { get; set; }
      public string? UserName { get; set; }
      public DateTime Timestamp { get; set; }
      public string? Changes { get; set; }        // JSON: {"PropertyName": {"Old": "x", "New": "y"}}
  }
  ```
- `AuditSaveChangesInterceptor`: EF Core の `SaveChangesInterceptor` を継承
  - `SavingChangesAsync` でChangeTrackerから Added/Modified/Deleted エンティティを検出
  - 各変更を `AuditLog` レコードとして保存
  - `ICurrentUserService` から操作ユーザー情報取得
- `GetAuditLogsQuery`: フィルタ（EntityName, Action, DateFrom, DateTo, UserName）+ ページング
- `AuditViewer.razor`: MudTable + フィルターパネル（管理者のみ: `@attribute [Authorize(Policy = "AdminOnly")]`）

**修正ファイル:**

| ファイル | 変更内容 |
|---------|---------|
| `src/AutoPartsPM.Application/Common/Interfaces/IApplicationDbContext.cs` | `DbSet<AuditLog>` 追加 |
| `src/AutoPartsPM.Infrastructure/Persistence/ApplicationDbContext.cs` | AuditLog 設定 + Interceptor 登録 |
| `src/AutoPartsPM.Infrastructure/DependencyInjection.cs` | Interceptor DI 登録 |
| `src/AutoPartsPM.Web/Components/_Imports.razor` | 新namespace追加 |
| `src/AutoPartsPM.Web/Components/Layout/NavMenu.razor` | 管理者メニューに「監査ログ」追加 |

### P3 ファイルサマリー

| 種別 | ファイル数 |
|------|-----------|
| 新規 | 11 ファイル |
| 修正 | 8 ファイル |

---

## 実装パターン（コーディング規約）

既存コードから抽出したパターン。全新規ファイルはこれに従うこと。

### Command パターン

```csharp
namespace AutoPartsPM.Application.Features.Xxx.Commands.YyyXxx;

// record + { get; set; } で @bind-Value 対応
public record YyyXxxCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
```

### Handler パターン

```csharp
namespace AutoPartsPM.Application.Features.Xxx.Commands.YyyXxx;

// Primary constructor で DI
public class YyyXxxCommandHandler(IApplicationDbContext context)
    : IRequestHandler<YyyXxxCommand, Result<int>>
{
    public async Task<Result<int>> Handle(YyyXxxCommand request, CancellationToken cancellationToken)
    {
        // Result<T>.Success(id) / Result<T>.Failure("msg") を返す
        var entity = await context.XxxSet.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity is null)
            return Result<int>.Failure("見つかりません");

        // プロパティ更新
        entity.Name = request.Name;

        await context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(entity.Id);
    }
}
```

### Validator パターン

```csharp
namespace AutoPartsPM.Application.Features.Xxx.Commands.YyyXxx;

public class YyyXxxCommandValidator : AbstractValidator<YyyXxxCommand>
{
    public YyyXxxCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("名前は必須です")
            .MaximumLength(200).WithMessage("名前は200文字以内で入力してください");
    }
}
```

### Query + DTO パターン

```csharp
namespace AutoPartsPM.Application.Features.Xxx.Queries.GetXxxList;

public record GetXxxListQuery : IRequest<List<XxxListDto>>
{
    public string? SearchTerm { get; set; }
    public XxxStatus? Status { get; set; }
}

public class XxxListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    // ...
}
```

### Blazor ページパターン

```razor
@page "/xxx"
@attribute [Authorize]
@inject IMediator Mediator
@inject NavigationManager Navigation
@inject ISnackbar Snackbar
@using AutoPartsPM.Application.Features.Xxx.Queries.GetXxxList

<PageTitle>Xxx一覧 - AutoParts PM</PageTitle>

<!-- 日本語ラベル、MudBlazor コンポーネント -->
<!-- MudTable には T="XxxDto" 属性を必ず指定 -->
<!-- @onclick:stopPropagation="true" で行クリックイベント抑止 -->
<!-- _loading / _creating / _saving フラグで UI 制御 -->
```

### 重要な注意事項

1. **`init` vs `set`**: Blazor `@bind-Value` で使う Command の record プロパティは `{ get; set; }` にする（`init` だとCS8852エラー）
2. **`MudTable T属性`**: `<MudTable T="XxxDto">` を必ず指定（型推論失敗防止）
3. **`OnRowClick`**: `TableRowClickEventArgs<XxxDto>` で型付きハンドラーメソッドを使用
4. **削除ボタンの行クリック抑止**: `<MudTd @onclick:stopPropagation="true">` で囲む
5. **ファイルスコープ namespace**: `namespace Xxx;` 形式（ブロック不要）
6. **日本語**: UIラベル、バリデーションメッセージ、Snackbar通知すべて日本語

---

## 検証手順

### ビルド確認（各ステップ完了後に実施）

```bash
cd /c/Users/daigo/source/project
dotnet build src/AutoPartsPM.Web/AutoPartsPM.Web.csproj
```
- 0 errors を確認（warnings は許容）

### 機能確認

**P1 完了後:**
1. `/projects/create` でプロジェクト新規作成
2. `/projects/{id}` で基本情報の編集・保存
3. APQPフェーズの進捗率更新 → OverallProgress 自動再計算の確認

**P2 完了後:**
4. ProjectDetail「メンバー」タブでメンバー追加/削除
5. ProjectDetail「成果物」タブで成果物 CRUD
6. `/` ダッシュボードで統計カード・チャート・リスト表示

**P3 完了後:**
7. PPAP/成果物にファイルアップロード・ダウンロード
8. `/reports` でプロジェクト別Excelダウンロード
9. `/admin/audit` で操作履歴の確認（フィルタ動作含む）

### EF Core マイグレーション（P3 で AuditLog 追加時）

```bash
dotnet ef migrations add AddAuditLog -p src/AutoPartsPM.Infrastructure -s src/AutoPartsPM.Web
dotnet ef database update -p src/AutoPartsPM.Infrastructure -s src/AutoPartsPM.Web
```

---

## 実装順序（推奨）

| 順番 | 区分 | 内容 | 新規ファイル | 修正ファイル |
|------|------|------|-------------|-------------|
| 1 | P1-1 | CreateProjectCommand の init→set 修正 + ProjectCreate.razor 作成 | 1 | 1 |
| 2 | P1-2 | UpdateProject Command/Handler/Validator + ProjectDetail 編集モード | 3 | 1 |
| 3 | P1-3 | UpdateProjectPhase Command/Handler/Validator + ProjectDetail フェーズ編集 | 3 | 1 |
| 4 | P2-1 | ProjectMembers CRUD + ProjectDetail メンバータブ | 6 | 2 |
| 5 | P2-2 | Deliverables CRUD + ProjectDetail 成果物タブ | 11 | 2 |
| 6 | P2-3 | Dashboard Query/Handler + Dashboard/Index.razor 強化 | 2 | 1 |
| 7 | P3-1 | IFileStorageService + LocalFileStorageService + UI統合 | 2 | 3 |
| 8 | P3-2 | IReportService + ExcelReportService + ProjectReport.razor | 3 | 3 |
| 9 | P3-3 | AuditLog Entity + Interceptor + AuditViewer.razor | 6 | 5 |

**合計: 新規 37 ファイル / 修正 19 ファイル**

各ステップ完了後に `dotnet build` で 0 errors を確認してから次へ進むこと。

---

## 主要参照ファイル（実装時に読むべきファイル）

| ファイル | 参照理由 |
|---------|---------|
| `src/AutoPartsPM.Application/Features/Projects/Commands/CreateProject/CreateProjectCommand.cs` | Command record の正式パターン |
| `src/AutoPartsPM.Application/Features/Projects/Commands/CreateProject/CreateProjectCommandHandler.cs` | Handler パターン（フェーズ/PPAP/ゲートレビュー自動生成） |
| `src/AutoPartsPM.Application/Features/Projects/Queries/GetProjectList/GetProjectListQueryHandler.cs` | Query handler パターン（Include, Filter, Select, ToList） |
| `src/AutoPartsPM.Web/Components/Pages/Customers/CustomerDetail.razor` | 詳細+編集切替UIパターン |
| `src/AutoPartsPM.Web/Components/Pages/Customers/CustomerList.razor` | 一覧+インライン作成+削除確認パターン |
| `src/AutoPartsPM.Web/Components/Pages/Projects/ProjectDetail.razor` | 現在のタブ構成（修正対象） |
| `src/AutoPartsPM.Application/Common/Interfaces/IApplicationDbContext.cs` | 利用可能な DbSet 一覧 |
| `src/AutoPartsPM.Domain/Entities/` | 全エンティティ定義 |
| `src/AutoPartsPM.Domain/Enums/` | 全列挙型定義 |
