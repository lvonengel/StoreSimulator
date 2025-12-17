using System;
using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour {

    public static CartController instance { get; private set; }

    private List<CartItem> cartList;
    private float deliveryCost;
    [SerializeField] private StockBoxController boxToSpawn;

    [SerializeField] private Transform cartItemTemplate;
    [SerializeField] private Transform cartItemTemplateContainer;

    public event EventHandler OnCartChanged;

    private void Awake() {
        instance = this;
        cartList = new List<CartItem>();
        deliveryCost = 0f;

        cartItemTemplate.gameObject.SetActive(false);
    }


    public void DeleteItem(StockInfo stock) {
        for (int i = 0; i < cartList.Count; i++) {
            if (cartList[i].stock == stock) {
                cartList.RemoveAt(i);
                UpdateCart();
                return;
            }
        }
    }

    /// <summary>
    /// Adds an item to a cart. Checks if it exists already
    /// and if it does, adds to the quantity.
    /// </summary>
    /// <param name="stock"></param>
    /// <param name="amount"></param>
    public void AddToCart(StockInfo stock, int amount = 1) {
        foreach (CartItem item in cartList) {
            if (item.stock == stock) {
                item.quantity += amount;
                UpdateCart();
                return;
            }
        }

        cartList.Add(new CartItem(stock, amount));
        UpdateCart();
    }

    public float GetBoxCost(StockInfo item) {
        return boxToSpawn.GetStockAmount(item.typeOfStock) * item.price;
    }

    /// <summary>
    /// Gets the total cost of the items in the cart.
    /// </summary>
    public float GetCartSubtotal() {
        float subtotal = 0f;
        foreach (CartItem item in cartList) {
            subtotal += GetBoxCost(item.stock) * item.quantity;
        }
        return subtotal;
    }

    /// <summary>
    /// Gets the delivery cost depending on how many items in the cart.
    /// </summary>
    public float GetDeliveryCost() {
        int totalItems = 0;
        foreach (CartItem item in cartList) totalItems += item.quantity;
        if (totalItems == 0) deliveryCost = 0f;
        else if (totalItems > 20) deliveryCost = 100f;
        else if (totalItems > 15) deliveryCost = 50f;
        else deliveryCost = 15f;

        return deliveryCost;
    }

    /// <summary>
    /// Gets the total cost of the cart
    /// </summary>
    /// <returns>The total cost of the cart</returns>
    public float GetTotalCost() {
        return GetCartSubtotal() + GetDeliveryCost();
    }

    /// <summary>
    /// Updates the cart list frame
    /// </summary>
    public void UpdateCart() {
        OnCartChanged?.Invoke(this, EventArgs.Empty);

        // destroys previous templates
        foreach (Transform child in cartItemTemplateContainer) {
            if (child == cartItemTemplate) continue;
            Destroy(child.gameObject);
        }

        // duplicates the original template with new items
        foreach (CartItem item in cartList) {
            Transform cartItemTransform = Instantiate(cartItemTemplate, cartItemTemplateContainer);
            cartItemTransform.gameObject.SetActive(true);
            cartItemTransform.GetComponent<CartItemFrame>().UpdateFrameInfo(item.stock, item.quantity);
        }
    }

    /// <summary>
    /// Buys the cart and clears it
    /// </summary>
    public void BuyCart() {
        if (StoreController.instance.CheckMoneyAvailable(GetTotalCost()) == true) {
            StoreController.instance.SpendMoney(GetTotalCost());
            // spawns each item in the cart
            foreach (CartItem item in cartList) {
                for (int i = 0; i < item.quantity; i ++) {
                    Instantiate(boxToSpawn, StoreController.instance.stockSpawnPoint.position, Quaternion.identity).SetupBox(item.stock);
                }
            }
            cartList.Clear();
            UpdateCart();
        }
    }
}
