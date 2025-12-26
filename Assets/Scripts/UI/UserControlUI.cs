using UnityEngine;

/// <summary>
/// Controls the UI in the bottom left corner to display
/// what the player can do.
/// </summary>
public class UserControlUI : MonoBehaviour {

    public static UserControlUI instance {get; private set;}
    
    public GameObject nothingInHandControls, stockInHandControls, boxInHandControls;
    public GameObject furnitureControls, openingPackControls;

    private void Awake() {
        instance = this;
    }

    public void ShowOnlyControls(GameObject controlScreen) {
        nothingInHandControls.SetActive(false);
        stockInHandControls.SetActive(false);
        boxInHandControls.SetActive(false);
        furnitureControls.SetActive(false);
        openingPackControls.SetActive(false);

        if (controlScreen != null) {
            controlScreen.SetActive(true);
        }
    }

}