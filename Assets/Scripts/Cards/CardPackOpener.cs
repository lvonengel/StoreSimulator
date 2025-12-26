using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic for pulling different rarity of cards
/// </summary>
public static class CardPackOpener {


    // Opens an entire pack of the given card pack
    public static List<CardInfo> OpenPack(CardPack pack) {
        List<CardInfo> pulledCards = new List<CardInfo>();

        for (int i = 0; i < pack.cardsPerPack; i++) {
            pulledCards.Add(PullSingleCard(pack.possibleCardsList));
        }

        return pulledCards;
    }

    // opens a single card from a pack and calculates rarity
    private static CardInfo PullSingleCard(List<CardInfo> possibleCards) {
        CardInfo.Rarity rolledRarity = RollRarity();

        // Collect cards of that rarity
        List<CardInfo> pool = new List<CardInfo>();
        for (int i = 0; i < possibleCards.Count; i++) {
            if (possibleCards[i].rarity == rolledRarity) {
                pool.Add(possibleCards[i]);
            }
        }

        if (pool.Count == 0) {
            return PullSingleCard(possibleCards); 
        }

        int index = Random.Range(0, pool.Count);
        return pool[index];
    }

    /// <summary>
    /// Helper function to randomize the rarity of the card
    /// </summary>
    /// <returns>The rarity of the card</returns>
    private static CardInfo.Rarity RollRarity() {
        float roll = Random.Range(0f, 100f);

        if (roll < 70f) {
            return CardInfo.Rarity.Common;
        }
        if (roll < 90f) {
            return CardInfo.Rarity.Uncommon;
        }
        if (roll < 98f) {
            return CardInfo.Rarity.Rare;
        }
        if (roll < 99.5f) {
            return CardInfo.Rarity.Epic;
        }

        return CardInfo.Rarity.Legendary;
    }

}
