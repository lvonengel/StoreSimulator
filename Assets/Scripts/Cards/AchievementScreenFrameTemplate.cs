using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls a single achievement frame template in the CardsCollectedBookUI.
/// </summary>
public class AchievementScreenFrameTemplate : MonoBehaviour {
    [SerializeField] private Button bookButton;

    private CardPack info;

    private void Awake() {
        bookButton.onClick.AddListener(() => {
            OpenBook(info);
        });
    }

    public void UpdateFrameInfo(CardPack info) {
        this.info = info;
    }

    private void OpenBook(CardPack cardPack) {
        CardsCollectedBookUI.instance.OpenGivenBook(cardPack);
    }
}