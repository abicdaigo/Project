namespace AutoPartsPM.Domain.Enums;

/// <summary>
/// APQP (先行製品品質計画) の5フェーズ
/// </summary>
public enum APQPPhase
{
    /// <summary>計画と定義</summary>
    Planning = 1,

    /// <summary>製品設計・開発</summary>
    ProductDesign = 2,

    /// <summary>工程設計・開発</summary>
    ProcessDesign = 3,

    /// <summary>製品・工程の妥当性確認</summary>
    Validation = 4,

    /// <summary>フィードバック・評価・是正処置（量産）</summary>
    Production = 5
}
