using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class CardInfo : ScriptableObject {
    
    public string cardName;
    public enum Rarity { Common, Uncommon, Rare, Epic, Legendary}
    public Rarity rarity;

    [TextArea]
    public string actionDescription, cardLore;
    public int attackPower, currentHealth, manaCost;

    public Sprite characterSprite, bgSprite;
}