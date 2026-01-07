using System;
using System.Collections.Generic;

/// <summary>
/// Class that specifically is used to save
/// the player's custom deck.
/// </summary>
[Serializable]
public class DeckSaveData {
    public string deckId;
    public string deckName;

    public List<string> cardIds;
}
