using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シナリオの進行・好感度・カクテル提供を管理するクラス。
/// NovelUIと連携してゲームを進行させます。
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

    [Header("最初に会話する客の名前")]
    [SerializeField] private string startCharacterName;

    [Header("最初のシナリオID")]
    [SerializeField] private int startId = 1;

    // -------------------------------------------------------
    // 内部状態
    // -------------------------------------------------------

    /// <summary>現在進行中のシナリオデータ</summary>
    private ScenarioData currentScenario;

    /// <summary>現在表示中の行</summary>
    private ScenarioLine currentLine;

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
        // 最初の客のシナリオを読み込んで開始
        LoadCharacter(startCharacterName);
        GoToLine(startId);
    }

    // -------------------------------------------------------
    // シナリオ読み込み
    // -------------------------------------------------------

    /// <summary>
    /// 指定した客のシナリオCSVを読み込みます。
    /// </summary>
    /// <param name="characterName">客の名前（CSVファイル名）</param>
    public void LoadCharacter(string characterName)
    {
        currentScenario = ScenarioLoader.Load(characterName);

        if (currentScenario == null)
        {
            Debug.LogWarning($"[NovelManager] {characterName} のシナリオが読み込めませんでした。");
            return;
        }

        // 好感度の初期化（初めての客なら0でスタート）
        if (!affinityDict.ContainsKey(characterName))
            affinityDict[characterName] = 0;

        Debug.Log($"[NovelManager] {characterName} のシナリオを読み込みました。");
    }

    // -------------------------------------------------------
    // シナリオ進行
    // -------------------------------------------------------

    /// <summary>
    /// 指定IDの行に移動して表示します。
    /// </summary>
    /// <param name="id">移動先のシナリオID</param>
    public void GoToLine(int id)
    {
        if (currentScenario == null) return;

        if (!currentScenario.lines.TryGetValue(id, out currentLine))
        {
            Debug.LogWarning($"[NovelManager] ID:{id} の行が見つかりません。");
            return;
        }

        // BGM切り替え
        if (!string.IsNullOrEmpty(currentLine.bgm))
            PlayBGM(currentLine.bgm);

        // SE再生
        if (!string.IsNullOrEmpty(currentLine.se))
            SoundManager.Instance?.PlaySE(currentLine.se);

        // カクテル提供
        if (currentLine.HasCocktail)
            CocktailManager.Instance?.ServeCocktail(currentLine.cocktail);

        // UIに表示を依頼
        NovelUI.Instance?.ShowLine(currentLine, GetCurrentAffinity());
    }

    /// <summary>
    /// 次の行に進みます（選択肢がない場合に呼ばれます）。
    /// </summary>
    public void NextLine()
    {
        if (currentLine == null) return;
        if (currentLine.HasChoice) return; // 選択肢があるときは進まない

        GoToLine(currentLine.id + 1);
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
        if (currentScenario == null) return 0;
        return affinityDict.TryGetValue(currentScenario.characterName, out var val) ? val : 0;
    }

    /// <summary>
    /// 現在の客の好感度を増減します。
    /// </summary>
    /// <param name="amount">増減量（マイナスで減少）</param>
    private void AddAffinity(int amount)
    {
        if (currentScenario == null) return;

        var name = currentScenario.characterName;
        if (!affinityDict.ContainsKey(name))
            affinityDict[name] = 0;

        affinityDict[name] += amount;
        Debug.Log($"[NovelManager] {name} 好感度: {affinityDict[name]}");

        // UIの好感度表示を更新
        NovelUI.Instance?.UpdateAffinity(affinityDict[name]);
    }

    // -------------------------------------------------------
    // BGM
    // -------------------------------------------------------

    /// <summary>
    /// BGMをSoundManager経由で再生します。
    /// </summary>
    private void PlayBGM(string bgmName)
    {
        // BGMのAudioClipはResourcesから読み込む
        var clip = Resources.Load<UnityEngine.AudioClip>($"BGM/{bgmName}");
        if (clip == null)
        {
            Debug.LogWarning($"[NovelManager] BGM '{bgmName}' が見つかりません。Resources/BGM/に配置してください。");
            return;
        }
        SoundManager.Instance?.PlayBGM(clip);
    }
}