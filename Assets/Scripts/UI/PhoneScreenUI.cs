using UnityEngine;
using UnityEngine.UI;

public class PhoneScreenUI : MonoBehaviour {

    public static PhoneScreenUI instance {get; private set;}

    [SerializeField] private GameObject screen;
    public GameObject buyStockScreen, buyFurnitureScreen;
    [SerializeField] private Button buyStockButton, buyFurnitureButton;
    [SerializeField] private Button homeButton;

    [SerializeField] private Transform stockTemplate;
    [SerializeField] private Transform stockTemplateContainer;
    [SerializeField] private Transform furnitureTemplate;
    [SerializeField] private Transform furnitureTemplateContainer;

    private void Awake() {
        instance = this;
        stockTemplate.gameObject.SetActive(false);
        furnitureTemplate.gameObject.SetActive(false);

        homeButton.onClick.AddListener(() => {
            CloseAllPhoneApps();
        });
        buyStockButton.onClick.AddListener(() => {
            screen.SetActive(true);
            buyStockScreen.SetActive(true);
        });
        buyFurnitureButton.onClick.AddListener(() => {
            screen.SetActive(true);
            buyFurnitureScreen.SetActive(true);
        });
    }
    private void Start() {
        CreateStockTemplates();
        CreateFurnitureTemplates();
    }
    
    public void CloseAllPhoneApps() {
        buyStockScreen.SetActive(false);
        buyFurnitureScreen.SetActive(false);
    }

    /// <summary>
    /// Automatically creates the stock items in a grid by duplicating a template.
    /// </summary>
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

    // <summary>
    /// Automatically creates the furniture items in a grid by duplicating a template.
    /// </summary>
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

}