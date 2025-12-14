using TMPro;
using UnityEngine;

/// <summary>
/// Controls buying the furniture in the buy panel.
/// </summary>
public class BuyFurnitureFrameController : MonoBehaviour {
    public FurnitureInfo furniture;

    public TMP_Text nameText;
    public TMP_Text priceText;

    public void UpdateFrameInfo(FurnitureInfo furniture) {
        this.furniture = furniture;
        // info = StockInfoController.instance.GetInfo(info.name);
        nameText.text = furniture.name;
        priceText.text = "Price: $" + furniture.price.ToString("F2");
    }


    /// <summary>
    /// Checks whether you have the money. If you do, spawns the furniture
    /// at the spawn point.
    /// </summary>
    public void BuyFurniture() {
        if (StoreController.instance.CheckMoneyAvailable(furniture.price)) {
            StoreController.instance.SpendMoney(furniture.price);
            Instantiate(furniture.furnitureObject, StoreController.instance.furnitureSpawnPoint.position, Quaternion.identity);
        }
    }
}