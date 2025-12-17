using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the buy menu UI where players buy stock or furniture.
/// This controls switching the two tabs.
/// </summary>
public class BuyStockController : MonoBehaviour {
    [SerializeField] private Transform stockTemplate;
    [SerializeField] private Transform stockTemplateContainer;
    [SerializeField] private GameObject stockPanel;
    [SerializeField] private Button cartButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject cartScreen;
    [SerializeField] private TMP_Text cartPriceLabel;
    

    private void Awake() {
        stockTemplate.gameObject.SetActive(false);
        cartButton.onClick.AddListener(() => {
            cartScreen.SetActive(true);
        });
        closeButton.onClick.AddListener(() => {
            gameObject.SetActive(false);
        });
        cartPriceLabel.text = "$0.00";
    }

    private void Start() {
        CreateStockTemplates();
        CartController.instance.OnCartChanged += CartController_OnCartChanged;
    }

    //Fired when the cart changes
    // mostly for UI
    private void CartController_OnCartChanged(object sender, System.EventArgs e) {
        cartPriceLabel.text = "$" + CartController.instance.GetCartSubtotal().ToString("F2");
    }

    /// <summary>
    /// Automatically creates the stock items in a grid by duplicating a template.
    /// </summary>
    private void CreateStockTemplates() {
        foreach (Transform child in stockTemplateContainer) {
            if (child == stockTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (StockInfo food in StockInfoController.instance.allStock) {
            Transform stockTransform = Instantiate(stockTemplate, stockTemplateContainer);
            stockTransform.gameObject.SetActive(true);
            stockTransform.GetComponent<BuyStockFrameController>().UpdateFrameInfo(food);
        }
    }

}