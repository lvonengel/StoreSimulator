using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle Deck")]
public class DeckData : ScriptableObject {

    public string deckName;
    public Sprite deckIcon;

    public List<CardInfo> cards = new List<CardInfo>();
    public enum DeckType { Prebuilt, Custom }
    public DeckType deckType;

    public int MaxCards = 20;

    public bool IsValidDeck() {
        return cards.Count == MaxCards;
    }
}
