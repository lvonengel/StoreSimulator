using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the tabs in the deck manager UI by filtering decks.
/// Uses a disabled template inside the scroll content (stock-style pattern).
/// </summary>
public class BattleCardsDeckManagerUI : MonoBehaviour {

    public static BattleCardsDeckManagerUI instance {get; private set;}

    private const string BATTLE_SELECT_SCENE = "BattleCardBattleSelect";

    
    public enum DeckFilter {All, Prebuilt, Custom}
    private DeckFilter currentFilter = DeckFilter.All;

    [SerializeField] private Button allDecksButton, prebuiltDecksButton, customDeckButton;
    [SerializeField] private Button quitScreenButton;
    [SerializeField] private Transform deckTemplateContainer;
    [SerializeField] private Transform deckTemplate;


    [SerializeField] private Transform ownedCardsTemplateContainer;
    [SerializeField] private Transform ownedCardsTemplate;
    private Dictionary<CardInfo, int> usedCardCounts = new Dictionary<CardInfo, int>();



    private enum DeckManagerMode { Browse, Edit }
    private DeckManagerMode currentMode = DeckManagerMode.Browse;

    [SerializeField] private Button createCustomDeckButton;
    [SerializeField] private GameObject editModeScreen;
    public bool IsInEditMode => currentMode == DeckManagerMode.Edit;



    private void Awake() {
        instance = this;

        deckTemplate.gameObject.SetActive(false);
        ownedCardsTemplate.gameObject.SetActive(false);
        editModeScreen.gameObject.SetActive(false);

        quitScreenButton.onClick.AddListener(() => SceneManager.LoadScene(BATTLE_SELECT_SCENE));
        createCustomDeckButton.onClick.AddListener(() => EnterEditMode(null));

        allDecksButton.onClick.AddListener(() => ApplyFilter(DeckFilter.All));
        prebuiltDecksButton.onClick.AddListener(() => ApplyFilter(DeckFilter.Prebuilt));
        customDeckButton.onClick.AddListener(() => ApplyFilter(DeckFilter.Custom));
    }

    private void Start() {
        ApplyFilter(DeckFilter.All);
        DeckPreviewUI.instance.OnEditRequested += EnterEditMode;

    }

    private void ApplyFilter(DeckFilter filter) {
        currentFilter = filter;

        List<DeckData> decksToShow = GetFilteredDecks(filter);
        CreateDeckTemplates(decksToShow);

        if (filter == DeckFilter.Custom) {
            createCustomDeckButton.gameObject.SetActive(true);
        } else {
            createCustomDeckButton.gameObject.SetActive(false);
        }
    }

    private void CreateDeckTemplates(List<DeckData> decksToShow) {
        foreach (Transform child in deckTemplateContainer) {
            if (child == deckTemplate) {
                continue;
            }
            Destroy(child.gameObject);
        }

        foreach (DeckData deck in decksToShow) {
            Transform deckTransform = Instantiate(deckTemplate, deckTemplateContainer);
            deckTransform.gameObject.SetActive(true);
            deckTransform.GetComponent<DeckListTemplateFrameUI>().Setup(deck);
        }
    }

    private List<DeckData> GetFilteredDecks(DeckFilter filter) {
        List<DeckData> allDecks = DeckSelectionController.instance.GetAllDecks();
        List<DeckData> result = new List<DeckData>();

        foreach (DeckData deck in allDecks) {
            switch (filter) {
                case DeckFilter.All:
                    result.Add(deck);
                    break;

                case DeckFilter.Prebuilt:
                    if (deck.deckType == DeckData.DeckType.Prebuilt) {
                        result.Add(deck);
                    }
                    break;

                case DeckFilter.Custom:
                    if (deck.deckType == DeckData.DeckType.Custom) {
                        result.Add(deck);
                    }
                    break;
            }
        }

        return result;
    }

