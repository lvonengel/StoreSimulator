using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleCardsMainMenuUI : MonoBehaviour {
    
    [SerializeField] private Button playButton, quitGameButton;
    [SerializeField] private GameObject ownedCardsWarning;
    private const string BATTLE_SCENE = "BattleCardBattleSelect";
    private const string SHOP_SCENE = "Main Shop";

    private float ownedCardsWarningTime = 2f;
    private float ownedCardsWarningCounter;

    private void Start() {
        ownedCardsWarning.SetActive(false);
        playButton.onClick.AddListener(() => {
            if (CardInventoryController.instance.ownedCards.Count > 0) {
                SceneManager.LoadScene(BATTLE_SCENE);
            } else {
                ShowOwnedCardsWarning();
            }
        });
        quitGameButton.onClick.AddListener(() => {
            SceneManager.LoadScene(SHOP_SCENE);
        });
    }

    private void Update() {
        if (ownedCardsWarningCounter > 0) {
            ownedCardsWarningCounter -= Time.deltaTime;
            if (ownedCardsWarningCounter <= 0) {
                ownedCardsWarning.SetActive(false);
            }
        }
    }

    public void ShowOwnedCardsWarning() {
        ownedCardsWarning.SetActive(true);
        ownedCardsWarningCounter = ownedCardsWarningTime;
    }

}
