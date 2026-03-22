using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// エンドシーンのボタンを管理するクラス。
/// タイトルに戻る・直前の選択肢に戻る・特定IDに戻るボタンを管理します。
/// </summary>
public class EndSceneManager : MonoBehaviour
{
    // -------------------------------------------------------
    // インスペクター設定
    // -------------------------------------------------------

    [Header("タイトルシーン名")]
    [SerializeField] private string titleSceneName = "Title";

    [Header("ノベルシーン名")]
    [SerializeField] private string novelSceneName = "Novel";

    [Header("タイトルに戻るボタン")]
    [SerializeField] private Button backToTitleButton;

    [Header("直前の選択肢に戻るボタン")]
    [SerializeField] private Button backToLastChoiceButton;

    [Header("特定IDに戻るボタン")]
    [SerializeField] private Button backToCheckpointButton;

    [Header("特定IDに戻るときのID")]
    [SerializeField] private int checkpointId = 1;

    // -------------------------------------------------------
    // ライフサイクル
    // -------------------------------------------------------

    private void Start()
    {
        backToTitleButton.onClick.AddListener(BackToTitle);
        backToLastChoiceButton.onClick.AddListener(BackToLastChoice);
        backToCheckpointButton.onClick.AddListener(BackToCheckpoint);
    }

    // -------------------------------------------------------
    // ボタン処理
    // -------------------------------------------------------

    /// <summary>
    /// タイトルシーンに戻ります。
    /// </summary>
    private void BackToTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }

    /// <summary>
    /// 直前の選択肢のIDに戻ります。
    /// </summary>
    private void BackToLastChoice()
    {
        int lastChoiceId = NovelProgressData.LastChoiceId;

        if (lastChoiceId <= 0)
        {
            Debug.LogWarning("[EndSceneManager] 直前の選択肢IDが記録されていません。");
            return;
        }

        NovelProgressData.JumpToId = lastChoiceId;
        SceneManager.LoadScene(novelSceneName);
    }

    /// <summary>
    /// 特定のIDに戻ります。
    /// </summary>
    private void BackToCheckpoint()
    {
        NovelProgressData.JumpToId = checkpointId;
        SceneManager.LoadScene(novelSceneName);
    }
}