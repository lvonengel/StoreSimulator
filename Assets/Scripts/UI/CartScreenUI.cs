using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartScreenUI : MonoBehaviour {

    [SerializeField] private TMP_Text subtotalValue, deliveryValue, totalValue;
    [SerializeField] private Button buyCartButton, closeButton;



    private void Awake() {
        buyCartButton.onClick.AddListener(() => {
            CartController.instance.BuyCart();
            UIController.instance.OpenClosePhone();
            Hide();
        });
        closeButton.onClick.AddListener(() => {
            gameObject.SetActive(false);
        });

        subtotalValue.text = "$0.00";
        deliveryValue.text = "$0.00";
        totalValue.text = "$0.00";
    }

    private void OnEnable() {
        CartController.instance.OnCartChanged += CartController_OnCartChanged;
        UpdateSummary();
    }

    private void OnDisable() {
        if (CartController.instance != null) {
            CartController.instance.OnCartChanged -= CartController_OnCartChanged;
        }
    }

    //Fired when the cart changes
    // mostly for UI
    private void CartController_OnCartChanged(object sender, System.EventArgs e) {
        UpdateSummary();
    }

    private void UpdateSummary() {
        subtotalValue.text = CartController.instance.GetCartSubtotal().ToString("F2");
        deliveryValue.text = CartController.instance.GetDeliveryCost().ToString("F2");
        totalValue.text = CartController.instance.GetTotalCost().ToString("F2");
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

}