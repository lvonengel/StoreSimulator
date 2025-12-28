using UnityEngine;

/// <summary>
/// Controls the screen for the store stats
/// </summary>
public class StoreStatCanvasUI : MonoBehaviour {
    public static StoreStatCanvasUI instance {get; private set;}
    
    [SerializeField] private GameObject dayStatsUI;
    [SerializeField] private GameObject centerDot;

    private void Awake() {
        instance = this;
    }

    private void Update() {
        if (Cursor.lockState == CursorLockMode.None) {
            centerDot.SetActive(false);
        } else {
            centerDot.SetActive(true);
        }
    }

    public void ShowHideDayStats() {
        if (dayStatsUI.activeSelf == true) {
            dayStatsUI.SetActive(false);
        } else {
            dayStatsUI.SetActive(true);
        }
    }


}