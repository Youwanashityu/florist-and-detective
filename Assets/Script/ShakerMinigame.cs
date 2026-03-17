using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// シェイカーミニゲームを管理するクラス。
/// クリック中にタイマーが進み、提供ボタンで結果を判定します。
/// </summary>
public class ShakerMinigame : MonoBehaviour
{
    // -------------------------------------------------------
    // シングルトン
    // -------------------------------------------------------

    /// <summary>唯一のインスタンス（読み取り専用）</summary>
    public static ShakerMinigame Instance { get; private set; }

    // -------------------------------------------------------
    // インスペクター設定
    // -------------------------------------------------------

    [Header("シェイカーパネル")]
    [SerializeField] private GameObject shakerPanel;

    [Header("シェイカー画像（クリック中に揺れる）")]
    [SerializeField] private RectTransform shakerImage;

    [Header("タイマーゲージ")]
    [SerializeField] private Image timerGauge;

    [Header("経過時間テキスト")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("提供ボタン")]
    [SerializeField] private Button serveButton;

    // -------------------------------------------------------
    // 内部状態
    // -------------------------------------------------------

    /// <summary>ちょうどの最小シェイク時間（秒）</summary>
    private float minTime = 2.5f;

    /// <summary>ちょうどの最大シェイク時間（秒）</summary>
    private float maxTime = 3.5f;

    /// <summary>計測中の経過時間（秒）</summary>
    private float elapsedTime = 0f;

    /// <summary>現在クリック中かどうか</summary>
    private bool isShaking = false;

    /// <summary>ミニゲームが進行中かどうか</summary>
    private bool isPlaying = false;

    /// <summary>シェイク結果を返すコールバック</summary>
    private System.Action<ShakeResult> onComplete;

    /// <summary>シェイカーの初期位置</summary>
    private Vector3 originalPosition;

    // -------------------------------------------------------
    // シェイク結果の種類
    // -------------------------------------------------------

    public enum ShakeResult
    {
        /// <summary>少ない</summary>
        Short,
        /// <summary>ちょうど</summary>
        Just,
        /// <summary>多い</summary>
        Long
    }

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

        shakerPanel.SetActive(false);
        serveButton.onClick.AddListener(OnServeButtonClicked);

        if (shakerImage != null)
            originalPosition = shakerImage.localPosition;
    }

    private void Update()
    {
        if (!isPlaying) return;

        // クリック中だけタイマーを進める
        isShaking = Input.GetMouseButton(0);

        if (isShaking)
        {
            elapsedTime += Time.deltaTime;
            ShakeAnimation();
        }
        else
        {
            // クリックしていないときは元の位置に戻す
            if (shakerImage != null)
                shakerImage.localPosition = originalPosition;
        }

        // ゲージ・テキスト更新（最大時間の2倍を上限とする）
        float maxDisplay = maxTime * 2f;
        if (timerGauge != null)
            timerGauge.fillAmount = Mathf.Clamp01(elapsedTime / maxDisplay);
        if (timerText != null)
            timerText.text = $"{elapsedTime:F1}s";
    }

    // -------------------------------------------------------
    // 公開メソッド
    // -------------------------------------------------------

    /// <summary>
    /// シェイカーミニゲームを開始します。
    /// CocktailManagerから呼ばれます。
    /// </summary>
    /// <param name="minTime">ちょうどの最小シェイク時間（秒）</param>
    /// <param name="maxTime">ちょうどの最大シェイク時間（秒）</param>
    /// <param name="onComplete">結果を受け取るコールバック</param>
    public void StartShaker(float minTime, float maxTime, System.Action<ShakeResult> onComplete)
    {
        this.minTime = minTime;
        this.maxTime = maxTime;
        this.onComplete = onComplete;

        elapsedTime = 0f;
        isPlaying = true;

        shakerPanel.SetActive(true);

        if (timerGauge != null) timerGauge.fillAmount = 0f;
        if (timerText != null) timerText.text = "0.0s";
    }

    // -------------------------------------------------------
    // 内部処理
    // -------------------------------------------------------

    /// <summary>
    /// 提供ボタンが押されたときに結果を判定します。
    /// </summary>
    private void OnServeButtonClicked()
    {
        if (!isPlaying) return;

        isPlaying = false;
        shakerPanel.SetActive(false);

        if (shakerImage != null)
            shakerImage.localPosition = originalPosition;

        // 結果判定
        ShakeResult result;
        if (elapsedTime < minTime)
            result = ShakeResult.Short;
        else if (elapsedTime <= maxTime)
            result = ShakeResult.Just;
        else
            result = ShakeResult.Long;

        Debug.Log($"[ShakerMinigame] 経過時間:{elapsedTime:F1}s 範囲:{minTime}〜{maxTime}s 結果:{result}");
        onComplete?.Invoke(result);
    }

    /// <summary>
    /// シェイカーを揺らすアニメーション処理。
    /// </summary>
    private void ShakeAnimation()
    {
        if (shakerImage == null) return;

        float shakeAmount = 10f;
        shakerImage.localPosition = originalPosition + new Vector3(
            Random.Range(-shakeAmount, shakeAmount),
            Random.Range(-shakeAmount, shakeAmount),
            0f
        );
    }
}