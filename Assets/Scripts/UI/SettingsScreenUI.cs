using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the UI for the settings app on the phone.
/// </summary>
public class SettingsScreenUI : MonoBehaviour {
    
    [SerializeField] private Button saveGameButton;

    private void Awake() {
        saveGameButton.onClick.AddListener(() => {
            SaveManager.instance.SaveGame();
        });
    }

}