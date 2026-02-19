using AutoPartsPM.Domain.Enums;

namespace AutoPartsPM.Domain.Entities;

public class PPAPDocument : BaseEntity
{
    public int ProjectId { get; set; }
    public int ElementNumber { get; set; }
    public string ElementName { get; set; } = string.Empty;
    public DocumentStatus Status { get; set; } = DocumentStatus.NotStarted;
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public PPAPLevel SubmissionLevel { get; set; } = PPAPLevel.Level3;
    public bool Required { get; set; } = true;
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;

    /// <summary>
    /// PPAP 18要素の標準名称を返す
    /// </summary>
    public static string GetElementName(int elementNumber) => elementNumber switch
    {
        1 => "設計記録",
        2 => "認可された設計変更文書",
        3 => "顧客技術承認",
        4 => "設計FMEA (DFMEA)",
        5 => "工程フロー図",
        6 => "工程FMEA (PFMEA)",
        7 => "コントロールプラン",
        8 => "測定システム解析 (MSA)",
        9 => "寸法測定結果",
        10 => "材料・性能試験結果",
        11 => "初期工程調査",
        12 => "認定試験所文書",
        13 => "外観承認報告書 (AAR)",
        14 => "量産サンプル部品",
        15 => "マスターサンプル",
        16 => "検査補助具",
        17 => "顧客固有要件",
        18 => "部品提出保証書 (PSW)",
        _ => throw new ArgumentOutOfRangeException(nameof(elementNumber))
    };
}
