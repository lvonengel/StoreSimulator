using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls all UI including the price panel, buy screen, and pause screen.
/// </summary>
public class UIController : MonoBehaviour {
    public static UIController instance;

    public GameObject updatePricePanel;

    public TMP_Text basePriceText, currentPriceText;

    public TMP_InputField priceInputfield;

    private StockInfo activeStockInfo;

    public TMP_Text moneyText;

    public GameObject buyMenuScreen;

    public string mainMenuScene;

    public GameObject pauseScreen;
    public GameObject phoneScreen;

    [SerializeField] private Transform stockTemplate;
    [SerializeField] private Transform stockTemplateContainer;
    [SerializeField] private Transform furnitureTemplate;
    [SerializeField] private Transform furnitureTemplateContainer;

    private void Awake() {
        instance = this;
        CreateStockTemplates();
        CreateFurnitureTemplates();
    }

    private void Update() {
        if (Keyboard.current.tabKey.wasPressedThisFrame) {
            OpenClosePhone();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            PauseUnpause();
        }
    }

    private void CreateStockTemplates() {
        foreach (Transform child in stockTemplateContainer) {
            if (child == stockTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (StockInfo food in StockInfoController.instance.allStock) {
            Transform stockTransform = Instantiate(stockTemplate, stockTemplateContainer);
            stockTransform.gameObject.SetActive(true);
            stockTransform.GetComponent<BuyStockFrameController>().UpdateFrameInfo(food);
        }
    }

    private void CreateFurnitureTemplates() {
        foreach (Transform child in furnitureTemplateContainer) {
            if (child == furnitureTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (FurnitureInfo furniture in FurnitureInfoController.instance.furnitureInfo) {
            Transform furnitureTransform = Instantiate(furnitureTemplate, furnitureTemplateContainer);
            furnitureTransform.gameObject.SetActive(true);
            furnitureTransform.GetComponent<BuyFurnitureFrameController>().UpdateFrameInfo(furniture);
        }
    }

    public void OpenUpdatePrice(StockInfo stockToUpdate) {
        updatePricePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;

        basePriceText.text = "$" + stockToUpdate.price.ToString("F2");
        currentPriceText.text = "$" + stockToUpdate.currentPrice.ToString("F2");

        activeStockInfo = stockToUpdate;

        priceInputfield.text = stockToUpdate.currentPrice.ToString();
    }

    public void CloseUpdatePrice() {
        updatePricePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ApplyPriceUpdate() {
        activeStockInfo.currentPrice = float.Parse(priceInputfield.text);
        currentPriceText.text = "$" + activeStockInfo.currentPrice.ToString("F2");

        StockInfoController.instance.UpdatePrice(activeStockInfo.name, activeStockInfo.currentPrice);

        CloseUpdatePrice();
    }

    public void UpdateMoney(float currentMoney) {
        moneyText.text = "$" + currentMoney.ToString("F2");
    }

    public void OpenClosePhone() {
        if (phoneScreen.activeSelf == false) {
            phoneScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        } else {
            phoneScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            CloseAllPhoneApps();
            
        }
    }
    public void OpenCloseBuyMenu() {
        if (buyMenuScreen.activeSelf == false) {
            buyMenuScreen.SetActive(true);
        } else {
            buyMenuScreen.SetActive(false);
        }
    }

    public void CloseAllPhoneApps() {
        buyMenuScreen.SetActive(false);
    }

    public void MainMenu() {
        SceneManager.LoadScene(mainMenuScene);
        Time.timeScale = 1f;
    }

    public void QuitGame() {
        Application.Quit();
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