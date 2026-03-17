using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// カクテルの提供演出を管理するクラス。
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

    [Header("カクテル表示パネル")]
    [SerializeField] private GameObject cocktailPanel;

    [Header("カクテル名テキスト")]
    [SerializeField] private TextMeshProUGUI cocktailNameText;

    [Header("カクテル画像")]
    [SerializeField] private Image cocktailImage;

    [Header("閉じるボタン")]
    [SerializeField] private Button closeButton;

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
        closeButton.onClick.AddListener(() => cocktailPanel.SetActive(false));
    }

    // -------------------------------------------------------
    // カクテル提供
    // -------------------------------------------------------

    /// <summary>
    /// カクテルを提供する演出を表示します。
    /// 画像はResources/Cocktails/に配置してください。
    /// </summary>
    /// <param name="cocktailName">カクテル名</param>
    public void ServeCocktail(string cocktailName)
    {
        cocktailNameText.text = cocktailName;

        var sprite = Resources.Load<Sprite>($"Cocktails/{cocktailName}");
        if (sprite != null)
            cocktailImage.sprite = sprite;
        else
            Debug.LogWarning($"[CocktailManager] カクテル画像が見つかりません: Cocktails/{cocktailName}");

        cocktailPanel.SetActive(true);
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