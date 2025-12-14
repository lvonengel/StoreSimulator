using TMPro;
using UnityEngine;

/// <summary>
/// Thi controls the UI for buying stock in the game. This
/// creates each frame with the data for the item.
/// </summary>
public class BuyStockFrameController : MonoBehaviour {
    public StockInfo info;

    public TMP_Text nameText, priceText, amountInBoxText, boxPriceText, buttonText;

    public StockBoxController boxToSpawn;

    private float boxCost;

    private void Start() {
        UpdateFrameInfo();
    }

    /// <summary>
    /// Creates the information for each item.
    /// </summary>
    public void UpdateFrameInfo() {
        info = StockInfoController.instance.GetInfo(info.name);

        nameText.text = info.name;
        priceText.text = "$" + info.price.ToString("F2");

        int boxAmount = boxToSpawn.GetStockAmount(info.typeOfStock);
        amountInBoxText.text = boxAmount.ToString() + " per box";

        boxCost = boxAmount * info.price;
        boxPriceText.text = "Box: $" + boxCost.ToString("F2");

        buttonText.text = "PAY: $" + boxCost.ToString("F2");
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
}