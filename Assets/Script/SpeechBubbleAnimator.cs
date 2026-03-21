using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 複数の画像を一定間隔で切り替えてアニメーションさせるクラス。
/// 吹き出しのもにょもにょ演出に使います。
/// </summary>
public class SpeechBubbleAnimator : MonoBehaviour
{
    // -------------------------------------------------------
    // インスペクター設定
    // -------------------------------------------------------

    [Header("アニメーションする画像コンポーネント")]
    [SerializeField] private Image bubbleImage;

    [Header("切り替える画像一覧（順番に表示されます）")]
    [SerializeField] private Sprite[] sprites;

    [Header("1秒間に何枚切り替えるか（4推奨）")]
    [SerializeField] private float fps = 4f;

    // -------------------------------------------------------
    // 内部状態
    // -------------------------------------------------------

    /// <summary>現在表示中の画像インデックス</summary>
    private int currentIndex = 0;

    /// <summary>次の切り替えまでの残り時間</summary>
    private float timer = 0f;

    // -------------------------------------------------------
    // ライフサイクル
    // -------------------------------------------------------

    private void Update()
    {
        if (sprites == null || sprites.Length == 0) return;

        timer += Time.deltaTime;

        // 1/fps秒ごとに次の画像に切り替える
        if (timer >= 1f / fps)
        {
            timer = 0f;
            currentIndex = (currentIndex + 1) % sprites.Length;
            bubbleImage.sprite = sprites[currentIndex];
        }
    }
}