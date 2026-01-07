using System;
using System.Collections.Generic;

/// <summary>
/// Manages all saved player data.
/// </summary>
[Serializable]
public class SaveData {

    public int saveVersion = 1;
    public string lastPlayed;

    // store stats
    public int currentDay;
    public float money;
    public int storeLevel;
    public int currentExperience;


    // cards/decks
    public List<CardSaveData> ownedCards;

    public List<DeckSaveData> customDecks;

    // inside store
    public List<FurnitureSaveData> placedFurniture;
    public List<ShelfStockSaveData> shelfStock;
    public List<string> purchasedStoreSpace;
    public List<string> purchasedAdvertisements;

}
