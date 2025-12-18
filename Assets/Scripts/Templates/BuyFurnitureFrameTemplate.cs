using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls buying the furniture in the buy panel.
/// </summary>
public class BuyFurnitureFrameTemplate : MonoBehaviour {
    private FurnitureInfo furniture;

    [SerializeField] private TMP_Text nameText, priceText;

    [SerializeField] private GameObject underleveledScreen;
    [SerializeField] private TMP_Text underleveledText;

    [SerializeField] private Button buyButton;
    [SerializeField] private Image furnitureImage;

    private void Awake() {
        buyButton.onClick.AddListener(() => {
            BuyFurniture();
        });
    }

    public void OnEnable() {
        StoreController.instance.OnStoreLevelChanged += StoreController_OnStoreLevelChanged;
        if (furniture != null) {
            RefreshBuyState();
        }
    }
    public void OnDisable() {
        StoreController.instance.OnStoreLevelChanged -= StoreController_OnStoreLevelChanged;
    }

    private void StoreController_OnStoreLevelChanged(int newLevel) {
        if (furniture == null) {
            Debug.Log("info is null");
            return;
        }

        RefreshBuyState();
    }

    public void UpdateFrameInfo(FurnitureInfo furniture) {
        this.furniture = furniture;
        // info = StockInfoController.instance.GetInfo(info.name);
        nameText.text = furniture.name;
        priceText.text = "Price: $" + furniture.price.ToString("F2");

        RefreshBuyState();
    }

    private void RefreshBuyState() {
        if (CanBuy(furniture)) {
            buyButton.gameObject.SetActive(true);
        } else {
            buyButton.gameObject.SetActive(false);
            underleveledScreen.SetActive(true);
            underleveledText.text = "MUST BE LV " + furniture.requiredStoreLevel;
        }
    }

    /// <summary>
    /// Checks whether you have the money. If you do, spawns the furniture
    /// at the spawn point.
    /// </summary>
    public void BuyFurniture() {
        if (StoreController.instance.CheckMoneyAvailable(furniture.price)) {
            StoreController.instance.SpendMoney(furniture.price);
            Instantiate(furniture.furnitureObject, StoreController.instance.furnitureSpawnPoint.position, Quaternion.identity);
        }
    }

    public bool CanBuy(FurnitureInfo furniture) {
        if (StoreController.instance.GetStoreLevel() >= furniture.requiredStoreLevel) {
            return true;
        }
        return false;
    }
}