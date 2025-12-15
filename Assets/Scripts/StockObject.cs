using UnityEngine;

/// <summary>
/// Controls a single stock item.
/// </summary>
public class StockObject : MonoBehaviour {
    public StockInfo info;

    [SerializeField] private float moveSpeed;

    public bool isPlaced;

    public Rigidbody theRB;
    [SerializeField] private Collider col;

    private bool inBag;

    private void Start() {
        info = StockInfoController.instance.GetInfo(info.name);
    }

    private void Update() {
        if (isPlaced == true) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, moveSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, moveSpeed * Time.deltaTime);
        }

        if (inBag == true) {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime);
        }
    }

    public void Pickup() {
        theRB.isKinematic = true;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        isPlaced = false;
        col.enabled = false;
    }

    public void MakePlaced() {
        theRB.isKinematic = true;
        isPlaced = true;
        col.enabled = false;
    }

    public void Release() {
        theRB.isKinematic = false;
        col.enabled = true;
    }

    public void PlaceInBox() {
        theRB.isKinematic = true;
        col.enabled = false;
    }

    public void PlaceInBag() {
        inBag = true;
        MakePlaced();
    }
}