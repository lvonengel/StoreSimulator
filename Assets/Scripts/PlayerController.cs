using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the player movement and interactions.
/// </summary>
public class PlayerController : MonoBehaviour {

    // Movement
    public InputActionReference moveAction;

    public CharacterController charCon;

    public float moveSpeed;

    private float ySpeed;

    public InputActionReference jumpAction;
    public float jumpForce;

    public InputActionReference lookAction;
    private float horiRot, vertRot;
    public float lookSpeed;
    public Camera theCam;
    public float minLookAngle, maxLookAngle;

    public float interactionRange;

    public float throwForce;

    // Layers
    public LayerMask whatIsStock;

    public LayerMask whatIsShelf;

    public LayerMask whatIsStockBox;
    public LayerMask whatIsBin;

    public LayerMask whatIsFurniture;
    public LayerMask whatIsCheckout;

    // Hold points and stock
    private StockObject heldPickup;
    public Transform holdPoint;
    public StockBoxController heldBox;
    public Transform boxHoldPoint;

    public float waitToPlaceStock;
    private float placeStockCounter;


    public Transform furniturePoint;
    public FurnitureController heldFurniture;

    

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if (UIController.instance.updatePricePanel != null) {
            if (UIController.instance.updatePricePanel.activeSelf == true) {
                return;
            }
        }

        if (UIController.instance.buyMenuScreen != null) {
            if (UIController.instance.buyMenuScreen.activeSelf == true) {
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


        //checks for pickup
        Ray ray = theCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        RaycastHit hit;

        if (heldPickup == null && heldBox == null && heldFurniture == null) {
            if (Mouse.current.leftButton.wasPressedThisFrame) {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsStock)) {

                    heldPickup = hit.collider.GetComponent<StockObject>();
                    heldPickup.transform.SetParent(holdPoint);
                    heldPickup.Pickup();

                    if (AudioManager.instance != null) {
                        AudioManager.instance.PlaySFX(6);
                    }

                    return;
                }

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

                if (Physics.Raycast(ray, out hit, interactionRange, whatIsCheckout)) {
                    hit.collider.GetComponent<Checkout>().CheckoutCustomer();
                }
            }

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

            if (Keyboard.current.eKey.wasPressedThisFrame) {
                if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf)) {
                    hit.collider.GetComponent<ShelfSpaceController>().StartPriceUpdate();
                }
            }

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
        } else {
            if (heldPickup != null) {
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

            if (heldBox != null) {
                if (Mouse.current.rightButton.wasPressedThisFrame) {

                    heldBox.Release();
                    heldBox.theRB.AddForce(theCam.transform.forward * throwForce, ForceMode.Impulse);

                    heldBox.transform.SetParent(null);
                    heldBox = null;

                    if (AudioManager.instance != null) {
                        AudioManager.instance.PlaySFX(0);
                    }
                }

                if (Keyboard.current.eKey.wasPressedThisFrame) {
                    heldBox.OpenClose();
                }

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
                        if (Physics.Raycast(ray, out hit, interactionRange, whatIsBin)) {
                            Destroy(heldBox.gameObject);

                            heldBox = null;

                            if (AudioManager.instance != null) {
                                AudioManager.instance.PlaySFX(10);
                            }
                        }
                    }

                    
                }

                if (Mouse.current.leftButton.isPressed) {
                    placeStockCounter -= Time.deltaTime;

                    if (placeStockCounter <= 0) {
                        if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf)) {
                            heldBox.PlaceStockOnShelf(hit.collider.GetComponent<ShelfSpaceController>());

                            placeStockCounter = waitToPlaceStock;
                        }
                    }
                }
            }

            if (heldFurniture != null) {

                // --- GRID-SNAPPED POSITION (Y LOCKED INSIDE) ---
                Vector3 targetPos = furniturePoint.position;
                heldFurniture.SetPlacementPosition(targetPos);

                // --- ROTATION (30° steps via scroll) ---
                float scroll = Mouse.current.scroll.ReadValue().y;
                if (scroll > 0f) {
                    heldFurniture.RotatePlacement(1);
                }
                else if (scroll < 0f) {
                    heldFurniture.RotatePlacement(-1);
                }

                // --- OVERLAP CHECK ---
                bool overlapping = heldFurniture.IsFurnitureOverlapping();

                if (overlapping) {
                    heldFurniture.setColorRed();
                } else {
                    heldFurniture.setColorGreen();
                }

                // --- PLACE ---
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
    }
    
    
}