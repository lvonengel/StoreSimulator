using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the player movement and interactions.
/// </summary>
public class PlayerController : MonoBehaviour {

    // Movement
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private Camera theCam;

    [SerializeField] private CharacterController charCon;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float ySpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float horiRot, vertRot;
    [SerializeField] private float lookSpeed;
    
    [SerializeField] private float minLookAngle, maxLookAngle;
    [SerializeField] private float interactionRange;
    [SerializeField] private float throwForce;
    [SerializeField] private StockBoxController heldBox;
    [SerializeField] private FurnitureController heldFurniture;

    // Layers
    [SerializeField] private LayerMask whatIsStock;

    [SerializeField] private LayerMask whatIsShelf;

    [SerializeField] private LayerMask whatIsStockBox;
    [SerializeField] private LayerMask whatIsBin;

    [SerializeField] private LayerMask whatIsFurniture;
    [SerializeField] private LayerMask whatIsCheckout;

    // Hold points and stock
    private StockObject heldPickup;
    [SerializeField] private Transform holdPoint;

    [SerializeField] private Transform boxHoldPoint;

    [SerializeField] private float waitToPlaceStock;
    private float placeStockCounter;


    [SerializeField] private Transform furniturePoint;
    

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        CardOpeningUI.instance.OnPackOpeningFinished += CardOpeningUI_OnPackOpeningFinished;
    }

    //Once a pack finished opening, delete it
    private void CardOpeningUI_OnPackOpeningFinished(object sender, System.EventArgs e) {
        if (heldPickup != null) {
            Destroy(heldPickup.gameObject);
            heldPickup = null;
        }
    }

    private void Update() {
        if (UIController.instance.updatePricePanel != null) {
            if (UIController.instance.updatePricePanel.activeSelf == true) {
                return;
            }
        }

        if (UIController.instance.buyStockScreen != null) {
            if (UIController.instance.buyStockScreen.activeSelf == true) {
                return;
            }
        }
        if (UIController.instance.buyFurnitureScreen != null) {
            if (UIController.instance.buyFurnitureScreen.activeSelf == true) {
                return;
            }
        }

        if (UIController.instance.pauseScreen != null) {
            if (UIController.instance.pauseScreen.activeSelf == true) {
                return;
            }
        }
        if (UIController.instance.phoneScreen != null) {
            if (UIController.instance.phoneScreen.activeSelf == true) {
                return;
            }
        }

        if (UIController.instance.endOfDayScreen != null) {
            if (UIController.instance.endOfDayScreen.activeSelf == true) {
                return;
            }
        }

        HandleMovement();

        //Handles player interactions depending on what they are holding.
        if (heldPickup == null && heldBox == null && heldFurniture == null) {
            HandleNothingInHand();
        } else {
            if (heldPickup != null) {
                HandlePickupInHand();
            }
            if (heldBox != null) {
                HandleBoxInHand();
            }
            if (heldFurniture != null) {
                HandleFurnitureInHand();

            }

        }
    }

    /// <summary>
    /// Handles all player movement including walking around and jumping.
    /// </summary>
    private void HandleMovement() {
        Vector2 lookinput = lookAction.action.ReadValue<Vector2>();

        horiRot += lookinput.x * Time.deltaTime * lookSpeed;
        transform.rotation = Quaternion.Euler(0f, horiRot, 0f);

        vertRot -= lookinput.y * Time.deltaTime * lookSpeed;
        vertRot = Mathf.Clamp(vertRot, minLookAngle, maxLookAngle);
        theCam.transform.localRotation = Quaternion.Euler(vertRot, 0f, 0f);

        
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();


        Vector3 vertMove = transform.forward * moveInput.y;
        Vector3 horiMove = transform.right * moveInput.x;

        Vector3 moveAmount = horiMove + vertMove;
        moveAmount = moveAmount.normalized;
        moveAmount = moveAmount * moveSpeed;

        if (charCon.isGrounded == true) {
            ySpeed = 0f;

            if (jumpAction.action.WasPressedThisFrame()) {
                ySpeed = jumpForce;

                if (AudioManager.instance != null) {
                    AudioManager.instance.PlaySFX(8);
                }
            }
        }

        ySpeed = ySpeed + (Physics.gravity.y * Time.deltaTime);

        moveAmount.y = ySpeed;

        charCon.Move(moveAmount * Time.deltaTime);
    }

    /// <summary>
    /// Handles interacting with stock, box of stock, and furniture with nothing in hand.
    /// </summary>
    private void HandleNothingInHand() {
        Ray ray = theCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        RaycastHit hit;
        UserControlUI.instance.ShowOnlyControls(UserControlUI.instance.nothingInHandControls);

        //picks an object to hold
        if (Mouse.current.leftButton.wasPressedThisFrame) {

            // if the object is stock
            if (Physics.Raycast(ray, out hit, interactionRange, whatIsStock)) {

                heldPickup = hit.collider.GetComponent<StockObject>();
                heldPickup.transform.SetParent(holdPoint);
                heldPickup.Pickup();

                if (AudioManager.instance != null) {
                    AudioManager.instance.PlaySFX(6);
                }

                return;
            }

            // if the object is a box of stock
            if (Physics.Raycast(ray, out hit, interactionRange, whatIsStockBox)) {
                heldBox = hit.collider.GetComponent<StockBoxController>();

                heldBox.transform.SetParent(boxHoldPoint);
                heldBox.Pickup();

                if (heldBox.flap1.activeSelf == true) {
                    heldBox.OpenClose();
                }

                if (AudioManager.instance != null) {
                    AudioManager.instance.PlaySFX(1);
                }

                return;
            }

            // if the object is the checkout register
            if (Physics.Raycast(ray, out hit, interactionRange, whatIsCheckout)) {
                hit.collider.GetComponent<Checkout>().CheckoutCustomer();
            }
        }

        // to get stock off of the shelf
        if (Mouse.current.rightButton.wasPressedThisFrame) {
            if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf)) {
                heldPickup = hit.collider.GetComponent<ShelfSpaceController>().GetStock();

                if (heldPickup != null) {
                    heldPickup.transform.SetParent(holdPoint);
                    heldPickup.Pickup();
                }

                return;
            }

            if (Physics.Raycast(ray, out hit, interactionRange, whatIsStockBox)) {
                hit.collider.GetComponent<StockBoxController>().OpenClose();
            }
        }

        // to update the price of a shelf 
        if (Keyboard.current.eKey.wasPressedThisFrame) {
            if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf)) {
                hit.collider.GetComponent<ShelfSpaceController>().StartPriceUpdate();
            }
        }

        // to rotate or move a piece of furniture
        if (Keyboard.current.rKey.wasPressedThisFrame) {
            if (Physics.Raycast(ray, out hit, interactionRange, whatIsFurniture)) {
                heldFurniture = hit.transform.GetComponent<FurnitureController>();

                heldFurniture.transform.SetParent(null, true);

                heldFurniture.MakePlaceable();
                heldFurniture.SetPlacementPosition(heldFurniture.transform.position);

                if (AudioManager.instance != null) {
                    AudioManager.instance.PlaySFX(4);
                }
            }
        }
    }

    /// <summary>
    /// Handles what you can do while holding stock in your hand.
    /// This includes placing it on shelf or throwing it.
    /// </summary>
    private void HandlePickupInHand() {
        Ray ray = theCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        RaycastHit hit;
        UserControlUI.instance.ShowOnlyControls(UserControlUI.instance.stockInHandControls);

        // placing stock onto a shelf
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf)) {

                hit.collider.GetComponent<ShelfSpaceController>().PlaceStock(heldPickup);

                if (heldPickup.isPlaced == true) {
                    heldPickup = null;
                }

                if (AudioManager.instance != null) {
                    AudioManager.instance.PlaySFX(7);
                }
            }
        }

        //if the stock is a card pack, open it.
        if (Keyboard.current.fKey.wasPressedThisFrame) {
            if (heldPickup.info != null && heldPickup.info.cardPack != null) {
                List<CardInfo> pulledCards = CardPackOpener.OpenPack(heldPickup.info.cardPack);

                CardOpeningUI.instance.ShowPackOpening(pulledCards);
                CardInventoryController.instance.AddMultipleCards(pulledCards);
                UserControlUI.instance.ShowOnlyControls(UserControlUI.instance.openingPackControls);

            }
        }

        // toss away whatever is in hand
        if (Mouse.current.rightButton.wasPressedThisFrame) {
            heldPickup.Release();
            heldPickup.theRB.AddForce(theCam.transform.forward * throwForce, ForceMode.Impulse);


            heldPickup.transform.SetParent(null);
            heldPickup = null;

            if (AudioManager.instance != null) {
                AudioManager.instance.PlaySFX(9);
            }
        }
    }

    /// <summary>
    /// Handles when you have a box of stock in your hand.
    /// </summary>
    private void HandleBoxInHand() {
        Ray ray = theCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        RaycastHit hit;
        UserControlUI.instance.ShowOnlyControls(UserControlUI.instance.boxInHandControls);

        //placing stock onto a shelf
        if (Mouse.current.leftButton.isPressed) {
            placeStockCounter -= Time.deltaTime;

            if (placeStockCounter <= 0) {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf)) {
                    heldBox.PlaceStockOnShelf(hit.collider.GetComponent<ShelfSpaceController>());

                    placeStockCounter = waitToPlaceStock;
                }
            }
        }
        
        // throwing a box of stock
        if (Mouse.current.rightButton.wasPressedThisFrame) {

            heldBox.Release();
            heldBox.theRB.AddForce(theCam.transform.forward * throwForce, ForceMode.Impulse);

            heldBox.transform.SetParent(null);
            heldBox = null;

            if (AudioManager.instance != null) {
                AudioManager.instance.PlaySFX(0);
            }
        }

        // open and close the box
        if (Keyboard.current.eKey.wasPressedThisFrame) {
            heldBox.OpenClose();
        }

        // placing whatever is in the box onto a shelf
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            if (heldBox.stockInBox.Count > 0) {

                if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf)) {
                    heldBox.PlaceStockOnShelf(hit.collider.GetComponent<ShelfSpaceController>());

                    placeStockCounter = waitToPlaceStock;

                    if (AudioManager.instance != null) {
                        AudioManager.instance.PlaySFX(7);
                    }
                }
            } else {
                // throwing away the box if it is empty
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsBin)) {
                    Destroy(heldBox.gameObject);

                    heldBox = null;

                    if (AudioManager.instance != null) {
                        AudioManager.instance.PlaySFX(10);
                    }
                }
            }

            
        }
        
    }

    /// <summary>
    /// Handles when there is furniture in your hand.
    /// </summary>
    private void HandleFurnitureInHand() {
        UserControlUI.instance.ShowOnlyControls(UserControlUI.instance.furnitureControls);
        //grid snapping
        Vector3 targetPos = furniturePoint.position;
        heldFurniture.SetPlacementPosition(targetPos);

        // rotating furniture by 30 degrees
        float scroll = Mouse.current.scroll.ReadValue().y;
        if (scroll > 0f) {
            heldFurniture.RotatePlacement(1);
        }
        else if (scroll < 0f) {
            heldFurniture.RotatePlacement(-1);
        }

        //checks if furniture is overlapping with an existing furniture
        bool overlapping = heldFurniture.IsFurnitureOverlapping();

        if (overlapping) {
            heldFurniture.setColorRed();
        } else {
            heldFurniture.setColorGreen();
        }

        // placing the furniture
        if (Mouse.current.leftButton.wasPressedThisFrame ||
            Keyboard.current.rKey.wasPressedThisFrame) {

            if (!overlapping) {
                heldFurniture.transform.SetParent(null);
                heldFurniture.PlaceFurniture();
                heldFurniture = null;
            }

            if (AudioManager.instance != null) {
                AudioManager.instance.PlaySFX(5);
            }
        }
    }
    
    
}