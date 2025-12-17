using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages the buttons when the user pauses mid game.
/// </summary>
public class PauseScreenUI : MonoBehaviour {
    
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private string mainMenuScene;


    private void Awake() {

        resumeButton.onClick.AddListener(() => {
            Unpause();
        });
        mainMenuButton.onClick.AddListener(() => {
            MainMenu();
        });
        quitButton.onClick.AddListener(() => {
            QuitGame();
        });
    }

    private void Unpause() {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void MainMenu() {
        SceneManager.LoadScene(mainMenuScene);
        Time.timeScale = 1f;
    }

    public void QuitGame() {
        Application.Quit();
    }

    


}