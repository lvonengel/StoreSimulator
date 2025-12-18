[System.Serializable]

//Represents a unique item in the cart
public class CartItem {
    public StockInfo stock;
    public int quantity;

    public CartItem(StockInfo stock, int quantity) {
        this.stock = stock;
        this.quantity = quantity;
    }
}
