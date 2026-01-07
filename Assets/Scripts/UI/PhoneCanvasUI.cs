using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Controls all UI including the price panel, buy screen, and pause screen.
/// </summary>
public class PhoneCanvasUI : MonoBehaviour {
    public static PhoneCanvasUI instance {get; private set;}

    public GameObject updatePricePanel;

    [SerializeField] private Button closeFurnitureScreenButton;

    public GameObject buyStockScreen, buyFurnitureScreen;

    public GameObject phoneScreen, endOfDayScreen;


    private void Awake() {
        instance = this;
        closeFurnitureScreenButton.onClick.AddListener(() => {
            buyFurnitureScreen.SetActive(false);
        });
    }


    private void Update() {
        if (Keyboard.current.tabKey.wasPressedThisFrame) {
            OpenClosePhone();
            UserControlUI.instance.HideAllControls();
            StoreStatCanvasUI.instance.ShowHideDayStats();
        }
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
            ClosePhone();
        }
    }

    public void ClosePhone() {
        phoneScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        PhoneHomeScreenUI.instance.CloseAllPhoneApps(); 
    }

}