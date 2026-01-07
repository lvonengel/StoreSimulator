using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStoreSpaceInfoController : MonoBehaviour {
    public static UpgradeStoreSpaceInfoController instance {get; private set;}

    public List<UpgradeStoreSpaceInfo> storeSpaceInfo;
    public List<UpgradeStoreSpaceFrameTemplate> frames = new();

    [Serializable]
    public class StoreSpaceUnlock {
        public UpgradeStoreSpaceInfo upgrade;
        public List<GameObject> wallToDisable;
    }

    public List<StoreSpaceUnlock> storeSpaceUnlocks = new();

    
    private void Awake() {
        instance = this;
    }
    
    public void RegisterFrame(UpgradeStoreSpaceFrameTemplate frame) {
        if (!frames.Contains(frame)) {
            frames.Add(frame);
        }
    }

    public void RefreshAllFrames() {
        foreach (UpgradeStoreSpaceFrameTemplate frame in frames) {
            frame.Refresh();
        }
    }

    public void ClearFrames() {
        frames.Clear();
    }

    public void ApplyPurchasedUpgrades() {

        foreach (StoreSpaceUnlock unlock in storeSpaceUnlocks) {
            if (unlock.wallToDisable == null) {
                continue;
            }

            foreach (GameObject wall in unlock.wallToDisable) {
                wall.SetActive(true);
            }
        }

        foreach (StoreSpaceUnlock unlock in storeSpaceUnlocks) {
            if (unlock.upgrade.isPurchased && unlock.wallToDisable != null) {
                foreach (GameObject wall in unlock.wallToDisable) {
                    wall.SetActive(false);
                }
            }
        }
    }


    public List<string> GetPurchasedStoreSpaces() {
        List<string> purchased = new();

        foreach (UpgradeStoreSpaceInfo info in storeSpaceInfo) {
            if (info.isPurchased) {
                purchased.Add(info.storeSpaceName);
            }
        }

        return purchased;
    }

    public void LoadPurchasedStoreSpaces(List<string> purchasedNames) {
        if (purchasedNames == null) {
            return;
        }

        foreach (UpgradeStoreSpaceInfo info in storeSpaceInfo) {
            info.isPurchased = purchasedNames.Contains(info.storeSpaceName);
        }

        ApplyPurchasedUpgrades();
        RefreshAllFrames();
    }




}