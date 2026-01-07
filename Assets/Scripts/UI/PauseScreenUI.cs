using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Manages the buttons when the user pauses mid game.
/// </summary>
public class PauseScreenUI : MonoBehaviour {
    public static PauseScreenUI instance {get; private set;}
    public GameObject pauseScreen;
    
    [SerializeField] private Button resumeButton, mainMenuButton, quitButton;

    private void Awake() {
        instance = this;

        resumeButton.onClick.AddListener(() => {
            Unpause();
        });
        mainMenuButton.onClick.AddListener(() => {
            GoToMainMenu();
        });
        quitButton.onClick.AddListener(() => {
            QuitGame();
        });
    }

    private void Update() {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            PauseUnpause();
        }
    }

    private void Unpause() {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
    }

    public void GoToMainMenu() {
        Loader.Load(Loader.Scene.MainMenu);
        Time.timeScale = 1f;
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void PauseUnpause() {
        if (pauseScreen.activeSelf == false) {
            pauseScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;

        } else {
            Unpause();
        }
    }

    


}