using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyAdvertisementFrameTemplate : MonoBehaviour {
    private AdvertisementInfo advertisement;
    [SerializeField] private TMP_Text nameText, priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button alreadyPurchasedButton;
    [SerializeField] private GameObject unableToBuyScreen;


    private void Awake() {
        buyButton.onClick.AddListener(() => {
            BuyAdvertisement();
        });
    }

    /// <summary>
    /// Updates a single frame template to the information on startup.
    /// </summary>
    /// <param name="advertisement">The advertisement you want this frame to be</param>
    public void UpdateFrameInfo(AdvertisementInfo advertisement) {
        this.advertisement = advertisement;

        nameText.text = advertisement.name;
        priceText.text = "Price: $" + advertisement.price.ToString("F2");

        // already purchased it
        if (advertisement.isPurchased) {
            alreadyPurchasedButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
            unableToBuyScreen.SetActive(false);
        }
        else if (CanBuy(advertisement)) {
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
        UpdateFrameInfo(advertisement);
    }

    private void BuyAdvertisement() {
        // you must buy the previous ad and this current ad is not bought yet
        if (CanBuy(advertisement)) {
            if (StoreController.instance.CheckMoneyAvailable(advertisement.price)) {
                StoreController.instance.SpendMoney(advertisement.price);
                advertisement.isPurchased = true;
                CustomerManager.instance.SetTimeBetweenCustomers(advertisement.timeBetweenCustomers);
                AdvertisementInfoController.instance.RefreshAllFrames();
            }
        }
    }

    public bool CanBuy(AdvertisementInfo advertisement) {
        // if it is already bought
        if (advertisement.isPurchased) {
            return false;
        }

        // first ad
        if (advertisement.requiredPreviousAd == null) {
            return true;
        }

        return advertisement.requiredPreviousAd.isPurchased;
    }

}