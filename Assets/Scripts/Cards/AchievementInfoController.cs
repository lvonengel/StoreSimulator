using System.Collections.Generic;
using UnityEngine;

public class AchievementInfoController : MonoBehaviour {
    
    public static AchievementInfoController instance {get; private set;}

    List<AchievementScreenFrameTemplate> frames = new List<AchievementScreenFrameTemplate>();

    private void Awake() {
        instance = this;
    }

    public void RegisterFrame(AchievementScreenFrameTemplate frame) {
        if (!frames.Contains(frame)) {
            frames.Add(frame);
        }
    }

    public void RefreshAllFrames() {
        foreach (AchievementScreenFrameTemplate frame in frames) {
            frame.Refresh();
        }
    }
    
    public void ClearFrames() {
        frames.Clear();
    }

}