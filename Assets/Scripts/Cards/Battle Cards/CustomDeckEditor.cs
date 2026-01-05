using System.Collections.Generic;
using UnityEngine;

public class CustomDeckEditor : MonoBehaviour {
    
    public static CustomDeckEditor instance {get; private set;}

    public bool IsCreatingNewDeck { get; private set; }

    private DeckData originalDeck, workingDeck;

    private void Awake() {
        instance = this;
    }

    public void StartEditing(DeckData baseDeck = null) {
        if (baseDeck != null) {
            IsCreatingNewDeck = false;
            originalDeck = baseDeck;
            workingDeck = DeckCloner.CloneDeck(baseDeck);
        } else {
            IsCreatingNewDeck = true;
            originalDeck = null;
            workingDeck = ScriptableObject.CreateInstance<DeckData>();
            workingDeck.deckType = DeckData.DeckType.Custom;
            workingDeck.cards = new List<CardInfo>();
        }

        DeckSelectionController.instance.SelectDeck(workingDeck);
    }

    public void SetDeckName(string name) {
        if (workingDeck != null) {
            workingDeck.deckName = name;
        }
    }


    public void AddCard(CardInfo card) {
        if (workingDeck.cards.Count >= workingDeck.MaxCards) {
            return;
        }

        workingDeck.cards.Add(card);
        DeckSelectionController.instance.SelectDeck(workingDeck);
    }

    public void RemoveCard(CardInfo card) {
        workingDeck.cards.Remove(card);
        DeckSelectionController.instance.SelectDeck(workingDeck);
    }

    public void FinishEditing() {
        if (originalDeck != null) {
            originalDeck.cards = new List<CardInfo>(workingDeck.cards);
        } else {
            DeckSelectionController.instance.AddCustomDeck(workingDeck);
        }
    }


}
