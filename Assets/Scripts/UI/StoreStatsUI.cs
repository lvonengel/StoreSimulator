using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents the stats on the store including money and store level.
/// </summary>
public class StoreStatsUI : MonoBehaviour {
    public static StoreStatsUI instance {get; private set;}
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text storeLevelText;
    [SerializeField] private Image storeLevelBar;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        StoreController.instance.OnExperienceChanged += StoreController_OnExperienceChanged;
        storeLevelBar.fillAmount= 0;
    }

    private void StoreController_OnExperienceChanged(object sender, StoreController.OnExperienceChangedEventArgs e) {
        storeLevelBar.fillAmount = e.experienceNormalized;
    }

    public void UpdateMoney(float currentMoney) {
        moneyText.text = "$" + currentMoney.ToString("F2");
    }
    public void UpdateStoreLevel(int storeLevel) {
        storeLevelText.text = "Store Level: " + storeLevel.ToString();
    }
    
}