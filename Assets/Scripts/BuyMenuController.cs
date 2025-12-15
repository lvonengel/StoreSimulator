using UnityEngine;

/// <summary>
/// Controls the buy menu UI where players buy stock or furniture.
/// This controls switching the two tabs.
/// </summary>
public class BuyMenuController : MonoBehaviour {
    [SerializeField] private GameObject stockPanel;

    public void OpenStockPanel() {
        stockPanel.SetActive(true);
    }

    public void OpenFurniturePanel() {
        stockPanel.SetActive(false);
    }
}