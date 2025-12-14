using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages each individual piece of furniture.
/// </summary>
public class FurnitureController : MonoBehaviour {
    public GameObject mainObject, placingObject;
    public Collider col;

    public float price;

    public Transform standPoint;

    public List<ShelfSpaceController> shelves;

    private void Start() {
        if (shelves.Count > 0) {
            StoreController.instance.shelvingCases.Add(this);
        }
    }

    /// <summary>
    /// Turns the placing object (highlighted in green when moving) on
    /// </summary>
    public void MakePlaceable() {
        mainObject.SetActive(false);
        placingObject.SetActive(true);
        col.enabled = false;
    }

    /// <summary>
    /// Places the furniture by erasing the duplicated placing object,
    /// and actually placing the active furniture down.
    /// </summary>
    public void PlaceFurniture() {
        mainObject.SetActive(true);
        placingObject.SetActive(false);
        col.enabled = true;
    }
}