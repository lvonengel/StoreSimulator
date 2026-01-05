using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents a single deck item in the display.
/// </summary>
public class DeckListTemplateFrameUI : MonoBehaviour, IPointerClickHandler {

    [SerializeField] private Image deckImage;
    [SerializeField] private TMP_Text deckNameText;

    private DeckData deck;

    public void Setup(DeckData deckData) {
        deck = deckData;

        deckImage.sprite = deckData.deckIcon;
        deckNameText.text = deckData.deckName;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            DeckSelectionController.instance.SelectDeck(deck);
        }
    }
}
