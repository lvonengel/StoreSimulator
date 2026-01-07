using System.Collections.Generic;
using UnityEngine;

public class FurnitureInfoController : MonoBehaviour {
    
    public static FurnitureInfoController instance {get; private set;}
    public List<FurnitureInfo> furnitureInfo;
    private Dictionary<string, FurnitureController> prefabLookup;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        prefabLookup = new Dictionary<string, FurnitureController>();

        for (int i = 0; i < furnitureInfo.Count; i++) {
            FurnitureController controller =
                furnitureInfo[i].furnitureObject.GetComponent<FurnitureController>();

            prefabLookup[furnitureInfo[i].furnitureId] = controller;
        }
    }

    public FurnitureController GetFurniturePrefab(string furnitureId) {
        if (prefabLookup.TryGetValue(furnitureId, out FurnitureController prefab)) {
            return prefab;
        }

        Debug.LogError($"Furniture prefab not found for id: {furnitureId}");
        return null;
    }


}