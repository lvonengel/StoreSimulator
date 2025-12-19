using System;
using UnityEngine;

/// <summary>
/// Manages the time/day in the game.
/// For every 10 seconds, 10 minutes pass in the game.
/// Minutes is only updated every 10 seconds.
/// </summary>
public class TimeController : MonoBehaviour {
    public static TimeController instance {get; private set; }

    // hour, minute
    public event Action<int, int> OnTimeChanged; 
    public event Action<int> OnDayChanged;

    [SerializeField] private float secondsPerTick = 10f;

    private float timer;

    private int minute = 0;
    private int hour = 8;
    private int day = 1;

    private void Awake() {
        instance = this;
    }

    private void Update() {
        timer += Time.deltaTime;

        if (timer >= secondsPerTick) {
            timer -= secondsPerTick;
            AdvanceTime();
        }
    }

    private void AdvanceTime() {
        minute += 10;

        if (minute >= 60) {
            minute = 0;
            hour++;

            if (hour >= 24) {
                hour = 0;
                day++;
                OnDayChanged?.Invoke(day);
            }
        }

        OnTimeChanged?.Invoke(hour, minute);
    }

    public void StartNewDay() {
        hour = 8;
        minute = 0;
        timer = 0f;

        OnTimeChanged?.Invoke(hour, minute);
    }


    public int GetHour() {
        return hour;
    }
    public int GetMinute() {
        return minute;
    }
    public int GetDay() {
        return day;
    }
}
