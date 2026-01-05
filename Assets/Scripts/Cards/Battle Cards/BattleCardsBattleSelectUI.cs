using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleCardsBattleSelectUI : MonoBehaviour {
    
    [SerializeField] private Button battle1Button;
    private const string BATTLE1_SCENE = "Battle1";
    private const string BATTLE2_SCENE = "Battle2";
    private const string BATTLE3_SCENE = "Battle3";

    private void Awake() {
        battle1Button.onClick.AddListener(() => {
            SceneManager.LoadScene(BATTLE1_SCENE);
        });
    }

}