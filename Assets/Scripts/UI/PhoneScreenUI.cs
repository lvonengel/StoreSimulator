using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages opening apps on the home screen of the phone.
/// </summary>
public class PhoneScreenUI : MonoBehaviour {

    public static PhoneScreenUI instance {get; private set;}

    [SerializeField] private GameObject screen;
    public GameObject buyStockScreen, buyFurnitureScreen;
    [SerializeField] private Button buyStockButton, buyFurnitureButton;
    [SerializeField] private Button homeButton;

    [SerializeField] private Transform furnitureTemplate;
    [SerializeField] private Transform furnitureTemplateContainer;

    private void Awake() {
        instance = this;
        
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
        CreateFurnitureTemplates();
    }
    
    public void CloseAllPhoneApps() {
        buyStockScreen.SetActive(false);
        buyFurnitureScreen.SetActive(false);
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