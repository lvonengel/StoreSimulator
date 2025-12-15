using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The info for each stock item.
/// </summary>
[System.Serializable]
public class FurnitureInfo {
    public string name;

    public int requiredStoreLevel;

    public enum StockType {
        cereal, bigDrink, chipsTube, fruit, fruitLarge
    }
    public List<StockType> typeOfStock;

    public float price;
    public GameObject furnitureObject;
}