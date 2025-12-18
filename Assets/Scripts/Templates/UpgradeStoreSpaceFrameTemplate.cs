using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStoreSpaceFrameTemplate : MonoBehaviour {
    
    private UpgradeStoreSpaceInfo storeSpace;
    [SerializeField] private TMP_Text nameText, priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button alreadyPurchasedButton;
    [SerializeField] private GameObject unableToBuyScreen;


    private void Awake() {
        buyButton.onClick.AddListener(() => {
            BuyStoreSpace();
        });
    }

    /// <summary>
    /// Updates a single frame template to the information on startup.
    /// </summary>
    /// <param name="UpgradeStoreSpaceInfo">The store space you want this frame to be</param>
    public void UpdateFrameInfo(UpgradeStoreSpaceInfo storeSpace) {
        this.storeSpace = storeSpace;

        nameText.text = storeSpace.name;
        priceText.text = "Price: $" + storeSpace.price.ToString("F2");

        // already purchased it
        if (storeSpace.isPurchased) {
            alreadyPurchasedButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
            unableToBuyScreen.SetActive(false);
        }
        else if (CanBuy(storeSpace)) {
            // didn't purchase it, but you can buy it
            alreadyPurchasedButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);
            unableToBuyScreen.SetActive(false);
        } else {
            // didn't purchase it, but you can't buy it
            alreadyPurchasedButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
            unableToBuyScreen.SetActive(true);
        }
    }

    public void Refresh() {
        UpdateFrameInfo(storeSpace);
    }

    private void BuyStoreSpace() {
        // you must buy the previous store space and this current store space is not bought yet
        if (CanBuy(storeSpace)) {
            if (StoreController.instance.CheckMoneyAvailable(storeSpace.price)) {
                StoreController.instance.SpendMoney(storeSpace.price);
                storeSpace.isPurchased = true;
                UpgradeStoreSpaceInfoController.instance.ApplyPurchasedUpgrades();
                UpgradeStoreSpaceInfoController.instance.RefreshAllFrames();
            }
        }
    }

    public bool CanBuy(UpgradeStoreSpaceInfo storeSpace) {
        // if it is already bought
        if (storeSpace.isPurchased) {
            return false;
        }

        // first ad
        if (storeSpace.requiredPreviousStoreSpace == null) {
            return true;
        }

        return storeSpace.requiredPreviousStoreSpace.isPurchased;
    }


}