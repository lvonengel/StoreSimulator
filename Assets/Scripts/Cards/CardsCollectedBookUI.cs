using UnityEngine;
using UnityEngine.InputSystem;

public class CardsCollectedBookUI : MonoBehaviour {

    [SerializeField] private GameObject bookScreen;

    [SerializeField] private CardCollectedFrameTemplate[] leftSlots;
    [SerializeField] private CardCollectedFrameTemplate[] rightSlots; 

    private const int CARDS_PER_PAGE = 6;

    private void Awake() {
        bookScreen.SetActive(false);
        ClearSlots(leftSlots);
        ClearSlots(rightSlots);
    }

    private void Update() {
        if (Keyboard.current.cKey.wasPressedThisFrame) {
            bool opening = !bookScreen.activeSelf;
            bookScreen.SetActive(opening);

            if (opening) {
                PopulatePages();
            }
        }
    }

    private void PopulatePages() {
        ClearSlots(leftSlots);
        ClearSlots(rightSlots);

        var ownedCards = CardInventoryController.instance.ownedCards;

        for (int i = 0; i < ownedCards.Count && i < CARDS_PER_PAGE * 2; i++) {
            var entry = ownedCards[i];

            if (i < CARDS_PER_PAGE) {
                leftSlots[i].gameObject.SetActive(true);
                leftSlots[i].UpdateFrameInfo(entry.card, entry.quantity);
            } else {
                int rightIndex = i - CARDS_PER_PAGE;
                rightSlots[rightIndex].gameObject.SetActive(true);
                rightSlots[rightIndex].UpdateFrameInfo(entry.card, entry.quantity);
            }
        }
    }

    private void ClearSlots(CardCollectedFrameTemplate[] slots) {
        for (int i = 0; i < slots.Length; i++) {
            slots[i].gameObject.SetActive(false);
        }
    }
}
