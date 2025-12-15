using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages updating the price of stock items.
/// </summary>
public class UpdatePricePanelUI : MonoBehaviour {
    public static UpdatePricePanelUI instance {get; private set;}
    
    [SerializeField] private TMP_Text basePriceText, currentPriceText;

    [SerializeField] private TMP_InputField priceInputfield;

    [SerializeField] private StockInfo activeStockInfo;
    [SerializeField] private Button applyChangesButton;
    [SerializeField] private Button closeButton;

    private void Awake() {
        instance = this;
        applyChangesButton.onClick.AddListener(() => {
            ApplyPriceUpdate();
        });
        closeButton.onClick.AddListener(() => {
            CloseUpdatePrice();
        });
    }

    /// <summary>
    /// Updates the panel with the information for the stock item.
    /// </summary>
    public void LoadUpdatePrice(StockInfo stockToUpdate) {
        Cursor.lockState = CursorLockMode.None;

        basePriceText.text = "$" + stockToUpdate.price.ToString("F2");
        currentPriceText.text = "$" + stockToUpdate.currentPrice.ToString("F2");

        activeStockInfo = stockToUpdate;

        priceInputfield.text = stockToUpdate.currentPrice.ToString();
    }


    public void CloseUpdatePrice() {
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);
    }


    public void ApplyPriceUpdate() {
        activeStockInfo.currentPrice = float.Parse(priceInputfield.text);
        currentPriceText.text = "$" + activeStockInfo.currentPrice.ToString("F2");

        StockInfoController.instance.UpdatePrice(activeStockInfo.name, activeStockInfo.currentPrice);

        CloseUpdatePrice();
    }

}