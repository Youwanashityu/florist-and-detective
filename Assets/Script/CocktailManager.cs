using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// カクテルの提供演出とシェイカーミニゲームを管理するクラス。
/// </summary>
public class CocktailManager : MonoBehaviour
{
    // -------------------------------------------------------
    // シングルトン
    // -------------------------------------------------------

    /// <summary>唯一のインスタンス（読み取り専用）</summary>
    public static CocktailManager Instance { get; private set; }

    // -------------------------------------------------------
    // インスペクター設定
    // -------------------------------------------------------

    [Header("カクテル完成パネル")]
    [SerializeField] private GameObject cocktailPanel;

    //[Header("カクテル名テキスト")]
    //[SerializeField] private TextMeshProUGUI cocktailNameText;

    [Header("カクテル画像")]
    [SerializeField] private Image cocktailImage;

    [Header("閉じるボタン")]
    [SerializeField] private Button closeButton;

    // -------------------------------------------------------
    // 内部状態
    // -------------------------------------------------------

    /// <summary>シェイク結果に応じた次のIDを保持</summary>
    private int shortNextId;
    private int justNextId;
    private int longNextId;

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

        cocktailPanel.SetActive(false);
        closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    // -------------------------------------------------------
    // カクテル提供
    // -------------------------------------------------------

    /// <summary>
    /// シェイカーミニゲームを開始します。
    /// NovelManagerから呼ばれます。
    /// </summary>
    public void ServeCocktail(string cocktailName, float minTime, float maxTime, int shortNext, int justNext, int longNext)
    {
        shortNextId = shortNext;
        justNextId = justNext;
        longNextId = longNext;

        ShakerMinigame.Instance?.StartShaker(minTime, maxTime, OnShakeComplete);
    }

    /// <summary>
    /// シェイク完了時に呼ばれます。結果に応じてカクテル完成パネルを表示します。
    /// </summary>
    private void OnShakeComplete(ShakerMinigame.ShakeResult result)
    {
        // 結果に応じた次のIDを選択
        int nextId = result switch
        {
            ShakerMinigame.ShakeResult.Short => shortNextId,
            ShakerMinigame.ShakeResult.Just => justNextId,
            ShakerMinigame.ShakeResult.Long => longNextId,
            _ => justNextId
        };

        // カクテル完成パネルを表示してからシナリオを進める
        cocktailPanel.SetActive(true);
        NovelManager.Instance?.GoToLine(nextId);
    }

    /// <summary>
    /// 閉じるボタンが押されたときにパネルを非表示にします。
    /// </summary>
    private void OnCloseButtonClicked()
    {
        cocktailPanel.SetActive(false);
    }
}
//

//---

//## Resourcesフォルダの構成
//```
//Assets / Resources /
//├── BGM /          ← BGMのAudioClipを置く
//├── Sprites/
//│   ├── 客A/      ← 客AのSprite（happy.png, normal.png など）
//│   └── 客B/
//└── Cocktails/    ← カクテルの画像を置く

//Assets/StreamingAssets/
//└── Scenarios/
//    ├── 客A.csv
//   └── 客B.csv