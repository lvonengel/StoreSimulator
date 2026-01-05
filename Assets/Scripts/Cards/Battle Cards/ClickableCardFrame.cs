using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles deck managing where clicking adds a card to the deck.
/// </summary>
public class ClickableCardFrame : MonoBehaviour, IPointerClickHandler {

    private CardInfo card;

    public void Init(CardInfo cardInfo) {
        card = cardInfo;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            if (!BattleCardsDeckManagerUI.instance.CanAdd(card)) {
                return;
            }

            CustomDeckEditor.instance.AddCard(card);
            BattleCardsDeckManagerUI.instance.OnCardAdded(card);
        }
        else if (eventData.button == PointerEventData.InputButton.Right) {
            if (!BattleCardsDeckManagerUI.instance.CanRemove(card)) {
                return;
            }

            CustomDeckEditor.instance.RemoveCard(card);
            BattleCardsDeckManagerUI.instance.OnCardRemoved(card);
        }
    }

}
