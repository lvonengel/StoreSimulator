using UnityEngine;

/// <summary>
/// Controls the buy menu UI where players buy stock or furniture.
/// This controls switching the two tabs.
/// </summary>
public class BuyMenuController : MonoBehaviour {
    public GameObject stockPanel, furniturePanel;

    public void OpenStockPanel() {
        stockPanel.SetActive(true);
        furniturePanel.SetActive(false);
    }

    public void OpenFurniturePanel() {
        stockPanel.SetActive(false);
        furniturePanel.SetActive(true);
    }
}