using System;

/// <summary>
/// Class that specifically is used to save
/// the player's shelf stock on a specific furniture.
/// </summary>
[Serializable]
public class ShelfStockSaveData {
    public string furnitureInstanceId;
    public int shelfIndex;
    public string stockName;
    public int quantity;

}
