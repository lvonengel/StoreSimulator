using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleCardsBattleSelectUI : MonoBehaviour {
    
    [SerializeField] private Button battle1Button, manageDeckButton;
    private const string BATTLE_SCENE = "Battle";
    private const string DECK_MANAGER_SCENE = "BattleCardDeckManager";

    private void Awake() {
        battle1Button.onClick.AddListener(() => {
            SceneManager.LoadScene(BATTLE_SCENE);
        });
        manageDeckButton.onClick.AddListener(() => {
            SceneManager.LoadScene(DECK_MANAGER_SCENE);
        });
    }

}