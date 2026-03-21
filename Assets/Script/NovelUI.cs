using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ノベルパートのUIを管理するクラス。
/// テキスト・キャラクター画像・選択肢・好感度の表示を担当します。
/// </summary>
public class NovelUI : MonoBehaviour
{
    // -------------------------------------------------------
    // シングルトン
    // -------------------------------------------------------

    /// <summary>唯一のインスタンス（読み取り専用）</summary>
    public static NovelUI Instance { get; private set; }

    // -------------------------------------------------------
    // インスペクター設定
    // -------------------------------------------------------

    [Header("テキスト表示")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("キャラクター画像")]
    [SerializeField] private Image characterImage;

    [Header("選択肢")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button choice1Button;
    [SerializeField] private TextMeshProUGUI choice1Text;
    [SerializeField] private Button choice2Button;
    [SerializeField] private TextMeshProUGUI choice2Text;

    [Header("好感度ゲージ")]
    [SerializeField] private Image affinityGauge; // Fill画像
    [SerializeField] private int maxAffinity = 10;

    [Header("次へボタン")]
    [SerializeField] private Button nextButton;

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
        // ボタンのリスナー登録
        nextButton.onClick.AddListener(NovelManager.Instance.NextLine);
        choice1Button.onClick.AddListener(() => NovelManager.Instance.SelectChoice(0));
        choice2Button.onClick.AddListener(() => NovelManager.Instance.SelectChoice(1));

        choicePanel.SetActive(false);
    }

    // -------------------------------------------------------
    // 表示更新
    // -------------------------------------------------------

    /// <summary>
    /// シナリオの1行を受け取ってUIに反映します。
    /// </summary>
    /// <param name="line">表示するシナリオ行</param>
    /// <param name="affinity">現在の好感度</param>
    public void ShowLine(ScenarioLine line, int affinity)
    {
        // テキスト表示
        characterNameText.text = line.character;
        dialogueText.text = line.text;

        // キャラクター画像切り替え
        if (!string.IsNullOrEmpty(line.sprite))
        {
            var sprite = Resources.Load<Sprite>($"Sprites/{line.character}/{line.sprite}");
            if (sprite != null)
            {
                characterImage.sprite = sprite;
                characterImage.gameObject.SetActive(true); // 表示
            }
        }
        else
        {
            characterImage.gameObject.SetActive(false); // 非表示
        }

        // 好感度更新
        UpdateAffinity(affinity);

        // 選択肢の表示切り替え
        if (line.HasChoice)
        {
            choicePanel.SetActive(true);
            nextButton.gameObject.SetActive(false);
            choice1Text.text = line.choice1Text;
            choice2Text.text = line.choice2Text;
        }
        else
        {
            choicePanel.SetActive(false);
            nextButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 好感度ゲージを更新します。
    /// </summary>
    /// <param name="affinity">現在の好感度（0〜10）</param>
    public void UpdateAffinity(int affinity)
    {
        float fill = Mathf.Clamp01((float)affinity / maxAffinity);
        affinityGauge.fillAmount = fill;
    }
}