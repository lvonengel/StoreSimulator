using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the display for a 2D and 3D card
/// </summary>
public class CardDisplay : MonoBehaviour {
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text manaCostText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text actionDescriptionText;
    [SerializeField] private TMP_Text loreText;
    [SerializeField] private Image characterArt;
    [SerializeField] private Image bgArt;

    private CardInfo cardInfo;

    /// <summary>
    /// Sets the card information of the card.
    /// This includes attack, health, mana, name, description, lore, and image
    /// </summary>
    /// <param name="info">The info you want to set this card to</param>
    public void SetCard(CardInfo info) {
        cardInfo = info;

        nameText.text = info.cardName;
        actionDescriptionText.text = info.actionDescription;
        loreText.text = info.cardLore;

        attackText.text = info.attackPower.ToString();
        healthText.text = info.currentHealth.ToString();
        manaCostText.text = info.manaCost.ToString();

        characterArt.sprite = info.characterSprite;
        bgArt.sprite = info.bgSprite;
    }
}
