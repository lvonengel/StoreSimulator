using TMPro;
using UnityEngine;

/// <summary>
/// Manages the UI for the time at the top of the screen.
/// </summary>
public class DayStatsUI : MonoBehaviour {
    
    [SerializeField] private TMP_Text timeText;

    private void Start() {
        timeText.text = "8:00";
        TimeController.instance.OnTimeChanged += UpdateTime;
    }

    private void UpdateTime(int hour, int minute) {
        timeText.text = $"{hour:00}:{minute:00}";
    }

}