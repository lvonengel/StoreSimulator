using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveProfileFrameTemplate : MonoBehaviour {
    
    public SaveManager.SaveSlot saveSlot;
    [SerializeField] private TMP_Text saveSlotText;
    [SerializeField] private Button saveProfileButton, deleteProfileButton;


    private void Awake() {
        saveProfileButton.onClick.AddListener(() => SaveManager.instance.StartGame(saveSlot));
        deleteProfileButton.onClick.AddListener(() => MainMenu.instance.ShowDeleteScreen(saveSlot));
    }

    public void UpdateFrameInfo(SaveManager.SaveSlot slot) {
        saveSlot = slot;
        if (SaveManager.instance.IsSavePathExist(saveSlot)) {
            saveSlotText.text = $"Profile {saveSlot}";
            deleteProfileButton.gameObject.SetActive(true);
        } else {
            saveSlotText.text = $"Start New Profile";
            deleteProfileButton.gameObject.SetActive(false);
        }
    }

    public void Refresh() {
        UpdateFrameInfo(saveSlot);
    }

}
