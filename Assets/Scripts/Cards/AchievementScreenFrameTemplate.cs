using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls a single achievement frame template in the CardsCollectedBookUI.
/// </summary>
public class AchievementScreenFrameTemplate : MonoBehaviour {
    [SerializeField] private Button bookButton;
    [SerializeField] private TMP_Text cardPackText, collectedText;

    private CardPack info;

    private void Awake() {
        bookButton.onClick.AddListener(() => {
            OpenBook(info);
        });
    }

    /// <summary>
    /// Updates the information on each achievement frame.
    /// This includes the pack and the percent of the pack is completed.
    /// </summary>
    /// <param name="info"></param>
    public void UpdateFrameInfo(CardPack info) {
        this.info = info;
        cardPackText.text = info.packName;
        List<CardInventoryController.CardInventoryEntry> currentCardsInPack = CardInventoryController.instance.GetCurrentCardsInPack(info);
        collectedText.text = $"{(float)currentCardsInPack.Count / info.possibleCardsList.Count * 100}%";
    }

    /// <summary>
    /// Refreshes the frame for all frames
    /// </summary>
    public void Refresh() {
        UpdateFrameInfo(info);
    }

    /// <summary>
    /// Opens the given cardpack book.
    /// </summary>
    /// <param name="cardPack"></param>
    private void OpenBook(CardPack cardPack) {
        CardsCollectedBookUI.instance.OpenGivenBook(cardPack);
    }
}