using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the UI for the menu
/// </summary>
public class MainMenu : MonoBehaviour {
    public static MainMenu instance {get; private set;}

    public string mainScene;
    [SerializeField] private GameObject playScreen, saveScreen;
    [SerializeField] private Button playButton, quitButton;


    [SerializeField] private Transform saveProfileTemplate, saveProfileTemplateContainer;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject confirmDeleteScreen;
    [SerializeField] private TMP_Text confirmDeleteText;
    [SerializeField] private Button confirmDeleteButton, leaveConfirmDeleteButton;

    private SaveManager.SaveSlot selectedSlot;

    private void Awake() {
        instance = this;

        saveScreen.SetActive(false);
        saveProfileTemplate.gameObject.SetActive(false);
        confirmDeleteScreen.gameObject.SetActive(false);

        //main menu screen UI
        playButton.onClick.AddListener(() => {
            saveScreen.gameObject.SetActive(true);
            playScreen.gameObject.SetActive(false);
        });
        quitButton.onClick.AddListener(() => QuitGame());

        //confirm save screen UI
        backButton.onClick.AddListener(() => {
            playScreen.SetActive(true);
            saveScreen.SetActive(false);
            
        });

        confirmDeleteButton.onClick.AddListener(() => {
            SaveManager.instance.DeleteSave(selectedSlot);
            RefreshSaveProfiles();
            HideDeleteScreen();
        });
        leaveConfirmDeleteButton.onClick.AddListener(() => HideDeleteScreen());
    }

    private void Start() {
        // AudioManager.instance.StartTitleMusic();
        CreateSaveProfiles();
    }

    public void QuitGame() {
        Application.Quit();
    }

    private void CreateSaveProfiles() {
        // destroys previous templates
        foreach (Transform child in saveProfileTemplateContainer) {
            if (child == saveProfileTemplate) continue;
            Destroy(child.gameObject);
        }

        // duplicates the original template with new items
        foreach (SaveManager.SaveSlot slot in System.Enum.GetValues(typeof(SaveManager.SaveSlot))) {
            Transform saveProfileTransform = Instantiate(saveProfileTemplate, saveProfileTemplateContainer);
            saveProfileTransform.gameObject.SetActive(true);
            saveProfileTransform.GetComponent<SaveProfileFrameTemplate>().UpdateFrameInfo(slot);
        }
    }

    private void RefreshSaveProfiles() {
        foreach (Transform child in saveProfileTemplateContainer) {
            SaveProfileFrameTemplate frame = child.GetComponent<SaveProfileFrameTemplate>();
            if (frame != null) {
                frame.Refresh();
            }
        }
    }

    public void ShowDeleteScreen(SaveManager.SaveSlot slot) {
        selectedSlot = slot;
        confirmDeleteScreen.SetActive(true);
        confirmDeleteText.text = $"Are you sure you want to delete {slot}";
    }

    public void HideDeleteScreen() {
        confirmDeleteScreen.SetActive(false);
    }

    
}