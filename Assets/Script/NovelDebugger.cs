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

    // -------------------------------------------------------
    // ライフサイクル
    // -------------------------------------------------------

    private void Start()
    {
        if (!enableDebug) return;

        // NovelManagerの通常のStart処理を上書きして指定IDから開始
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