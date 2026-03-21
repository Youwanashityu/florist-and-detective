using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シナリオの進行・好感度・キャラクター切り替えを管理するクラス。
/// </summary>
public class NovelManager : MonoBehaviour
{
    // -------------------------------------------------------
    // シングルトン
    // -------------------------------------------------------

    /// <summary>唯一のインスタンス（読み取り専用）</summary>
    public static NovelManager Instance { get; private set; }

    // -------------------------------------------------------
    // インスペクター設定
    // -------------------------------------------------------

    [Header("最初のシナリオID")]
    [SerializeField] private int startId = 1;

    // -------------------------------------------------------
    // 内部状態
    // -------------------------------------------------------

    /// <summary>シナリオデータ</summary>
    private ScenarioData scenarioData;

    /// <summary>現在表示中の行</summary>
    private ScenarioLine currentLine;

    /// <summary>現在会話中のキャラクター名</summary>
    private string currentCharacter;

    /// <summary>客ごとの好感度を管理する辞書</summary>
    private Dictionary<string, int> affinityDict = new();

    // -------------------------------------------------------
    // ライフサイクル
    // -------------------------------------------------------

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        scenarioData = ScenarioLoader.Load();

        if (scenarioData == null)
        {
            Debug.LogWarning("[NovelManager] シナリオの読み込みに失敗しました。");
            return;
        }

        GoToLine(startId);
    }

    // -------------------------------------------------------
    // シナリオ進行
    // -------------------------------------------------------

    /// <summary>
    /// 指定IDの行に移動して表示します。
    /// </summary>
    public void GoToLine(int id)
    {
        if (scenarioData == null) return;

        if (!scenarioData.lines.TryGetValue(id, out currentLine))
        {
            Debug.LogWarning($"[NovelManager] ID:{id} の行が見つかりません。");
            return;
        }

        // キャラクター切り替え
        if (!string.IsNullOrEmpty(currentLine.character) && currentLine.character != currentCharacter)
        {
            currentCharacter = currentLine.character;

            // 新しい客の好感度を初期化
            if (!affinityDict.ContainsKey(currentCharacter))
                affinityDict[currentCharacter] = 0;
        }

        // 好感度自動分岐チェック
        if (currentLine.HasAffinityBranch && GetCurrentAffinity() >= currentLine.affinityThreshold)
        {
            GoToLine(currentLine.affinityNext);
            return;
        }

        // BGM切り替え
        if (!string.IsNullOrEmpty(currentLine.bgm))
            PlayBGM(currentLine.bgm);

        // SE再生
        if (!string.IsNullOrEmpty(currentLine.se))
            SoundManager.Instance?.PlaySE(currentLine.se);
        // カクテル提供（GoToLine内のこの部分を変更）
        if (currentLine.HasCocktail)
        {
            CocktailManager.Instance?.ServeCocktail(
                currentLine.cocktail,
                currentLine.cocktailMinTime,
                currentLine.cocktailMaxTime,
                currentLine.cocktailShortNext,
                currentLine.cocktailJustNext,
                currentLine.cocktailLongNext
            );
            return;
        }
        // UIに表示を依頼
        NovelUI.Instance?.ShowLine(currentLine, GetCurrentAffinity());

        // シーン遷移チェック（UIに表示した後に追加）
        if (currentLine.HasNextScene)
        {
            SceneManager.LoadScene(currentLine.nextScene);
            return;
        }
    }



    /// <summary>
    /// 次の行に進みます。
    /// </summary>
    public void NextLine()
    {
        if (currentLine == null) return;
        if (currentLine.HasChoice) return;

        if (currentLine.HasNextCharacter)
            currentCharacter = currentLine.nextCharacter;

        // next_idが指定されていればそこへ、なければid+1へ
        int nextId = currentLine.nextId > 0 ? currentLine.nextId : currentLine.id + 1;
        GoToLine(nextId);
    }

    /// <summary>
    /// 選択肢を選んだときに呼ばれます。
    /// </summary>
    /// <param name="choiceIndex">0:選択肢1、1:選択肢2</param>
    public void SelectChoice(int choiceIndex)
    {
        if (currentLine == null) return;

        if (choiceIndex == 0)
        {
            AddAffinity(currentLine.choice1Affinity);
            GoToLine(currentLine.choice1Next);
        }
        else
        {
            AddAffinity(currentLine.choice2Affinity);
            GoToLine(currentLine.choice2Next);
        }
    }

    // -------------------------------------------------------
    // 好感度管理
    // -------------------------------------------------------

    /// <summary>
    /// 現在の客の好感度を取得します。
    /// </summary>
    public int GetCurrentAffinity()
    {
        if (string.IsNullOrEmpty(currentCharacter)) return 0;
        return affinityDict.TryGetValue(currentCharacter, out var val) ? val : 0;
    }

    /// <summary>
    /// 現在の客の好感度を増減します。
    /// </summary>
    private void AddAffinity(int amount)
    {
        if (string.IsNullOrEmpty(currentCharacter)) return;

        if (!affinityDict.ContainsKey(currentCharacter))
            affinityDict[currentCharacter] = 0;

        affinityDict[currentCharacter] += amount;
        NovelUI.Instance?.UpdateAffinity(affinityDict[currentCharacter]);
    }

    // -------------------------------------------------------
    // BGM
    // -------------------------------------------------------

    /// <summary>
    /// BGMをSoundManager経由で再生します。
    /// </summary>
    private void PlayBGM(string bgmName)
    {
        var clip = Resources.Load<UnityEngine.AudioClip>($"BGM/{bgmName}");
        if (clip == null)
        {
            Debug.LogWarning($"[NovelManager] BGM '{bgmName}' が見つかりません。");
            return;
        }
        SoundManager.Instance?.PlayBGM(clip);
    }
}