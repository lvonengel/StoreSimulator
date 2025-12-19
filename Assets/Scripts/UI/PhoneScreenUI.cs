using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages opening apps on the home screen of the phone.
/// </summary>
public class PhoneScreenUI : MonoBehaviour {

    public static PhoneScreenUI instance {get; private set;}

    [SerializeField] private GameObject screen;
    [SerializeField] private TMP_Text timeText;
    public GameObject buyStockScreen, buyFurnitureScreen, buyAdvertisementScreen, upgradeStoreSpaceScreen;
    [SerializeField] private Button buyStockButton, buyFurnitureButton, buyAdvertisementButton, buyUpgradeStoreSpaceButton;
    [SerializeField] private Button homeButton;

    [SerializeField] private Transform furnitureTemplate;
    [SerializeField] private Transform furnitureTemplateContainer;
    [SerializeField] private Transform advertisementTemplate;
    [SerializeField] private Transform advertisementTemplateContainer;
    [SerializeField] private Transform storeSpaceTemplate;
    [SerializeField] private Transform storeSpaceTemplateContainer;

    private void Awake() {
        instance = this;
        
        furnitureTemplate.gameObject.SetActive(false);
        advertisementTemplate.gameObject.SetActive(false);
        storeSpaceTemplate.gameObject.SetActive(false);

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
        buyUpgradeStoreSpaceButton.onClick.AddListener(() => {
            screen.SetActive(true);
            upgradeStoreSpaceScreen.SetActive(true);
        });
    }

    private void OnEnable() {
        TimeController.instance.OnTimeChanged += UpdateTime;
        UpdateTime(TimeController.instance.GetHour(), TimeController.instance.GetMinute());

    }

    private void OnDisable() {
        if (TimeController.instance != null)
            TimeController.instance.OnTimeChanged -= UpdateTime;
    }

    private void UpdateTime(int hour, int minute) {
        timeText.text = $"{hour:00}:{minute:00}";
    }


    private void Start() {
        CreateFurnitureTemplates();
        CreateAdvertisementTemplates();
        CreateStoreSpaceTemplates();
    }
    
    public void CloseAllPhoneApps() {
        buyStockScreen.SetActive(false);
        buyFurnitureScreen.SetActive(false);
        buyAdvertisementScreen.SetActive(false);
        upgradeStoreSpaceScreen.SetActive(false);
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
    private void CreateStoreSpaceTemplates() {
        UpgradeStoreSpaceInfoController.instance.ClearFrames();
        foreach (Transform child in storeSpaceTemplateContainer) {
            if (child == storeSpaceTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (UpgradeStoreSpaceInfo storeSpace in UpgradeStoreSpaceInfoController.instance.storeSpaceInfo) {
            Transform storeSpaceTransform = Instantiate(storeSpaceTemplate, storeSpaceTemplateContainer);
            storeSpaceTransform.gameObject.SetActive(true);
            UpgradeStoreSpaceFrameTemplate frame = storeSpaceTransform.GetComponent<UpgradeStoreSpaceFrameTemplate>();
            frame.UpdateFrameInfo(storeSpace);
            UpgradeStoreSpaceInfoController.instance.RegisterFrame(frame);
        }
    }

}