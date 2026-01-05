using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

/// <summary>
/// Manages the preview UI for the deck.
/// Shows deck name, total card count, and grouped card list with quantities.
/// </summary>
public class DeckPreviewUI : MonoBehaviour {

    public static DeckPreviewUI instance {get; private set;}

    [SerializeField] private TMP_Text deckNameText, deckCountText;
    [SerializeField] private TMP_InputField deckNameInput;

    [SerializeField] private Image deckImage;
    [SerializeField] private Button editCustomButton, saveCustomButton;

    [SerializeField] private Transform cardTemplateContainer;
    [SerializeField] private Transform cardTemplate;
    [SerializeField] private Image deckIsNotValidImage;
    [SerializeField] private TMP_Text validDeckTextWarning;

    private DeckData currentDeck;
    public event Action<DeckData> OnEditRequested;

    private void Awake() {
        instance = this;

        ClearPreview();
        cardTemplate.gameObject.SetActive(false);
        saveCustomButton.gameObject.SetActive(false);

        editCustomButton.onClick.AddListener(() => {
            OnEditRequested?.Invoke(currentDeck);
            saveCustomButton.gameObject.SetActive(true);
        });

        saveCustomButton.onClick.AddListener(() => {
            CustomDeckEditor.instance.SetDeckName(deckNameInput.text);
            CustomDeckEditor.instance.FinishEditing();
            BattleCardsDeckManagerUI.instance.ExitEditMode();
            saveCustomButton.gameObject.SetActive(false);
        });


    }

    private void Start() {
        DeckSelectionController.instance.OnDeckSelected += UpdatePreview;
    }

    private void UpdatePreview(DeckData deck) {
        currentDeck = deck;

        bool isEditing = CustomDeckEditor.instance.IsCreatingNewDeck 
            || BattleCardsDeckManagerUI.instance.IsInEditMode;

        deckNameText.gameObject.SetActive(!isEditing);
        deckNameInput.gameObject.SetActive(isEditing);

        if (isEditing) {
            if (string.IsNullOrEmpty(deck.deckName)) {
                deckNameInput.text = "New Deck";
            } else {
                deckNameInput.text = deck.deckName;
            }

        } else {
            deckNameText.text = deck.deckName;
        }



        deckCountText.text = $"{deck.cards.Count} / {deck.MaxCards} cards";
        deckImage.sprite = deck.deckIcon;

        deckCountText.gameObject.SetActive(true);
        deckImage.gameObject.SetActive(true);

        editCustomButton.gameObject.SetActive(deck.deckType == DeckData.DeckType.Custom 
            && !CustomDeckEditor.instance.IsCreatingNewDeck);

        saveCustomButton.gameObject.SetActive(CustomDeckEditor.instance.IsCreatingNewDeck 
            || (deck.deckType == DeckData.DeckType.Custom && !CustomDeckEditor.instance.IsCreatingNewDeck));



        bool isValid = deck.cards.Count == 20;

        deckIsNotValidImage.gameObject.SetActive(!isValid);
        validDeckTextWarning.gameObject.SetActive(!isValid);
        saveCustomButton.interactable = isValid;
        

        CreateCardPreviews(deck);
    }

    private void ClearPreview() {
        deckNameText.gameObject.SetActive(false);
        deckCountText.gameObject.SetActive(false);
        deckImage.gameObject.SetActive(false);
        editCustomButton.gameObject.SetActive(false);
        deckIsNotValidImage.gameObject.SetActive(false);
        validDeckTextWarning.gameObject.SetActive(false);
        deckNameInput.gameObject.SetActive(false);

    }

    private void CreateCardPreviews(DeckData deck) {
        foreach (Transform child in cardTemplateContainer) {
            if (child == cardTemplate) {
                continue;
            }
            Destroy(child.gameObject);
        }

        Dictionary<CardInfo, int> cardCounts = new Dictionary<CardInfo, int>();
        foreach (CardInfo card in deck.cards) {
            if (cardCounts.ContainsKey(card)) {
                cardCounts[card]++;
            } else {
                cardCounts.Add(card, 1);
            }
        }

        // creates a single UI entry for each unique card
        foreach (KeyValuePair<CardInfo, int> entry in cardCounts) {
            Transform cardTransform = Instantiate(cardTemplate, cardTemplateContainer);
            cardTransform.gameObject.SetActive(true);

            cardTransform.GetComponent<CardCollectedFrameTemplate>().UpdateFrameInfo(entry.Key, entry.Value);
        }
    }
}
