using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// Manages the logic behind saving all player and game data.
/// </summary>
public class SaveManager : MonoBehaviour {
    public static SaveManager instance {get; private set;}
    public enum SaveSlot {Slot1, Slot2, Slot3}

    public SaveSlot currentSlot;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            SaveGame();
            Debug.Log("SAVED");
        }

        if (Input.GetKeyDown(KeyCode.O)) {
            LoadGame(currentSlot);
            Debug.Log("LOADED");
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            DeleteAllSaves();
            Debug.Log("ALL SAVES CLEARED");
        }
    }
    private string GetSavePath(SaveSlot slot) {
        return Application.persistentDataPath + $"/save_{slot}.json";
    }

    public void SaveGame() {
        SaveData data = BuildSaveDataFromGame();
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);

        File.WriteAllText(GetSavePath(currentSlot), json);
    }

    public bool LoadGame(SaveSlot slot) {
        string path = GetSavePath(slot);
        if (!File.Exists(path)) {
            return false;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonConvert.DeserializeObject<SaveData>(json);

        ApplySaveDataToGame(data);
        currentSlot = slot;
        return true;
    }

    public void DeleteAllSaves() {
        foreach (SaveSlot slot in System.Enum.GetValues(typeof(SaveSlot))) {
            string path = GetSavePath(slot);

            if (File.Exists(path)) {
                File.Delete(path);
                Debug.Log($"Deleted save file: {path}");
            }
        }
    }

    private SaveData BuildSaveDataFromGame() {
        SaveData data = new SaveData();

        SaveMoneyAndLevel(data);
        SaveOwnedCards(data);
        data.purchasedStoreSpace = UpgradeStoreSpaceInfoController.instance.GetPurchasedStoreSpaces();
        data.purchasedAdvertisements = AdvertisementInfoController.instance.GetPurchasedAdvertisements();

        SaveFurniture(data);
        SaveShelfStock(data);

        return data;
    }

    private void SaveShelfStock(SaveData data) {
        data.shelfStock = new List<ShelfStockSaveData>();

        foreach (FurnitureController furniture in StoreController.instance.shelvingCases) {
            if (furniture.IsPlaced()) {
                for (int i = 0; i < furniture.shelves.Count; i++) {
                    ShelfSpaceController shelf = furniture.shelves[i];

                    if (shelf.HasStock()) {
                        ShelfStockSaveData saveData = new ShelfStockSaveData();
                        saveData.furnitureInstanceId = furniture.instanceId;
                        saveData.shelfIndex = i;
                        saveData.stockName = shelf.GetStockName();
                        saveData.quantity = shelf.GetStockCount();

                        data.shelfStock.Add(saveData);
                    }
                }
            }
        }
    }

 
    private void SaveOwnedCards(SaveData data) {
        if (CardInventoryController.instance == null) {
            Debug.Log("card inventory is null");
            return;
        }

        data.ownedCards = new List<CardSaveData>();

        List<CardInventoryController.CardInventoryEntry> owned = CardInventoryController.instance.ownedCards;

        for (int i = 0; i < owned.Count; i++) {
            CardInventoryController.CardInventoryEntry entry = owned[i];

            CardSaveData saveData = new CardSaveData();
            saveData.cardName = entry.card.cardName;
            saveData.quantity = entry.quantity;

            data.ownedCards.Add(saveData);
        }
    }


    private void SaveMoneyAndLevel(SaveData data) {
        data.money = StoreController.instance.GetCurrentMoney();
        data.storeLevel = StoreController.instance.GetStoreLevel();
        data.currentExperience = StoreController.instance.GetCurrentExperience();
    }

    private void SaveFurniture(SaveData data) {
        data.placedFurniture = new List<FurnitureSaveData>();

        foreach (FurnitureController furniture in StoreController.instance.shelvingCases) {
            if (furniture.IsPlaced() && furniture.isPrimaryFurniture) {
                FurnitureSaveData saveData = new FurnitureSaveData();
                saveData.instanceId = furniture.instanceId;
                saveData.furnitureId = furniture.furnitureId;
                saveData.position = new SerializableVector3(furniture.transform.position);
                saveData.rotation = new SerializableQuaternion(furniture.transform.rotation);

                data.placedFurniture.Add(saveData);
            }
        }
    }

    private void ApplySaveDataToGame(SaveData data) {
        StoreController.instance.LoadMoneyAndLevel(
            data.money,
            data.storeLevel,
            data.currentExperience
        );

        LoadOwnedCards(data);
        AchievementInfoController.instance.RefreshAllFrames();

        UpgradeStoreSpaceInfoController.instance.LoadPurchasedStoreSpaces(data.purchasedStoreSpace);
        AdvertisementInfoController.instance.LoadPurchasedAdvertisements(data.purchasedAdvertisements);

        ClearFurniture();
        LoadFurniture(data);

        //must wait for all furniture to load in
        StartCoroutine(LoadShelfStockDelayed(data));
    }

    private IEnumerator LoadShelfStockDelayed(SaveData data) {
        yield return null;

        LoadShelfStock(data);
    }

    private void LoadOwnedCards(SaveData data) {

        CardInventoryController.instance.ownedCards.Clear();

        if (data.ownedCards == null) {
            return;
        }

        List<StockInfo> cardPacks = StockInfoController.instance.GetCardPackInfo();

        for (int i = 0; i < data.ownedCards.Count; i++) {
            CardSaveData saveData = data.ownedCards[i];

            CardInfo foundCard = null;

            for (int p = 0; p < cardPacks.Count; p++) {
                StockInfo pack = cardPacks[p];

                for (int c = 0; c < pack.cardPack.possibleCardsList.Count; c++) {
                    CardInfo card = pack.cardPack.possibleCardsList[c];

                    if (card.cardName == saveData.cardName) {
                        foundCard = card;
                    }
                }
            }

            if (foundCard != null) {
                CardInventoryController.instance.AddCard(foundCard, saveData.quantity);
            }
        }
    }

    private void LoadShelfStock(SaveData data) {
        Dictionary<string, FurnitureController> furnitureLookup = new Dictionary<string, FurnitureController>();

        foreach (FurnitureController furniture in StoreController.instance.shelvingCases) {
            furnitureLookup[furniture.instanceId] = furniture;
        }

        foreach (ShelfStockSaveData stockData in data.shelfStock) {
            if (furnitureLookup.TryGetValue(stockData.furnitureInstanceId, out FurnitureController furniture)) {
                if (stockData.shelfIndex >= 0 && stockData.shelfIndex < furniture.shelves.Count) {

                    ShelfSpaceController shelf = furniture.shelves[stockData.shelfIndex];
                    StockInfo stockInfo = StockInfoController.instance.GetInfo(stockData.stockName);

                    if (stockInfo != null) {
                        shelf.LoadStock(stockInfo, stockData.quantity);
                    }
                }
            }
        }
    }



    private void LoadFurniture(SaveData data) {
        if (FurnitureInfoController.instance == null) {
            Debug.LogError("FurnitureInfoController instance is null");
            return;
        }

        foreach (FurnitureSaveData furnitureData in data.placedFurniture) {
            FurnitureController prefab =
                FurnitureInfoController.instance.GetFurniturePrefab(
                    furnitureData.furnitureId
                );

            if (prefab == null) {
                continue;
            }

            FurnitureController furniture =
                Instantiate(
                    prefab,
                    furnitureData.position.ToVector3(),
                    furnitureData.rotation.ToQuaternion()
                );

            furniture.instanceId = furnitureData.instanceId;
            furniture.PlaceFurniture();
        }
    }

    private void ClearFurniture() {
        foreach (FurnitureController furniture in StoreController.instance.shelvingCases) {
            Destroy(furniture.gameObject);
        }

        StoreController.instance.shelvingCases.Clear();
    }


}

