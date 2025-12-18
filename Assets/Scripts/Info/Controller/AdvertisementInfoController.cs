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

}