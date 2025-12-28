using TMPro;
using UnityEngine;

public class CardCollectedFrameTemplate : MonoBehaviour {

    [SerializeField] private TMP_Text quantityText;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardSpawnPoint;
    [SerializeField] private float localCardScale = .8f;
    private GameObject spawnedCard;
    private CardInfo info;

    public void UpdateFrameInfo(CardInfo card, int quantity) {
        info = card;
        quantityText.text = $"x{quantity}";

        SpawnCard();
    }

    private void SpawnCard() {
        if (spawnedCard != null) {
            Destroy(spawnedCard);
        }

        spawnedCard = Instantiate(cardPrefab, cardSpawnPoint);
        spawnedCard.transform.localScale = Vector3.one * localCardScale;

        CardDisplay display = spawnedCard.GetComponent<CardDisplay>();
        display.SetCard(info, false);
    }
}
