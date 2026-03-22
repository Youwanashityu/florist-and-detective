using System.Collections;
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
    [SerializeField] private Image affinityGauge;
    [SerializeField] private int maxAffinity = 5;

    [Header("次へボタン")]
    [SerializeField] private Button nextButton;

    [Header("タイプライター効果")]
    [SerializeField] private float typingSpeed = 0.05f;

    // -------------------------------------------------------
    // 内部状態
    // -------------------------------------------------------

    /// <summary>タイプライター中かどうか</summary>
    private bool isTyping = false;

    /// <summary>現在表示中の全文テキスト</summary>
    private string fullText = "";

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

        // 初期テキストをクリア
        characterNameText.text = "";
        dialogueText.text = "";
    }
    private void Start()
    {
        nextButton.onClick.AddListener(OnNextButtonClicked);
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
    public void ShowLine(ScenarioLine line, int affinity)
    {
        characterNameText.text = line.character;

        // キャラクター画像切り替え
        if (!string.IsNullOrEmpty(line.sprite))
        {
            Debug.Log($"[NovelUI] スプライト読み込み: Sprites/{line.character}/{line.sprite}");
            var sprite = Resources.Load<Sprite>($"Sprites/{line.character}/{line.sprite}");
            if (sprite != null)
            {
                characterImage.sprite = sprite;
                characterImage.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"[NovelUI] スプライトが見つかりません: Sprites/{line.character}/{line.sprite}");
            }
        }

        // 好感度更新
        UpdateAffinity(affinity);

        // 選択肢の表示切り替え（タイプライター終了後に表示）
        choicePanel.SetActive(false);
        nextButton.gameObject.SetActive(false);

        // タイプライター開始
        StopAllCoroutines();
        fullText = line.text;
        StartCoroutine(TypeText(line));
    }

    /// <summary>
    /// 好感度ゲージを更新します。
    /// </summary>
    public void UpdateAffinity(int affinity)
    {
        float fill = Mathf.Clamp01((float)affinity / maxAffinity);
        affinityGauge.fillAmount = fill;
    }

    // -------------------------------------------------------
    // タイプライター
    // -------------------------------------------------------

    /// <summary>
    /// テキストを1文字ずつ表示するコルーチン。
    /// </summary>
    private IEnumerator TypeText(ScenarioLine line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (var c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        // カクテル提供がある場合はタイプライター終了後に開始
        if (line.HasCocktail)
        {
            CocktailManager.Instance?.ServeCocktail(
                line.cocktail,
                line.cocktailMinTime,
                line.cocktailMaxTime,
                line.cocktailShortNext,
                line.cocktailJustNext,
                line.cocktailLongNext
            );
            yield break;
        }

        // タイプライター終了後に選択肢or次へボタンを表示
        if (line.HasChoice)
        {
            choicePanel.SetActive(true);
            choice1Text.text = line.choice1Text;
            choice2Text.text = line.choice2Text;
        }
        else
        {
            nextButton.gameObject.SetActive(true);
        }
    }

    // -------------------------------------------------------
    // ボタン処理
    // -------------------------------------------------------

    /// <summary>
    /// 次へボタンが押されたときの処理。
    /// タイプライター中なら全文表示、終わっていれば次の行へ。
    /// </summary>
    private void OnNextButtonClicked()
    {
        Debug.Log($"[NovelUI] NextButton押された isTyping:{isTyping}");

        if (isTyping)
        {
            StopAllCoroutines();
            isTyping = false;
            dialogueText.text = fullText;
            nextButton.gameObject.SetActive(true);
            return;
        }
        NovelManager.Instance.NextLine();
    }
}