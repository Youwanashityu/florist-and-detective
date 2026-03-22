using System.Collections;
using UnityEngine;

/// <summary>
/// デバッグ用のスクリプト。
/// インスペクターから任意のIDを入力してシナリオを再生できます。
/// </summary>
public class NovelDebugger : MonoBehaviour
{
    // -------------------------------------------------------
    // インスペクター設定
    // -------------------------------------------------------

    [Header("ジャンプしたいシナリオID")]
    [SerializeField] private int debugStartId = 1;

    [Header("デバッグモードを有効にするか")]
    [SerializeField] private bool enableDebug = true;

    [Header("再生するBGM名（空欄なら再生しない）")]
    [SerializeField] private string debugBGMName = "";

    // -------------------------------------------------------
    // ライフサイクル
    // -------------------------------------------------------

    private IEnumerator Start()
    {
        if (!enableDebug) yield break;

        NovelManager.Instance.skipAutoStart = true;

        yield return null;
        yield return null;

        // BGM再生
        if (!string.IsNullOrEmpty(debugBGMName))
        {
            var clip = Resources.Load<AudioClip>($"BGM/{debugBGMName}");
            if (clip != null)
                SoundManager.Instance?.PlayBGM(clip);
            else
                Debug.LogWarning($"[NovelDebugger] BGM '{debugBGMName}' が見つかりません。");
        }

        Debug.Log($"[NovelDebugger] ID:{debugStartId} からジャンプ");
        NovelManager.Instance?.GoToLine(debugStartId);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log($"[NovelDebugger] ID:{debugStartId} にジャンプ");
            NovelManager.Instance?.GoToLine(debugStartId);
        }
    }
#endif
}