using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls all UI including the price panel, buy screen, and pause screen.
/// </summary>
public class UIController : MonoBehaviour {
    public static UIController instance;

    public GameObject updatePricePanel;

    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text storeLevelText;

    public GameObject buyStockScreen;
    public GameObject buyFurnitureScreen;

    public GameObject pauseScreen;
    public GameObject phoneScreen;


    private void Awake() {
        instance = this;
    }


    private void Update() {
        if (Keyboard.current.tabKey.wasPressedThisFrame) {
            OpenClosePhone();
        }
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            PauseUnpause();
        }
    }

    public void UpdateMoney(float currentMoney) {
        moneyText.text = "$" + currentMoney.ToString("F2");
    }
    public void UpdateStoreLevel(int storeLevel) {
        storeLevelText.text = "Store Level: " + storeLevel.ToString();
    }

    public void OpenUpdatePrice(StockInfo stockToUpdate) {
        updatePricePanel.gameObject.SetActive(true);
        UpdatePricePanelUI.instance.LoadUpdatePrice(stockToUpdate);
    }

    public void OpenClosePhone() {
        if (phoneScreen.activeSelf == false) {
            phoneScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        } else {
            phoneScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            PhoneScreenUI.instance.CloseAllPhoneApps(); 
        }
    }

    public void PauseUnpause() {
        if (pauseScreen.activeSelf == false) {
            pauseScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;

        } else {
            pauseScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
    }

}