using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the card achievements and book UI.
/// </summary>
public class CardsCollectedBookUI : MonoBehaviour {

    public static CardsCollectedBookUI instance {get; private set;}

    public GameObject bookScreen, chooseBookScreen;
    [SerializeField] private TMP_Text cardPackNameText, cardsCollectedText, pageText;

    [SerializeField] private CardCollectedFrameTemplate[] leftSlots, rightSlots; 

    [SerializeField] private Transform achievementTemplate, achievementTemplateContainer;
    private int pageIndex = 0;
    private const int CARDS_PER_PAGE = 6;
    private const int CARDS_PER_SPREAD = CARDS_PER_PAGE * 2;
    private CardPack currentCardPackBook;

    private void Awake() {
        instance = this;
        bookScreen.SetActive(false);
        chooseBookScreen.SetActive(false);
        achievementTemplate.gameObject.SetActive(false);
        ClearSlots(leftSlots);
        ClearSlots(rightSlots);
    }

    private void Start() {
        CreateCardAchievementBook();
    }

    private void Update() {
        if (Keyboard.current.cKey.wasPressedThisFrame) {
            bool opening = !chooseBookScreen.activeSelf;
            chooseBookScreen.SetActive(opening);
            if (opening) {
                UserControlUI.instance.HideAllControls();
                Cursor.lockState = CursorLockMode.None;
            } else {
                bookScreen.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (bookScreen.activeSelf == true) {
            if (Keyboard.current.qKey.wasPressedThisFrame) {
                PreviousPage(currentCardPackBook);
            }
            if (Keyboard.current.eKey.wasPressedThisFrame) {
                NextPage(currentCardPackBook);
            }
        }
    }

    /// <summary>
    /// Updates the slots of the book screen based on the cardpack.
    /// </summary>
    /// <param name="cardPack"></param>
    private void PopulatePages(CardPack cardPack) {
        ClearSlots(leftSlots);
        ClearSlots(rightSlots);

        List<CardInventoryController.CardInventoryEntry> cardsInThisPack = CardInventoryController.instance.GetCurrentCardsInPack(cardPack);

        int startIndex = pageIndex * CARDS_PER_SPREAD;

        for (int i = 0; i < CARDS_PER_SPREAD; i++) {
            int cardIndex = startIndex + i;
            if (cardIndex >= cardsInThisPack.Count) break;

            CardInventoryController.CardInventoryEntry entry = cardsInThisPack[cardIndex];

            if (i < CARDS_PER_PAGE) {
                leftSlots[i].gameObject.SetActive(true);
                leftSlots[i].UpdateFrameInfo(entry.card, entry.quantity);
            } else {
                int rightIndex = i - CARDS_PER_PAGE;
                rightSlots[rightIndex].gameObject.SetActive(true);
                rightSlots[rightIndex].UpdateFrameInfo(entry.card, entry.quantity);
            }
        }
        UpdateStatTexts();
    }

    //goes to the next page of the book screen
    private void NextPage(CardPack cardPack) {
        int maxPage = Mathf.Max(0, (CardInventoryController.instance.ownedCards.Count - 1) / CARDS_PER_SPREAD);

        if (pageIndex < maxPage) {
            pageIndex++;
            PopulatePages(cardPack);
        }
    }

    //goes to the previous page of the book screen
    private void PreviousPage(CardPack cardPack) {
        if (pageIndex > 0) {
            pageIndex--;
            PopulatePages(cardPack);
        }
    }

    /// <summary>
    /// Clears the slots on each page.
    /// </summary>
    /// <param name="slots"></param>
    private void ClearSlots(CardCollectedFrameTemplate[] slots) {
        for (int i = 0; i < slots.Length; i++) {
            slots[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the stats for the book screen.
    /// This includes which pack you're looking at, what page,
    /// and how many you've collected in this pack. 
    /// </summary>
    private void UpdateStatTexts() {
        int totalCards = CardInventoryController.instance.ownedCards.Count;

        cardPackNameText.text = $"Card Pack: {currentCardPackBook.packName}";
        cardsCollectedText.text = $"Cards: {CardInventoryController.instance.GetCurrentCardsInPack(currentCardPackBook).Count}/{currentCardPackBook.possibleCardsList.Count}";

        int maxPage = Mathf.Max(1,
            Mathf.CeilToInt(totalCards / (float)CARDS_PER_SPREAD));

        pageText.text = $"Page {pageIndex + 1} / {maxPage}";
    }


    /// <summary>
    /// Creates the achievement templates based on the stockinfo list for cardpacks.
    /// </summary>
    private void CreateCardAchievementBook() {
        AchievementInfoController.instance.ClearFrames();
        foreach (Transform child in achievementTemplateContainer) {
            if (child == achievementTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (StockInfo cardInfo in StockInfoController.instance.GetCardPackInfo()) {
            if (cardInfo.cardPack != null) {
                Transform achievementTransform = Instantiate(achievementTemplate, achievementTemplateContainer);
                achievementTransform.gameObject.SetActive(true);
                AchievementScreenFrameTemplate frame = achievementTransform.GetComponent<AchievementScreenFrameTemplate>();
                frame.UpdateFrameInfo(cardInfo.cardPack);
                AchievementInfoController.instance.RegisterFrame(frame);
                
            }
        }
    }
    
    /// <summary>
    /// Opens the book screen with just the given card pack
    /// </summary>
    /// <param name="cardPack">The pack of cards you want the book screen to show</param>
    public void OpenGivenBook(CardPack cardPack) {
        currentCardPackBook = cardPack;
        bookScreen.SetActive(true);
        PopulatePages(cardPack);
        UpdateStatTexts();
    }
    

}