    private void EnterEditMode(DeckData deckToEdit) {
        currentMode = DeckManagerMode.Edit;

        deckTemplateContainer.gameObject.SetActive(false);
        editModeScreen.SetActive(true);

        CustomDeckEditor.instance.StartEditing(deckToEdit);
        PopulateOwnedCards();

        usedCardCounts.Clear();

        DeckData deck = DeckSelectionController.instance.currentDeck;
        for (int i = 0; i < deck.cards.Count; i++) {
            CardInfo card = deck.cards[i];
            if (!usedCardCounts.ContainsKey(card)) {
                usedCardCounts[card] = 0;
            }
            usedCardCounts[card]++;
        }

    }

    public void ExitEditMode() {
        currentMode = DeckManagerMode.Browse;
        editModeScreen.SetActive(false);
        deckTemplateContainer.gameObject.SetActive(true);

        ApplyFilter(currentFilter); 
    }

    private void PopulateOwnedCards() {
        foreach (Transform child in ownedCardsTemplateContainer) {
            if (child == ownedCardsTemplate) {
                continue;
            }
            Destroy(child.gameObject);
        }

        List<CardInventoryController.CardInventoryEntry> owned = CardInventoryController.instance.ownedCards;

        for (int i = 0; i < owned.Count; i++) {
            Transform ownedCardsTransform = Instantiate(ownedCardsTemplate, ownedCardsTemplateContainer);
            ownedCardsTransform.gameObject.SetActive(true);

            int used = 0;
            if (usedCardCounts.ContainsKey(owned[i].card)) {
                used = usedCardCounts[owned[i].card];
            }

            int remaining = owned[i].quantity - used;

            ownedCardsTransform.GetComponent<CardCollectedFrameTemplate>()
                .UpdateFrameInfo(owned[i].card, remaining);


            ownedCardsTransform.gameObject.AddComponent<ClickableCardFrame>()
                .Init(owned[i].card);
        }
    }

    public bool CanAdd(CardInfo card) {
        int owned = 0;
        var list = CardInventoryController.instance.ownedCards;
        for (int i = 0; i < list.Count; i++) {
            if (list[i].card == card) {
                owned = list[i].quantity;
                break;
            }
        }

        int used = 0;
        if (usedCardCounts.ContainsKey(card)) {
            used = usedCardCounts[card];
        }

        return used < owned;
    }

    public bool CanRemove(CardInfo card) {
        return usedCardCounts.ContainsKey(card) && usedCardCounts[card] > 0;
    }

    public void OnCardAdded(CardInfo card) {
        if (!usedCardCounts.ContainsKey(card)) {
            usedCardCounts[card] = 0;
        }
        usedCardCounts[card]++;
        RefreshOwnedCard(card);
    }

    public void OnCardRemoved(CardInfo card) {
        if (!usedCardCounts.ContainsKey(card)) {
            return;
        }

        usedCardCounts[card]--;
        if (usedCardCounts[card] <= 0) {
            usedCardCounts.Remove(card);
        }

        RefreshOwnedCard(card);
    }

    private void RefreshOwnedCard(CardInfo card) {
        foreach (Transform child in ownedCardsTemplateContainer) {
            if (child == ownedCardsTemplate) {
                continue;
            }

            CardCollectedFrameTemplate frame = child.GetComponent<CardCollectedFrameTemplate>();
            if (frame == null) {
                continue;
            }

            // frame.info is private, so compare via CardDisplay
            CardDisplay display = child.GetComponentInChildren<CardDisplay>();
            if (display == null || display.cardInfo != card) {
                continue;
            }

            int owned = 0;
            var list = CardInventoryController.instance.ownedCards;
            for (int i = 0; i < list.Count; i++) {
                if (list[i].card == card) {
                    owned = list[i].quantity;
                    break;
                }
            }

            int used = 0;

            if (usedCardCounts.ContainsKey(card)) {
                used = usedCardCounts[card];
            }

            frame.UpdateFrameInfo(card, owned - used);
            return;
        }
    }





}
