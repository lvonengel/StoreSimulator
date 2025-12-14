using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the customer details including their states of shopping,
/// move speed, and grabbing an item.
/// </summary>
public class Customer : MonoBehaviour {
    public List<NavPoint> points = new List<NavPoint>();

    public float moveSpeed;
    private float currentWaitTime;

    public Animator anim;

    public enum CustomerState { entering, browsing, queuing, atCheckout, leaving }
    public CustomerState currentState;

    public int maxBrowsePoints = 5;
    private int browsePointsRemain;

    public float browseTime;

    public FurnitureController currentShelfCase;

    public GameObject shoppingBag;
    private bool hasGrabbed;
    public float waitAfterGrabbing = .5f;

    private List<StockObject> stockInBag = new List<StockObject>();

    private Vector3 queuePoint;

    private void Start() {
        points.Clear();
        points.AddRange(CustomerManager.instance.GetEntryPoints());

        if (points.Count > 0) {
            transform.position = points[0].point.position;
            currentWaitTime = points[0].waitTime;
        }

    }

    private void Update() {

        switch (currentState) {
            case CustomerState.entering:
                if (points.Count > 0) {
                    MoveToPoint();
                } else {
                    if (StoreController.instance.shelvingCases.Count > 0) {
                        currentState = CustomerState.browsing;

                        browsePointsRemain = Random.Range(1, maxBrowsePoints + 1);
                        browsePointsRemain = Mathf.Clamp(browsePointsRemain, 1, StoreController.instance.shelvingCases.Count);

                        GetBrowsePoint();
                    } else {
                        StartLeaving();
                    }
                }
                break;

            case CustomerState.browsing:
                MoveToPoint();
                if (points.Count == 0) {
                    if (hasGrabbed == false) {
                        GrabStock();
                    }
                    else {
                        hasGrabbed = false;
                        browsePointsRemain--;
                        if (browsePointsRemain > 0) {
                            GetBrowsePoint();
                        } else {
                            if (stockInBag.Count > 0) {
                                Checkout.instance.AddCustomerToQueue(this);
                                currentState = CustomerState.queuing;
                            } else {
                                StartLeaving();
                            }
                        }
                    }
                }

                break;
                
            case CustomerState.queuing:
                transform.position = Vector3.MoveTowards(transform.position, queuePoint, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, queuePoint) > .1f) {
                    anim.SetBool("isMoving", true);
                } else {
                    anim.SetBool("isMoving", false);
                }
                break;

            case CustomerState.atCheckout:
                break;

            case CustomerState.leaving:
                if (points.Count > 0) {
                    MoveToPoint();
                } else {
                    Destroy(gameObject);
                }
                break;
        }
    }

    /// <summary>
    /// Moves the customer to a certain point. This functions helps move
    /// the customer to/from the store.
    /// </summary>
    public void MoveToPoint() {
        if (points.Count > 0) {

            bool isMoving = true;
            Vector3 targetPosition = new Vector3(points[0].point.position.x, transform.position.y, points[0].point.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            transform.LookAt(targetPosition);

            if (Vector3.Distance(transform.position, targetPosition) < .25f) {
                isMoving = false;
                currentWaitTime -= Time.deltaTime;
                if (currentWaitTime <= 0) {
                    StartNextPoint();
                }
            }

            anim.SetBool("isMoving", isMoving);
        } else {
            StartNextPoint();
        }
    }

    /// <summary>
    /// Helper to update the points that the customer has to go to.
    /// </summary>
    public void StartNextPoint() {
        if (points.Count > 0) {
            points.RemoveAt(0);

            if (points.Count > 0) {
                currentWaitTime = points[0].waitTime;
            }
        }
    }

    /// <summary>
    /// Changes the customer state to leaving and adds the path for the customer to leave.
    /// </summary>
    public void StartLeaving() {
        currentState = CustomerState.leaving;
        points.Clear();
        points.AddRange(CustomerManager.instance.GetExitPoints());
    }

    /// <summary>
    /// Goes to a shelf at random and waits there to act as if browsing.
    /// </summary>
    private void GetBrowsePoint() {
        points.Clear();
        int selectedShelf = Random.Range(0, StoreController.instance.shelvingCases.Count);
        points.Add(new NavPoint());
        points[0].point = StoreController.instance.shelvingCases[selectedShelf].standPoint;
        points[0].waitTime = browseTime * Random.Range(.75f, 1.25f);

        currentWaitTime = points[0].waitTime;
        currentShelfCase = StoreController.instance.shelvingCases[selectedShelf];
    }

    /// <summary>
    /// Gives an item on the shelf to the customer.
    /// </summary>
    public void GrabStock() {
        
        hasGrabbed = true;

        int shelf = Random.Range(0, currentShelfCase.shelves.Count);

        StockObject stock = currentShelfCase.shelves[shelf].GetStock();

        if (stock != null) {
            stock.transform.SetParent(shoppingBag.transform);
            stockInBag.Add(stock);
            stock.PlaceInBag();

            shoppingBag.SetActive(true);

            points.Clear();
            points.Add(new NavPoint());
            points[0].point = currentShelfCase.standPoint;
            points[0].waitTime = waitAfterGrabbing * Random.Range(.75f, 1.25f);
            currentWaitTime = points[0].waitTime;
        }  
    }

    /// <summary>
    /// Updates the customers line in queue. 
    /// </summary>
    public void UpdateQueuePoint(Vector3 newPoint) {
        queuePoint = newPoint;
        transform.LookAt(queuePoint);
    }

    /// <summary>
    /// Gets how much the customer grabbed. This is to see for checkout.
    /// </summary>
    /// <returns>Gets the total amount the customer spent</returns>
    public float GetTotalSpend() {
        float total = 0f;

        foreach (StockObject stock in stockInBag) {
            total += stock.info.currentPrice;
        }

        return total;
    }
}

/// <summary>
/// Manages the customer path points and how long they stay at each point.
/// </summary>
[System.Serializable]
public class NavPoint {
    public Transform point;
    public float waitTime;
}