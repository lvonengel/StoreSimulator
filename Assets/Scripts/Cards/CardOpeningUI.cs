using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Handles the UI for opening a card pack
/// </summary>
public class CardOpeningUI : MonoBehaviour {
    public static CardOpeningUI instance {get; private set;}

    public event EventHandler OnPackOpeningFinished;

    public enum CardOpeningState {displayCardPack, openingPacks, displayAllCards}

    [SerializeField] private Image cardPackImage;
    [SerializeField] private Transform revealParent;
    [SerializeField] private Transform allCardsParent;
    [SerializeField] private GameObject cardPrefab;

    private CardOpeningState cardOpeningState;

    private List<CardInfo> pulledCards = new List<CardInfo>();
    private int revealIndex;

    private GameObject currentRevealCard;

    private void Awake() {
        instance = this;
        gameObject.SetActive(false);
    }

    private void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            AdvanceOpeningState();
        }
    }

    // called when a pack is opened
    public void ShowPackOpening(List<CardInfo> cards) {
        pulledCards = cards;
        revealIndex = 0;

        ClearAllParents();

        cardPackImage.gameObject.SetActive(true);
        revealParent.gameObject.SetActive(true);
        allCardsParent.gameObject.SetActive(false);

        cardOpeningState = CardOpeningState.displayCardPack;
        gameObject.SetActive(true);
    }

    private void AdvanceOpeningState() {
        switch (cardOpeningState) {

            case CardOpeningState.displayCardPack:
                cardPackImage.gameObject.SetActive(false);
                cardOpeningState = CardOpeningState.openingPacks;
                RevealNextCard();
                break;

            case CardOpeningState.openingPacks:
                RevealNextCard();
                break;

            case CardOpeningState.displayAllCards:
                CloseUI();
                OnPackOpeningFinished?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    /// <summary>
    /// Displays one card at a time, unless all cards are shown
    /// Then changes state to all cards
    /// </summary>
    private void RevealNextCard() {
        if (revealIndex >= pulledCards.Count) {
            ShowAllCards();
            cardOpeningState = CardOpeningState.displayAllCards;
            return;
        }

        if (currentRevealCard != null) {
            Destroy(currentRevealCard);
        }


        CardInfo card = pulledCards[revealIndex];
        bool isNew = !CardInventoryController.instance.HasCard(card);
        
        currentRevealCard = Instantiate(cardPrefab, revealParent);
        CardDisplay display = currentRevealCard.GetComponent<CardDisplay>();

        display.SetCard(pulledCards[revealIndex], isNew);

        revealIndex++;
    }

    /// <summary>
    /// Displays all cards in a horizontal group after showing each card
    /// </summary>
    private void ShowAllCards() {
        if (currentRevealCard != null) {
            Destroy(currentRevealCard);
            currentRevealCard = null;
        }

        revealParent.gameObject.SetActive(false);
        allCardsParent.gameObject.SetActive(true);

        for (int i = 0; i < pulledCards.Count; i++) {
            CardInfo card = pulledCards[i];
            bool isNew = !CardInventoryController.instance.HasCard(card);

            GameObject cardObj = Instantiate(cardPrefab, allCardsParent);
            CardDisplay display = cardObj.GetComponent<CardDisplay>();

            display.SetCard(pulledCards[i], isNew);
        }
    }

    private void CloseUI() {
        ClearAllParents();
        gameObject.SetActive(false);
    }

    private void ClearAllParents() {
        ClearChildren(revealParent);
        ClearChildren(allCardsParent);
        currentRevealCard = null;
    }

    private void ClearChildren(Transform parent) {
        for (int i = parent.childCount - 1; i >= 0; i--) {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

}
