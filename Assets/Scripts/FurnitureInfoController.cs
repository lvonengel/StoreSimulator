using System.Collections.Generic;
using UnityEngine;

public class FurnitureInfoController : MonoBehaviour {
    
    public static FurnitureInfoController instance;
    public List<FurnitureInfo> furnitureInfo;

    private void Awake() {
        instance = this;
    }

}