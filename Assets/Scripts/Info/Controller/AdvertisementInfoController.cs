using System;
using System.Collections.Generic;
using UnityEngine;

public class AdvertisementInfoController : MonoBehaviour {
    public static AdvertisementInfoController instance {get; private set;}

    public List<AdvertisementInfo> advertisementInfo;
    public List<BuyAdvertisementFrameTemplate> frames = new();
    
    private void Awake() {
        instance = this;
    }
    
    public void RegisterFrame(BuyAdvertisementFrameTemplate frame) {
        if (!frames.Contains(frame)) {
            frames.Add(frame);
        }
    }

    public void RefreshAllFrames() {
        foreach (BuyAdvertisementFrameTemplate frame in frames) {
            frame.Refresh();
        }
    }

    public void ClearFrames() {
        frames.Clear();
    }

    public List<string> GetPurchasedAdvertisements() {
        List<string> purchased = new();

        foreach (AdvertisementInfo ad in advertisementInfo) {
            if (ad.isPurchased) {
                purchased.Add(ad.advertisementName);
            }
        }

        return purchased;
    }

    public void LoadPurchasedAdvertisements(List<string> purchasedNames) {
        if (purchasedNames == null) {
            return;
        }

        foreach (AdvertisementInfo ad in advertisementInfo) {
            ad.isPurchased = purchasedNames.Contains(ad.advertisementName);
        }

        ApplyActiveAdvertisement();
        RefreshAllFrames();
    }

    private void ApplyActiveAdvertisement() {
        AdvertisementInfo activeAd = null;

        foreach (AdvertisementInfo ad in advertisementInfo) {
            if (ad.isPurchased) {
                activeAd = ad;
            }
        }

        if (activeAd != null) {
            CustomerManager.instance
                .SetTimeBetweenCustomers(activeAd.timeBetweenCustomers);
        }
    }



}