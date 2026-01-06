using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Manages the customer details including their states of shopping,
/// move speed, and grabbing an item.
/// </summary>
public class Customer : MonoBehaviour {
    [SerializeField] private List<NavPoint> points = new List<NavPoint>();

    [SerializeField] private float moveSpeed;
    private float currentWaitTime;

    [SerializeField] private Animator anim;

    public enum CustomerState { entering, browsing, queuing, atCheckout, leaving }
    [SerializeField] private CustomerState currentState;

    [SerializeField] private int maxBrowsePoints = 5;
    private int browsePointsRemain;

    [SerializeField] private float browseTime;

    [SerializeField] private FurnitureController currentShelfCase;

    [SerializeField] private GameObject shoppingBag;
    private bool hasGrabbed;
    [SerializeField] private float waitAfterGrabbing = .5f;
    [SerializeField] private TMP_Text talkText;

    private List<StockObject> stockInBag = new List<StockObject>();
    private float talkTextDuration = 3f;

    private Vector3 queuePoint;
    private NavMeshAgent agent;
    private Vector3 lastDestination;


    string[] complaints = {
        "This is way too expensive!",
        "No way I'm paying that.",
        "Are they serious with this price?"
    };

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    private void Start() {
        talkText.gameObject.SetActive(false);
        points.Clear();
        points.AddRange(CustomerManager.instance.GetEntryPoints());

        if (points.Count > 0) {
            Vector3 spawnPos = points[0].point.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPos, out hit, 2f, NavMesh.AllAreas)) {
                agent.Warp(hit.position);
            } else {
                Debug.LogError("Customer spawned off NavMesh");
            }

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
                agent.SetDestination(queuePoint);
                anim.SetBool("isMoving", agent.velocity.magnitude > 0.1f);

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
    /// Moves the customer to a certain point.
    /// </summary>
    public void MoveToPoint() {
        if (points.Count == 0) {
            StartNextPoint();
            return;
        }

        Vector3 target = points[0].point.position;

        if (!agent.pathPending && lastDestination != target) {
            agent.SetDestination(target);
            lastDestination = target;
        }

        if (!agent.pathPending && agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathInvalid) {
            StartNextPoint();
            return;
        }

        anim.SetBool("isMoving", agent.velocity.sqrMagnitude > 0.01f);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
            currentWaitTime -= Time.deltaTime;
            if (currentWaitTime <= 0f) {
                StartNextPoint();
            }
        }
    }


    private bool CanReach(Vector3 target) {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target, path);
        return path.status == NavMeshPathStatus.PathComplete;
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

        List<FurnitureController> shelves = StoreController.instance.shelvingCases;

        if (shelves.Count == 0) {
            StartLeaving();
            return;
        }

        // try few times to find a reachable shelf
        int attempts = Mathf.Min(5, shelves.Count);

        for (int i = 0; i < attempts; i++) {
            int index = Random.Range(0, shelves.Count);
            Transform standPoint = shelves[index].standPoint;

            if (CanReach(standPoint.position)) {
                points.Add(new NavPoint {
                    point = standPoint,
                    waitTime = browseTime * Random.Range(.75f, 1.25f)
                });

                currentWaitTime = points[0].waitTime;
                currentShelfCase = shelves[index];
                return;
            }
        }

        // if no shelves reachable
        browsePointsRemain--;

        if (browsePointsRemain > 0) {
            GetBrowsePoint(); // try again
        } else {
            StartLeaving();
        }
    }


    /// <summary>
    /// Gives an item on the shelf to the customer.
    /// </summary>
    public void GrabStock() {
        
        hasGrabbed = true;

        int shelf = Random.Range(0, currentShelfCase.shelves.Count);
        StockObject stock = currentShelfCase.shelves[shelf].GetStock();

        if (stock != null) {

            float basePrice = stock.info.price;
            float currentPrice = stock.info.currentPrice;

            if (currentPrice > basePrice * 3f) {
                
                ShowMessage(complaints[Random.Range(0, complaints.Length)]);

                points.Clear();
                points.Add(new NavPoint());
                points[0].point = currentShelfCase.standPoint;
                points[0].waitTime = waitAfterGrabbing * Random.Range(.75f, 1.25f);
                currentWaitTime = points[0].waitTime;

                return;
            }

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

    private void ShowMessage(string message) { 
        StopAllCoroutines(); 
        StartCoroutine(ShowMessageCo(message)); 
    } 
    
    private IEnumerator ShowMessageCo(string message) { 
        talkText.text = message;
        talkText.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(talkTextDuration); 
        talkText.gameObject.SetActive(false); }

}

/// <summary>
/// Manages the customer path points and how long they stay at each point.
/// </summary>
[System.Serializable]
public class NavPoint {
    public Transform point;
    public float waitTime;
}