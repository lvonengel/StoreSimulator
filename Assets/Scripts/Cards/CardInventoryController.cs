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

    public List<CardInventoryEntry> ownedCards = new List<CardInventoryEntry>();

    private void Awake() {
        instance = this;
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


}
