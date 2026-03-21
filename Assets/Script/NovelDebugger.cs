using UnityEngine;
using System.Collections;


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

    // -------------------------------------------------------
    // ライフサイクル
    // -------------------------------------------------------

    private IEnumerator Start()
    {
        if (!enableDebug) yield break;

        NovelManager.Instance.skipAutoStart = true;

        // NovelManagerとNovelUIの初期化を待つ
        yield return null;
        yield return null;

        Debug.Log($"[NovelDebugger] ID:{debugStartId} からジャンプ");
        NovelManager.Instance?.GoToLine(debugStartId);
    }

#if UNITY_EDITOR
    /// <summary>
    /// エディター上でGキーを押したら指定IDにジャンプします。
    /// </summary>
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