using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Thi controls the UI for buying stock in the game. This
/// creates each frame with the data for the item.
/// </summary>
public class BuyStockFrameController : MonoBehaviour {
    [SerializeField] private StockInfo info;

    [SerializeField] private TMP_Text nameText, priceText, amountInBoxText, boxPriceText, buttonText;
    [SerializeField] private GameObject underleveledScreen;
    [SerializeField] private TMP_Text underleveledText;

    [SerializeField] private Button buyButton;


    [SerializeField] private StockBoxController boxToSpawn;

    private float boxCost;

    /// <summary>
    /// Creates the information for each item.
    /// </summary>
    public void UpdateFrameInfo(StockInfo food) {
        info = food;
        // info = StockInfoController.instance.GetInfo(info.name);

        nameText.text = food.name;
        priceText.text = "$" + food.price.ToString("F2");

        int boxAmount = boxToSpawn.GetStockAmount(food.typeOfStock);
        amountInBoxText.text = boxAmount.ToString() + " per box";

        boxCost = boxAmount * food.price;
        boxPriceText.text = "Box: $" + boxCost.ToString("F2");

        if (CanBuy(food)) {
            buyButton.gameObject.SetActive(true);
            buttonText.text = "PAY: $" + boxCost.ToString("F2");
            underleveledScreen.SetActive(false);
        } else {
            buyButton.gameObject.SetActive(false);
            underleveledScreen.SetActive(true);
            underleveledText.text = "MUST BE LV " + food.requiredStoreLevel;
        }
    }

    /// <summary>
    /// Checks whether the player has enough money, and if so, spawns it in.
    /// </summary>
    public void BuyBox() {
        if (StoreController.instance.CheckMoneyAvailable(boxCost) == true) {
            StoreController.instance.SpendMoney(boxCost);
            Instantiate(boxToSpawn, StoreController.instance.stockSpawnPoint.position, Quaternion.identity).SetupBox(info);
        }
    }

    public bool CanBuy(StockInfo food) {
        if (food.requiredStoreLevel < StoreController.instance.GetStoreLevel()) {
            return true;
        }
        return false;
    }
}