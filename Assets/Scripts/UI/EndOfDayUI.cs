using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This represents the EOD UI to display the user's stats.
/// </summary>
public class EndOfDayUI : MonoBehaviour {

    public static EndOfDayUI instance {get; private set;}
    
    [SerializeField] private TMP_Text itemsSoldValue, moneySpentText, moneyMadeText, netMoney;
    [SerializeField] private Button continueButton;

    private void Awake() {
        instance = this;
        gameObject.SetActive(false);

        continueButton.onClick.AddListener(() => {
            DayStatsController.instance.ResetDay();
            TimeController.instance.StartNewDay();
            Cursor.lockState = CursorLockMode.Locked;
            gameObject.SetActive(false);
        });
    }

    public void UpdateText() {
        itemsSoldValue.text = DayStatsController.instance.GetItemsSold().ToString();
        moneySpentText.text = $"${DayStatsController.instance.GetMoneySpent():F2}";
        moneyMadeText.text = $"${DayStatsController.instance.GetMoneyMade():F2}";
        netMoney.text = $"${DayStatsController.instance.NetMoneyToday():F2}";
    }

    public void Show() {
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
    }


}