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

    private void Awake() {
        instance = this;
        bookScreen.SetActive(false);
        chooseBookScreen.SetActive(false);
        ClearSlots(leftSlots);
        ClearSlots(rightSlots);
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

            // bool opening = !bookScreen.activeSelf;
            // bookScreen.SetActive(opening);

            // if (opening) {
            //     UserControlUI.instance.HideAllControls();
            //     PopulatePages();
            //     UpdateStatTexts();
            // }
        }

        if (bookScreen.activeSelf == true) {
            if (Keyboard.current.qKey.wasPressedThisFrame) {
                PreviousPage();
            }
            if (Keyboard.current.eKey.wasPressedThisFrame) {
                NextPage();
            }
        }
    }

    private void PopulatePages() {
        ClearSlots(leftSlots);
        ClearSlots(rightSlots);

        List<CardInventoryController.CardInventoryEntry> ownedCards = CardInventoryController.instance.ownedCards;
        int startIndex = pageIndex * CARDS_PER_SPREAD;

        for (int i = 0; i < CARDS_PER_SPREAD; i++) {
            int cardIndex = startIndex + i;
            if (cardIndex >= ownedCards.Count) break;

            CardInventoryController.CardInventoryEntry entry = ownedCards[cardIndex];

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

    private void NextPage() {
        int maxPage = Mathf.Max(0, (CardInventoryController.instance.ownedCards.Count - 1) / CARDS_PER_SPREAD);

        if (pageIndex < maxPage) {
            pageIndex++;
            PopulatePages();
        }
    }

    private void PreviousPage() {
        if (pageIndex > 0) {
            pageIndex--;
            PopulatePages();
        }
    }

    private void ClearSlots(CardCollectedFrameTemplate[] slots) {
        for (int i = 0; i < slots.Length; i++) {
            slots[i].gameObject.SetActive(false);
        }
    }

    private void UpdateStatTexts() {
        int totalCards = CardInventoryController.instance.ownedCards.Count;

        cardPackNameText.text = $"Collected: {totalCards}";
        cardsCollectedText.text = $"Cards: {totalCards}";

        int maxPage = Mathf.Max(1,
            Mathf.CeilToInt(totalCards / (float)CARDS_PER_SPREAD));

        pageText.text = $"Page {pageIndex + 1} / {maxPage}";
    }


    private void CreateCardAchievementBook() {
        // AdvertisementInfoController.instance.ClearFrames();
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
                // AdvertisementInfoController.instance.RegisterFrame(frame);
                
            }
        }
    }
    
    public void OpenGivenBook(CardPack cardpack) {
        bookScreen.SetActive(true);
        PopulatePages();
        UpdateStatTexts();
    }

    

}
