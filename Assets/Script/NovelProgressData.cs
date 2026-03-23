using UnityEngine;

/// <summary>
/// シーンをまたいでノベルの進行データを保持する静的クラス。
/// WebGL対応のためPlayerPrefsを使用しています。
/// </summary>
public static class NovelProgressData
{
    private const string LastChoiceKey = "LastChoiceId";
    private const string JumpToKey = "JumpToId";

    /// <summary>直前の選択肢のID</summary>
    public static int LastChoiceId
    {
        get => PlayerPrefs.GetInt(LastChoiceKey, 0);
        set => PlayerPrefs.SetInt(LastChoiceKey, value);
    }

    /// <summary>ジャンプ先のID（0なら通常開始）</summary>
    public static int JumpToId
    {
        get => PlayerPrefs.GetInt(JumpToKey, 0);
        set => PlayerPrefs.SetInt(JumpToKey, value);
    }
}