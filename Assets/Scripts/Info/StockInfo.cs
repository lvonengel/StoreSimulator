using UnityEngine;

/// <summary>
/// The info for each stock item.
/// </summary>
[System.Serializable]
public class StockInfo
{
    public string name;
    public int requiredStoreLevel;

    public enum StockType {
        cereal, bigDrink, chipsTube, fruit, fruitLarge
    }
    public StockType typeOfStock;

    public float price, currentPrice;

    public StockObject stockObject;
    public CardPack cardPack;
}