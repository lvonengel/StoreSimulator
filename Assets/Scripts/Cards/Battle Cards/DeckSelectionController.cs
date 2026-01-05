using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores all decks and emits events when a deck is selected.
/// </summary>
public class DeckSelectionController : MonoBehaviour {
    public static DeckSelectionController instance;

    public event Action<DeckData> OnDeckSelected;
    public DeckData currentDeck {get; private set;}

    [SerializeField] private List<DeckData> prebuiltDecks;
    private List<DeckData> customDecks = new List<DeckData>();

    private void Awake() {
        instance = this;
    }

    public void SelectDeck(DeckData deck) {
        currentDeck = deck;
        OnDeckSelected?.Invoke(deck);
    }

    /// <summary>
    /// Returns all decks (prebuilt + custom).
    /// </summary>
    public List<DeckData> GetAllDecks() {
        List<DeckData> all = new List<DeckData>();
        all.AddRange(prebuiltDecks);
        all.AddRange(customDecks);
        return all;
    }

    public void AddCustomDeck(DeckData deck) {
        if (!customDecks.Contains(deck)) {
            customDecks.Add(deck);
        }
    }
}
