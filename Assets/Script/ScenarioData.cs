using System.Collections.Generic;

/// <summary>
/// シナリオ1行分のデータ構造。
/// CSVの1行が1つのScenarioLineに対応します。
/// </summary>
[System.Serializable]
public class ScenarioLine
{
    /// <summary>行を識別するID</summary>
    public int id;

    /// <summary>話しているキャラクター名</summary>
    public string character;

    /// <summary>表示するスプライト名</summary>
    public string sprite;

    /// <summary>表示するテキスト</summary>
    public string text;
    /// <summary>次に進むID（0なら id+1 に自動で進む）</summary>
    public int nextId;



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

    /// <summary>次に切り替わるキャラクター名（なければ空）</summary>
    public string nextCharacter;

    /// <summary>選択肢があるかどうか</summary>
    public bool HasChoice => !string.IsNullOrEmpty(choice1Text);

    /// <summary>カクテル提供コマンドがあるかどうか</summary>
    public bool HasCocktail => !string.IsNullOrEmpty(cocktail);
    /// <summary>カクテル分岐があるかどうか</summary>
    public bool HasCocktailBranch => cocktailShortNext > 0;


    /// <summary>次のキャラクターへの切り替えがあるかどうか</summary>
    public bool HasNextCharacter => !string.IsNullOrEmpty(nextCharacter);

    /// <summary>振りが少ないときに飛ぶID</summary>
    public int cocktailShortNext;

    /// <summary>振りがちょうどのときに飛ぶID</summary>
    public int cocktailJustNext;

    /// <summary>振りが多いときに飛ぶID</summary>
    public int cocktailLongNext;

    /// <summary>ちょうどの最小シェイク時間（秒）</summary>
    public float cocktailMinTime;

    /// <summary>ちょうどの最大シェイク時間（秒）</summary>
    public float cocktailMaxTime;
    /// <summary>好感度分岐の閾値（0なら分岐なし）</summary>
    public int affinityThreshold;

    /// <summary>好感度が閾値以上のときに飛ぶID</summary>
    public int affinityNext;

    /// <summary>好感度分岐があるかどうか</summary>
    public bool HasAffinityBranch => affinityThreshold > 0 && affinityNext > 0;

    /// <summary>遷移するシーン名（空なら遷移しない）</summary>
    public string nextScene;

    /// <summary>シーン遷移があるかどうか</summary>
    public bool HasNextScene => !string.IsNullOrEmpty(nextScene);

}

/// <summary>
/// シナリオ全体を保持するクラス。
/// </summary>
[System.Serializable]
public class ScenarioData
{
    /// <summary>IDをキーにしたシナリオ行の辞書</summary>
    public Dictionary<int, ScenarioLine> lines = new();
}