using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Thi controls the UI for buying stock in the game. This
/// creates each frame with the data for the item.
/// </summary>
public class BuyStockFrameTemplate : MonoBehaviour {
    private StockInfo info;

    [SerializeField] private TMP_Text nameText, priceText, amountInBoxText, boxPriceText, buttonText;
    [SerializeField] private GameObject underleveledScreen;
    [SerializeField] private TMP_Text underleveledText;

    [SerializeField] private Button buyButton;
    [SerializeField] private Image productImage;


    [SerializeField] private StockBoxController boxToSpawn;

    private float boxCost;

    private void Start() {
        buyButton.onClick.AddListener(() => {
            CartController.instance.AddToCart(info);
        });
    }

    public void OnEnable() {
        StoreController.instance.OnStoreLevelChanged += StoreController_OnStoreLevelChanged;
        if (info != null) {
            RefreshBuyState();
        }
    }
    public void OnDisable() {
        StoreController.instance.OnStoreLevelChanged -= StoreController_OnStoreLevelChanged;
    }

    private void StoreController_OnStoreLevelChanged(int newLevel) {
        if (info == null) {
            Debug.Log("info is null");
            return;
        }

        RefreshBuyState();
    }


    /// <summary>
    /// Creates the information for each item.
    /// </summary>
    public void UpdateFrameInfo(StockInfo food) {
        info = food;

        nameText.text = food.name;
        priceText.text = "$" + food.price.ToString("F2");

        int boxAmount = boxToSpawn.GetStockAmount(food.typeOfStock);
        amountInBoxText.text = boxAmount.ToString() + " per box";

        boxCost = boxAmount * food.price;
        boxPriceText.text = "Box: $" + boxCost.ToString("F2");

        RefreshBuyState();
    }

    private void RefreshBuyState() {
        if (CanBuy(info)) {
            buyButton.gameObject.SetActive(true);
            buttonText.text = "PAY: $" + boxCost.ToString("F2");
            underleveledScreen.SetActive(false);
        } else {
            buyButton.gameObject.SetActive(false);
            underleveledScreen.SetActive(true);
            underleveledText.text = "MUST BE LV " + info.requiredStoreLevel;
        }
    }

    public bool CanBuy(StockInfo food) {
        if (StoreController.instance.GetStoreLevel() >= food.requiredStoreLevel) {
            return true;
        }
        return false;
    }
}