using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a card pack
/// </summary>
[CreateAssetMenu(fileName = "New Card Pack", menuName = "CardPack", order = 2)]
public class CardPack : ScriptableObject {
    public string packName;
    public List<CardInfo> possibleCardsList = new List<CardInfo>();
    [SerializeField] private int cardsPerPack = 5;

}