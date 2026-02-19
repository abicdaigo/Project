# プロジェクト管理システム - アーキテクチャ設計書

## 1. システム概要

自動車部品メーカー向けの統合プロジェクト管理Webシステム。
顧客からの受注（品番/型式単位）から量産立ち上げまでのプロジェクトライフサイクル全体を
APQP（先行製品品質計画）の5フェーズに基づき一元管理する。

## 2. 技術スタック

| レイヤー | 技術 |
|----------|------|
| フロントエンド | Blazor Server + MudBlazor |
| バックエンド | ASP.NET Core (.NET 9) |
| アーキテクチャ | Clean Architecture + CQRS (MediatR) |
| 認証 | Windows認証 (Negotiate/Kerberos) on IIS |
| 認可 | ADグループベースRBAC + ポリシーベース認可 |
| データベース | SQL Server |
| ORM | Entity Framework Core |
| リアルタイム通知 | SignalR (Blazor Server内蔵) |
| バリデーション | FluentValidation |
| マッピング | AutoMapper |
| ロギング | Serilog |
| テスト | xUnit + NSubstitute |

### 技術選定理由

- **Blazor Server**: イントラネット環境でのリッチUI、C#統一開発、SignalR内蔵によるリアルタイム更新、Windows認証との親和性が最高
- **Clean Architecture + CQRS**: ドメインロジックの保護、読み書きの分離による複雑なクエリへの対応、テスタビリティの確保
- **SQL Server**: Windows環境との親和性、Entity Framework Coreとの統合、トランザクション管理

## 3. ソリューション構成

```
AutoPartsPM/
├── src/
│   ├── AutoPartsPM.Domain/              # ドメイン層（依存なし）
│   │   ├── Entities/                     # エンティティ
│   │   ├── ValueObjects/                 # 値オブジェクト
│   │   ├── Enums/                        # 列挙型
│   │   ├── Events/                       # ドメインイベント
│   │   └── Interfaces/                   # リポジトリインターフェース
│   │
│   ├── AutoPartsPM.Application/         # アプリケーション層
│   │   ├── Features/                     # 機能別CQRS
│   │   │   ├── Projects/                 # プロジェクト管理
│   │   │   ├── Customers/                # 顧客管理
│   │   │   ├── GateReviews/              # ゲートレビュー
│   │   │   ├── Documents/                # 文書管理
│   │   │   ├── PPAP/                     # PPAP管理
│   │   │   ├── Schedules/                # スケジュール管理
│   │   │   ├── Equipment/                # 設備管理
│   │   │   └── Issues/                   # 課題管理
│   │   ├── Common/                       # 共通インターフェース・DTO
│   │   ├── Behaviors/                    # MediatRパイプライン
│   │   └── Mappings/                     # AutoMapperプロファイル
│   │
│   ├── AutoPartsPM.Infrastructure/      # インフラストラクチャ層
│   │   ├── Persistence/                  # EF Core DbContext・リポジトリ実装
│   │   │   ├── Configurations/           # エンティティ構成
│   │   │   ├── Migrations/               # DBマイグレーション
│   │   │   └── Repositories/             # リポジトリ実装
│   │   ├── Services/                     # 外部サービス（メール・ファイル等）
│   │   └── Identity/                     # 認証・認可サービス
│   │
│   └── AutoPartsPM.Web/                 # プレゼンテーション層 (Blazor Server)
│       ├── Components/                   # Blazorコンポーネント
│       │   ├── Layout/                   # レイアウト
│       │   ├── Pages/                    # ページコンポーネント
│       │   │   ├── Dashboard/            # ダッシュボード
│       │   │   ├── Projects/             # プロジェクト管理画面
│       │   │   ├── Customers/            # 顧客管理画面
│       │   │   ├── GateReviews/          # ゲートレビュー画面
│       │   │   ├── Documents/            # 文書管理画面
│       │   │   ├── PPAP/                 # PPAP管理画面
│       │   │   ├── Schedules/            # スケジュール画面
│       │   │   ├── Equipment/            # 設備管理画面
│       │   │   ├── Issues/               # 課題管理画面
│       │   │   └── Admin/                # 管理画面
│       │   └── Shared/                   # 共有コンポーネント
│       ├── wwwroot/                      # 静的ファイル
│       └── Program.cs                    # エントリポイント
│
├── tests/
│   ├── AutoPartsPM.Domain.Tests/
│   ├── AutoPartsPM.Application.Tests/
│   └── AutoPartsPM.Web.Tests/
│
├── docs/                                 # ドキュメント
└── AutoPartsPM.sln                       # ソリューションファイル
```

## 4. 依存関係の方向

```
Domain（最内層 - 依存なし）
  ↑
Application（Domain のみに依存）
  ↑
Infrastructure（Application + Domain に依存）
  ↑
Web（Application に依存、Infrastructure は DI 経由で注入）
```

## 5. ドメインモデル設計

### 5.1 主要エンティティ

#### プロジェクト (Project)
プロジェクトの中核エンティティ。品番・型式単位で管理。

