using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトル画面のボタンを管理するクラス。
/// </summary>
public class TitleSceneManager : MonoBehaviour
{
    // -------------------------------------------------------
    // インスペクター設定
    // -------------------------------------------------------

    [Header("ノベルシーン名")]
    [SerializeField] private string novelSceneName = "Novel";

    [Header("スタートボタン（id1から始める）")]
    [SerializeField] private Button startButton;

    [Header("リトライボタン（直前の選択肢から始める）")]
    [SerializeField] private Button retryButton;

    [Header("指定IDから始めるボタン")]
    [SerializeField] private Button checkpointButton;

    [Header("指定IDから始めるときのID")]
    [SerializeField] private int checkpointId = 1;

    // -------------------------------------------------------
    // ライフサイクル
    // -------------------------------------------------------

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        retryButton.onClick.AddListener(RetryFromLastChoice);
        checkpointButton.onClick.AddListener(StartFromCheckpoint);

    }

    // -------------------------------------------------------
    // ボタン処理
    // -------------------------------------------------------

    /// <summary>
    /// id1からゲームを開始します。
    /// </summary>
    private void StartGame()
    {
        NovelProgressData.JumpToId = 0;
        SceneManager.LoadScene(novelSceneName);
    }

    /// <summary>
    /// 直前の選択肢IDからゲームを再開します。
    /// </summary>
    private void RetryFromLastChoice()
    {
        if (NovelProgressData.LastChoiceId <= 0) return;

        NovelProgressData.JumpToId = NovelProgressData.LastChoiceId;
        SceneManager.LoadScene(novelSceneName);
    }

    /// <summary>
    /// 指定したIDからゲームを開始します。
    /// </summary>
    private void StartFromCheckpoint()
    {
        NovelProgressData.JumpToId = checkpointId;
        SceneManager.LoadScene(novelSceneName);
    }
}