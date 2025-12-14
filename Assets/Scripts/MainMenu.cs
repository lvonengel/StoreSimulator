using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the UI for the menu
/// </summary>
public class MainMenu : MonoBehaviour {
    public string mainScene;

    private void Start() {
        // AudioManager.instance.StartTitleMusic();
    }

    public void StartGame() {
        SceneManager.LoadScene(mainScene);
    }

    public void QuitGame() {
        Application.Quit();
    }
}