| プロパティ | 型 | 説明 |
|-----------|------|------|
| Id | int | プロジェクトID |
| ProjectCode | string | プロジェクトコード（社内管理番号） |
| ProjectName | string | プロジェクト名 |
| CustomerId | int | 顧客ID (FK) |
| PartNumber | string | 品番 |
| ModelCode | string | 型式コード |
| Status | ProjectStatus | プロジェクトステータス |
| CurrentPhase | APQPPhase | 現在のAPQPフェーズ |
| SOPDate | DateTime? | 量産開始目標日 |
| Description | string? | 概要・備考 |
| ProjectManagerId | string | プロジェクトマネージャー（AD User） |
| CreatedAt | DateTime | 作成日時 |
| UpdatedAt | DateTime | 更新日時 |

#### 顧客 (Customer)

| プロパティ | 型 | 説明 |
|-----------|------|------|
| Id | int | 顧客ID |
| CustomerCode | string | 顧客コード |
| CustomerName | string | 顧客名 |
| ContactPerson | string? | 担当者名 |
| Email | string? | メールアドレス |
| Phone | string? | 電話番号 |
| Address | string? | 住所 |

#### APQPフェーズ進捗 (ProjectPhase)
APQPの5フェーズごとの進捗を管理。

| プロパティ | 型 | 説明 |
|-----------|------|------|
| Id | int | フェーズ進捗ID |
| ProjectId | int | プロジェクトID (FK) |
| Phase | APQPPhase | APQPフェーズ |
| Status | PhaseStatus | フェーズステータス |
| PlannedStartDate | DateTime? | 計画開始日 |
| PlannedEndDate | DateTime? | 計画終了日 |
| ActualStartDate | DateTime? | 実績開始日 |
| ActualEndDate | DateTime? | 実績終了日 |
| CompletionRate | decimal | 進捗率 (0-100) |

#### ゲートレビュー (GateReview)
各フェーズ間のゲートレビュー（関門判定）。

| プロパティ | 型 | 説明 |
|-----------|------|------|
| Id | int | ゲートレビューID |
| ProjectId | int | プロジェクトID (FK) |
| GateNumber | int | ゲート番号 (1-5) |
| ReviewDate | DateTime? | レビュー実施日 |
| Result | GateResult | 判定結果 |
| ReviewerId | string | レビュアー（AD User） |
| Notes | string? | コメント・備考 |

#### PPAP文書 (PPAPDocument)
PPAP 18書類の管理。

| プロパティ | 型 | 説明 |
|-----------|------|------|
| Id | int | 文書ID |
| ProjectId | int | プロジェクトID (FK) |
| ElementNumber | int | PPAP要素番号 (1-18) |
| ElementName | string | 要素名 |
| Status | DocumentStatus | 文書ステータス |
| FilePath | string? | ファイルパス |
| SubmissionLevel | PPAPLevel | 提出レベル (1-5) |
| Required | bool | 必須か |
| ApprovedDate | DateTime? | 承認日 |
| ApprovedBy | string? | 承認者 |

#### マイルストーン (Milestone)
プロジェクトのマイルストーン管理。

| プロパティ | 型 | 説明 |
|-----------|------|------|
| Id | int | マイルストーンID |
| ProjectId | int | プロジェクトID (FK) |
| Name | string | マイルストーン名 |
| DueDate | DateTime | 期限日 |
| ActualDate | DateTime? | 実績日 |
| Status | MilestoneStatus | ステータス |
| Phase | APQPPhase | 対応フェーズ |

#### 課題 (Issue)
プロジェクト課題・アクションアイテムの管理。

| プロパティ | 型 | 説明 |
|-----------|------|------|
| Id | int | 課題ID |
| ProjectId | int | プロジェクトID (FK) |
| Title | string | 課題タイトル |
| Description | string? | 詳細説明 |
| Priority | IssuePriority | 優先度 |
| Status | IssueStatus | ステータス |
| AssigneeId | string | 担当者（AD User） |
| DueDate | DateTime? | 期限 |
| Category | IssueCategory | カテゴリ |

#### 成果物 (Deliverable)
各フェーズで必要な成果物の管理。

| プロパティ | 型 | 説明 |
|-----------|------|------|
| Id | int | 成果物ID |
| ProjectPhaseId | int | フェーズ進捗ID (FK) |
| Name | string | 成果物名 |
| Description | string? | 説明 |
| Status | DeliverableStatus | ステータス |
| DueDate | DateTime? | 期限 |
| FilePath | string? | ファイルパス |
| CompletedDate | DateTime? | 完了日 |

#### 設備 (Equipment)
金型・治具・検査機器等の設備管理。

| プロパティ | 型 | 説明 |
|-----------|------|------|
| Id | int | 設備ID |
| ProjectId | int | プロジェクトID (FK) |
| EquipmentCode | string | 設備コード |
| Name | string | 設備名 |
| Type | EquipmentType | 設備種別（金型/治具/検査機器等） |
| Status | EquipmentStatus | ステータス |
| Supplier | string? | 設備メーカー |
| OrderDate | DateTime? | 発注日 |
| DeliveryDate | DateTime? | 納期 |
| Location | string? | 設置場所 |

