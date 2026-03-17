using System.Collections.Generic;

/// <summary>
/// シナリオ1行分のデータ構造。
/// CSVの1行が1つのScenarioLineに対応します。
/// </summary>
[System.Serializable]
public class ScenarioLine
{
    /// <summary>行を識別するID（次の行の指定に使用）</summary>
    public int id;

    /// <summary>話しているキャラクター名</summary>
    public string character;

    /// <summary>表示するスプライト名</summary>
    public string sprite;

    /// <summary>表示するテキスト</summary>
    public string text;

    /// <summary>選択肢1のテキスト（なければ空）</summary>
    public string choice1Text;

    /// <summary>選択肢1を選んだときに進むID</summary>
    public int choice1Next;

    /// <summary>選択肢1を選んだときの好感度変動</summary>
    public int choice1Affinity;

    /// <summary>選択肢2のテキスト（なければ空）</summary>
    public string choice2Text;

    /// <summary>選択肢2を選んだときに進むID</summary>
    public int choice2Next;

    /// <summary>選択肢2を選んだときの好感度変動</summary>
    public int choice2Affinity;

    /// <summary>再生するBGM名（変更なければ空）</summary>
    public string bgm;

    /// <summary>再生するSE名（なければ空）</summary>
    public string se;

    /// <summary>提供するカクテル名（なければ空）</summary>
    public string cocktail;

    /// <summary>選択肢があるかどうか</summary>
    public bool HasChoice => !string.IsNullOrEmpty(choice1Text);

    /// <summary>カクテル提供コマンドがあるかどうか</summary>
    public bool HasCocktail => !string.IsNullOrEmpty(cocktail);
}

/// <summary>
/// 1人の客のシナリオ全体を保持するクラス。
/// </summary>
[System.Serializable]
public class ScenarioData
{
    /// <summary>客の名前</summary>
    public string characterName;

    /// <summary>IDをキーにしたシナリオ行の辞書</summary>
    public Dictionary<int, ScenarioLine> lines = new();
}