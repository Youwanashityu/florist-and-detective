/// <summary>
/// シーンをまたいでノベルの進行データを保持する静的クラス。
/// </summary>
public static class NovelProgressData
{
    /// <summary>直前の選択肢のID</summary>
    public static int LastChoiceId = 0;

    /// <summary>ジャンプ先のID（0なら通常開始）</summary>
    public static int JumpToId = 0;
}