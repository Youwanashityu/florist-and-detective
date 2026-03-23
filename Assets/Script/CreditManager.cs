using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// クレジットパネルの表示・非表示を管理するクラス。
/// </summary>
public class CreditManager : MonoBehaviour
{
    // -------------------------------------------------------
    // インスペクター設定
    // -------------------------------------------------------

    [Header("クレジットパネル")]
    [SerializeField] private GameObject creditPanel;

    [Header("クレジットボタン")]
    [SerializeField] private Button creditButton;

    [Header("閉じるボタン")]
    [SerializeField] private Button closeButton;

    // -------------------------------------------------------
    // ライフサイクル
    // -------------------------------------------------------

    private void Start()
    {
        creditPanel.SetActive(false);
        creditButton.onClick.AddListener(OpenCredit);
        closeButton.onClick.AddListener(CloseCredit);
    }

    // -------------------------------------------------------
    // ボタン処理
    // -------------------------------------------------------

    /// <summary>
    /// クレジットパネルを開きます。
    /// </summary>
    private void OpenCredit() => creditPanel.SetActive(true);

    /// <summary>
    /// クレジットパネルを閉じます。
    /// </summary>
    private void CloseCredit() => creditPanel.SetActive(false);
}