using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages opening apps on the home screen of the phone.
/// </summary>
public class PhoneScreenUI : MonoBehaviour {

    public static PhoneScreenUI instance {get; private set;}

    [SerializeField] private GameObject screen;
    public GameObject buyStockScreen, buyFurnitureScreen, buyAdvertisementScreen;
    [SerializeField] private Button buyStockButton, buyFurnitureButton, buyAdvertisementButton;
    [SerializeField] private Button homeButton;

    [SerializeField] private Transform furnitureTemplate;
    [SerializeField] private Transform furnitureTemplateContainer;
    [SerializeField] private Transform advertisementTemplate;
    [SerializeField] private Transform advertisementTemplateContainer;

    private void Awake() {
        instance = this;
        
        furnitureTemplate.gameObject.SetActive(false);
        advertisementTemplate.gameObject.SetActive(false);

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
        buyAdvertisementButton.onClick.AddListener(() => {
            screen.SetActive(true);
            buyAdvertisementScreen.SetActive(true);
        });
    }
    private void Start() {
        CreateFurnitureTemplates();
        CreateAdvertisementTemplates();
    }
    
    public void CloseAllPhoneApps() {
        buyStockScreen.SetActive(false);
        buyFurnitureScreen.SetActive(false);
        buyAdvertisementScreen.SetActive(false);
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
            furnitureTransform.GetComponent<BuyFurnitureFrameTemplate>().UpdateFrameInfo(furniture);
        }
    }

    private void CreateAdvertisementTemplates() {
        AdvertisementInfoController.instance.ClearFrames();
        foreach (Transform child in advertisementTemplateContainer) {
            if (child == advertisementTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (AdvertisementInfo advertisement in AdvertisementInfoController.instance.advertisementInfo) {
            Transform advertisementTransform = Instantiate(advertisementTemplate, advertisementTemplateContainer);
            advertisementTransform.gameObject.SetActive(true);
            BuyAdvertisementFrameTemplate frame = advertisementTransform.GetComponent<BuyAdvertisementFrameTemplate>();
            frame.UpdateFrameInfo(advertisement);
            AdvertisementInfoController.instance.RegisterFrame(frame);
        }
    }

}