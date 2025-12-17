using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartItemFrame : MonoBehaviour {
    [SerializeField] private StockInfo info;

    [SerializeField] private TMP_Text nameText, quantityText;
    [SerializeField] private TMP_Text ppuText, totalText;
    [SerializeField] private Button deleteItemButton;

    /// <summary>
    /// Creates the information for each item.
    /// </summary>
    public void UpdateFrameInfo(StockInfo food, int quantity) {
        info = food;

        deleteItemButton.onClick.RemoveAllListeners();
        deleteItemButton.onClick.AddListener(() => CartController.instance.DeleteItem(info));

        nameText.text = food.name;
        float boxCost =  CartController.instance.GetBoxCost(food); 
        ppuText.text = "$" + boxCost.ToString("F2");
        quantityText.text = quantity.ToString();
        totalText.text = (boxCost * quantity).ToString("F2");
    }

}