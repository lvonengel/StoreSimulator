using UnityEngine;

/// <summary>
/// Manages the store stats for a single day.
/// This includes how many items you sold, how much money you made/spent.
/// </summary>
public class DayStatsController : MonoBehaviour {
    public static DayStatsController instance {get; private set; }

    public int itemsSoldToday;
    public float moneySpentToday;
    public float moneyMadeToday;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        TimeController.instance.OnDayChanged += HandleNewDay;
    }

    public void RegisterMoneyMade(float price) {
        itemsSoldToday++;
        moneyMadeToday += price;
    }

    public void RegisterMoneySpent(float amount) {
        moneySpentToday += amount;
    }

    private void HandleNewDay(int newDay) {
        Cursor.lockState = CursorLockMode.None;
        EndOfDayUI.instance.Show();
        EndOfDayUI.instance.UpdateText();
    }

    public float NetMoneyToday() {
        return moneyMadeToday - moneySpentToday;
    }

    public void ResetDay() {
        itemsSoldToday = 0;
        moneySpentToday = 0;
        moneyMadeToday = 0;
    }

    public int GetItemsSold() {
        return itemsSoldToday;
    }

    public float GetMoneySpent() {
        return moneySpentToday;
    }
    public float GetMoneyMade() {
        return moneyMadeToday;
    }
}