### 5.2 列挙型

```csharp
// APQPフェーズ
enum APQPPhase { Planning, ProductDesign, ProcessDesign, Validation, Production }

// プロジェクトステータス
enum ProjectStatus { Draft, Active, OnHold, Completed, Cancelled }

// フェーズステータス
enum PhaseStatus { NotStarted, InProgress, UnderReview, Completed, Delayed }

// ゲート判定結果
enum GateResult { NotReviewed, Approved, ConditionalApproval, Rejected }

// 文書ステータス
enum DocumentStatus { NotStarted, InProgress, UnderReview, Approved, Rejected }

// PPAPレベル
enum PPAPLevel { Level1, Level2, Level3, Level4, Level5 }

// マイルストーンステータス
enum MilestoneStatus { Pending, OnTrack, AtRisk, Delayed, Completed }

// 課題優先度
enum IssuePriority { Low, Medium, High, Critical }

// 課題ステータス
enum IssueStatus { Open, InProgress, Resolved, Closed }

// 課題カテゴリ
enum IssueCategory { Design, Process, Quality, Equipment, Supplier, Other }

// 成果物ステータス
enum DeliverableStatus { NotStarted, InProgress, UnderReview, Completed }

// 設備種別
enum EquipmentType { Mold, Jig, Fixture, InspectionEquipment, ProductionMachine, Other }

// 設備ステータス
enum EquipmentStatus { Planning, Ordered, Manufacturing, Delivered, Installed, Operational }
```

## 6. 機能一覧

### 6.1 ダッシュボード
- プロジェクト一覧（ステータス別集計）
- 遅延プロジェクトのアラート表示
- 直近のマイルストーン・期限一覧
- ゲートレビュー予定
- PPAP提出状況サマリー
- 課題件数サマリー（優先度別）

### 6.2 プロジェクト管理
- プロジェクトの登録・編集・削除
- プロジェクト詳細画面（APQP 5フェーズの進捗可視化）
- プロジェクト一覧（フィルタ・ソート・検索）
- プロジェクトメンバー管理
- プロジェクトコピー（テンプレートとして再利用）

### 6.3 APQPフェーズ管理
- 5フェーズの進捗管理（各フェーズの成果物チェックリスト）
- フェーズ移行のワークフロー
- 各フェーズの成果物管理
- 進捗率の自動計算

### 6.4 ゲートレビュー管理
- ゲートレビューの計画・実施・記録
- 判定結果の登録（承認/条件付承認/却下）
- レビュー資料の添付
- 過去レビュー履歴の参照

### 6.5 スケジュール管理
- マイルストーン管理
- ガントチャート表示
- 計画 vs 実績の比較
- 遅延アラート・通知

### 6.6 PPAP管理
- PPAP 18書類のチェックリスト管理
- 提出レベル別の必要書類自動設定
- 各書類のステータス管理・ファイル添付
- PPAP進捗ダッシュボード
- PSW（部品提出保証書）の作成・管理

### 6.7 文書管理
- 文書のアップロード・ダウンロード
- バージョン管理
- 文書カテゴリ分類
- 検索機能
- アクセス権限管理

### 6.8 課題管理
- 課題の登録・編集・クローズ
- 担当者アサイン
- 優先度・カテゴリ分類
- 期限管理・アラート
- 対策記録

### 6.9 設備管理
- 金型・治具・検査機器の登録・管理
- 発注〜設置までの進捗追跡
- 設備一覧（プロジェクト別/種別/ステータス別）

### 6.10 顧客管理
- 顧客マスタの登録・編集
- 顧客別プロジェクト一覧
- 顧客固有要件（CSR）の管理

### 6.11 マスタ管理（管理者機能）
- ユーザー権限管理（ADグループ連携）
- APQPテンプレート管理（フェーズ別の標準成果物定義）
- 通知設定
- システム設定

## 7. 認証・認可設計

### ロール定義（ADグループ対応）

| ロール | ADグループ例 | 権限 |
|--------|------------|------|
| Admin | PM_Admins | 全機能＋システム管理 |
| Manager | PM_Managers | プロジェクト作成・編集・ゲート判定承認 |
| Member | PM_Members | 担当プロジェクトの編集・文書アップロード |
| Viewer | PM_Viewers | 閲覧のみ |

## 8. 画面構成

```
/                           → ダッシュボード
/projects                   → プロジェクト一覧
/projects/{id}              → プロジェクト詳細
/projects/{id}/phases       → APQPフェーズ管理
/projects/{id}/gates        → ゲートレビュー
/projects/{id}/schedule     → スケジュール（ガントチャート）
/projects/{id}/ppap         → PPAP管理
/projects/{id}/documents    → 文書管理
/projects/{id}/issues       → 課題管理
/projects/{id}/equipment    → 設備管理
/customers                  → 顧客一覧
/customers/{id}             → 顧客詳細
/admin/users                → ユーザー管理
/admin/templates            → テンプレート管理
/admin/settings             → システム設定
```
