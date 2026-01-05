using System.Collections.Generic;
using UnityEngine;

public static class DeckCloner {

    public static DeckData CloneDeck(DeckData original) {
        DeckData clone = ScriptableObject.CreateInstance<DeckData>();

        clone.deckName = original.deckName;
        clone.deckIcon = original.deckIcon;
        clone.deckType = original.deckType;
        clone.MaxCards = original.MaxCards;

        clone.cards = new List<CardInfo>(original.cards);

        return clone;
    }
}
