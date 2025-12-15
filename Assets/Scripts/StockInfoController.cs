using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the price label UI for a shelf space.
/// </summary>
public class StockInfoController : MonoBehaviour {
    public static StockInfoController instance;
    [SerializeField] private List<StockInfo> foodInfo, produceInfo;

    public List<StockInfo> allStock = new List<StockInfo>();

    

    private void Awake() {
        instance = this;

        allStock.AddRange(foodInfo);
        allStock.AddRange(produceInfo);

        for (int i = 0; i < allStock.Count; i++) {
            if (allStock[i].currentPrice == 0) {
                allStock[i].currentPrice = allStock[i].price;
            }
        }
    }

    public StockInfo GetInfo(string stockName) {
        StockInfo infoToReturn = null;

        for (int i = 0; i < allStock.Count; i++) {
            if (allStock[i].name == stockName) {
                infoToReturn = allStock[i];
            }
        }

        return infoToReturn;
    }

    public void UpdatePrice(string stockName, float newPrice) {
        for (int i = 0; i < allStock.Count; i++) {
            if (allStock[i].name == stockName) {
                allStock[i].currentPrice = newPrice;
            }
        }

        List<ShelfSpaceController> shelves = new List<ShelfSpaceController>();

        shelves.AddRange(FindObjectsByType<ShelfSpaceController>(FindObjectsSortMode.None));

        foreach (ShelfSpaceController shelf in shelves) {
            if (shelf.info.name == stockName) {
                shelf.UpdateDisplayPrice(newPrice);
            }
        }
    }
}