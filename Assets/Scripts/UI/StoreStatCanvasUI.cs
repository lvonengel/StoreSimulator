using UnityEngine;

/// <summary>
/// Controls the screen for the store stats
/// </summary>
public class StoreStatCanvasUI : MonoBehaviour {
    public static StoreStatCanvasUI instance {get; private set;}
    
    [SerializeField] private GameObject dayStatsUI;

    private void Awake() {
        instance = this;
    }

    public void ShowHideDayStats() {
        if (dayStatsUI.activeSelf == true) {
            dayStatsUI.SetActive(false);
        } else {
            dayStatsUI.SetActive(true);
        }
    }


}