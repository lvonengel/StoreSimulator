using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages the UI for card battle during the actual battle.
/// </summary>
public class BattleCardsBattleUI : MonoBehaviour {
    public static BattleCardsBattleUI instance;
    [SerializeField] private TMP_Text playerManaText, playerHealthText, enemyHealthText, enemyManaText;
    [SerializeField] private GameObject manaWarning;
    [SerializeField] private Button drawCardButton, endTurnButton;
    public GameObject battleEndScreen;
    [SerializeField] private Button playAgainButton, selectNewBattleButton, mainMenuButton;
    [SerializeField] private TMP_Text battleResultText;
    [SerializeField] private Button resumeButton, battleSelectButton, pausedMainMenuButton;
    [SerializeField] private GameObject pauseScreen;
    public DamageIndicatorUI playerDamage, enemyDamage;
    private float manaWarningTime = 2f;
    private float manaWarningCounter;
    private const string MAIN_MENU_SCENE = "BattleCardMainMenu";
    private const string BATTLE_SELECT_SCENE = "BattleCardBattleSelect";

    private void Awake() {
        instance = this;
        manaWarning.SetActive(false);

        drawCardButton.onClick.AddListener(() => {
            DrawCard();
        });
        endTurnButton.onClick.AddListener(() => {
            EndPlayerTurn();
        });
        playAgainButton.onClick.AddListener(() => {
            RestartLevel();
        });
        selectNewBattleButton.onClick.AddListener(() => {
            ChooseNewBattle();
        });
        mainMenuButton.onClick.AddListener(() => {
            MainMenu();
        });
        resumeButton.onClick.AddListener(() => {
            PauseUnpause();
        });
        battleSelectButton.onClick.AddListener(() => {
            ChooseNewBattle();
        });
        pausedMainMenuButton.onClick.AddListener(() => {
            MainMenu();
        });
    }

    private void Update() {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            PauseUnpause();
        }

        if (manaWarningCounter > 0) {
            manaWarningCounter -= Time.deltaTime;
            if (manaWarningCounter <= 0) {
                manaWarning.SetActive(false);
            }
        }
    }

    public void SetPlayerManaText(int manaAmount) {
        playerManaText.text = "Mana: " + manaAmount;
    }

    public void SetEnemyManaText(int manaAmount) {
        enemyManaText.text = "Mana: " + manaAmount;
    }

    public void SetPlayerHealthText(int healthAmount) {
        playerHealthText.text = "Player Health " + healthAmount;
    }

    public void SetEnemyHealthText(int healthAmount) {
        enemyHealthText.text = "Enemy Health " + healthAmount;
    }

    public void ShowManaWarning() {
        manaWarning.SetActive(true);
        manaWarningCounter = manaWarningTime;
    }

    public void DrawCard() {
        DeckController.instance.DrawCardForMana();
    }

    public void EndPlayerTurn() {
        BattleController.instance.EndPlayerTurn();
    }

    public void ShowEndTurnButton() {
        endTurnButton.gameObject.SetActive(true);
    }
    public void HideEndTurnButton() {
        endTurnButton.gameObject.SetActive(false);
    }
    public void ShowDrawCardButton() {
        drawCardButton.gameObject.SetActive(true);
    }
    public void HideDrawCardButton() {
        drawCardButton.gameObject.SetActive(false);
    }

    public void SetBattleResults(string text) {
        battleResultText.text = text;
    }

    public void MainMenu() {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
        Time.timeScale = 1f;
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void ChooseNewBattle() {
        SceneManager.LoadScene(BATTLE_SELECT_SCENE);
        Time.timeScale = 1f;
    }

    public void PauseUnpause() {
        if (pauseScreen.activeSelf == false) {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        } else {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    

}