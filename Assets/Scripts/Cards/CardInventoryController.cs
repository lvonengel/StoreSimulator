using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the cards you currently own
/// </summary>
public class CardInventoryController : MonoBehaviour {
    
    public static CardInventoryController instance {get; private set;}
    public class CardInventoryEntry {
        public CardInfo card;
        public int quantity;
    }

    public List<CardInventoryEntry> ownedCards;

    private void Awake() {
        instance = this;
        ownedCards = new List<CardInventoryEntry>();
    }

    //adds a card to the inventory
    public void AddCard(CardInfo card, int amount = 1) {
        // if one of this card already exists
        for (int i = 0; i < ownedCards.Count; i++) {
            if (ownedCards[i].card == card) {
                ownedCards[i].quantity += amount;
                return;
            }
        }

        // if its a new entry
        CardInventoryEntry newEntry = new CardInventoryEntry();
        newEntry.card = card;
        newEntry.quantity = amount;
        ownedCards.Add(newEntry);
    }

    /// adds multiple cards to the inventory
    public void AddMultipleCards(List<CardInfo> cards) {
        for (int i = 0; i < cards.Count; i++) {
            AddCard(cards[i]);
        }
        AchievementInfoController.instance.RefreshAllFrames();
    }

    // removes a card from inventory
    public void RemoveCard(CardInfo card, int amount = 1) {
        for (int i = 0; i < ownedCards.Count; i++) {
            if (ownedCards[i].card == card) {
                ownedCards[i].quantity -= amount;

                if (ownedCards[i].quantity <= 0) {
                    ownedCards.RemoveAt(i);
                }

                return;
            }
        }
    }

    public bool HasCard(CardInfo card) {
        for (int i = 0; i < ownedCards.Count; i++) {
            if (ownedCards[i].card == card) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Gets the list of the cards the player owns in a certain pack
    /// </summary>
    /// <param name="cardPack">The cardpack you want to get the number of</param>
    /// <returns></returns>
    public List<CardInventoryEntry> GetCurrentCardsInPack(CardPack cardPack) {
        List<CardInventoryEntry> cardsInThisPack = new List<CardInventoryEntry>();
        foreach (CardInventoryEntry cardEntry in ownedCards) {
            if (cardEntry.card.cardPack == cardPack) {
                cardsInThisPack.Add(cardEntry);
            }
        }
        return cardsInThisPack;
    }



}
