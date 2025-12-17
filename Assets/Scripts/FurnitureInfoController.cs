using System.Collections.Generic;
using UnityEngine;

public class FurnitureInfoController : MonoBehaviour {
    
    public static FurnitureInfoController instance {get; private set;}
    public List<FurnitureInfo> furnitureInfo;

    private void Awake() {
        instance = this;
    }

}