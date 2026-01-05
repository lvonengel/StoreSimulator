using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour {
    public static DeckController instance {get; private set;}
    [SerializeField] private bool isTesting;
    [SerializeField] private CardPack testingPack;
    
    public Card cardToSpawn;
    private List<CardInfo> deckToUse = new List<CardInfo>();
    private List<CardInfo> activeCards = new List<CardInfo>();
    private int drawCardCost = 2;
    private float waitBetweenDrawingCards = .25f;


    private void Awake() {
        instance = this;

        if (isTesting) {
            foreach (CardInfo info in testingPack.possibleCardsList) {
                deckToUse.Add(info);
            }
        } else {
            // adds the user's deck to be the cards they own
            if (CardInventoryController.instance.ownedCards.Count > 0) {
                foreach (CardInventoryController.CardInventoryEntry entry in CardInventoryController.instance.ownedCards) {
                    deckToUse.Add(entry.card);
                }
            } 
        }
    }

    private void Start() {
        SetupDeck();
    }

    public void SetupDeck() {
        activeCards.Clear();

        List<CardInfo> tempDeck = new List<CardInfo>();
        tempDeck.AddRange(deckToUse);

        int iterations = 0;
        while (tempDeck.Count > 0 && iterations < 500) {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);
            iterations++;
        }
    }

    public void DrawCardToHand() {
        if (activeCards.Count == 0) {
            SetupDeck();
        }
        Card newCard = Instantiate(cardToSpawn, transform.position, transform.rotation);
        newCard.cardInfo = activeCards[0];
        newCard.SetupCard();

        activeCards.RemoveAt(0);
        HandController.instance.AddCardToHand(newCard);
    }

    public void DrawCardForMana() {
        if (BattleController.instance.playerMana >= drawCardCost) {
            DrawCardToHand();
            BattleController.instance.SpendPlayerMana(drawCardCost);
        } else {
            BattleCardsBattleUI.instance.ShowManaWarning();
        }
    }

    public void DrawMultipleCards(int amountToDraw) {
        StartCoroutine(DrawMultipleCo(amountToDraw));
    }

    IEnumerator DrawMultipleCo(int amountToDraw) {
        for (int i = 0; i < amountToDraw; i++) {
            DrawCardToHand();

            yield return new WaitForSeconds(waitBetweenDrawingCards);
        }
    }

}
