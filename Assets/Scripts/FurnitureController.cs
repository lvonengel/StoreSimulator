using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages each individual piece of furniture.
/// </summary>
public class FurnitureController : MonoBehaviour {
    public GameObject mainObject, placingObject;
    public Collider col;

    public float price;

    public Transform standPoint;

    public List<ShelfSpaceController> shelves;
    [SerializeField] private LayerMask furnitureMask;
    public GameObject placingModel;
    private float rotationStep = 45f;
    private float currentRotationY;
    public float gridSize = .25f;


    private void Start() {
        if (shelves.Count > 0) {
            StoreController.instance.shelvingCases.Add(this);
        }
    }

    /// <summary>
    /// Turns the placing object (highlighted in green when moving) on
    /// </summary>
    public void MakePlaceable() {
        mainObject.SetActive(false);
        placingObject.SetActive(true);
        col.enabled = false;
    }

    /// <summary>
    /// Places the furniture by erasing the duplicated placing object,
    /// and actually placing the active furniture down.
    /// </summary>
    public void PlaceFurniture() {
        
        mainObject.SetActive(true);
        placingObject.SetActive(false);
        col.enabled = true;
    }

    public bool IsFurnitureOverlapping() {
        BoxCollider bc = placingModel.GetComponent<BoxCollider>();
        if (bc == null) return false;

        // BoxCollider size/center are LOCAL, so convert to world
        Vector3 worldCenter = bc.transform.TransformPoint(bc.center);

        // Convert local half-extents to world half-extents (lossyScale)
        Vector3 halfExtents = Vector3.Scale(bc.size * 0.5f, bc.transform.lossyScale) * 0.95f;

        Collider[] hits = Physics.OverlapBox(
            worldCenter,
            halfExtents,
            bc.transform.rotation,
            furnitureMask,
            QueryTriggerInteraction.Collide
        );

        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].transform.IsChildOf(transform)) continue;
            return true;
        }

        return false;
    }



    public void setColorRed() {
        MeshRenderer mr = placingModel.GetComponent<MeshRenderer>();
        mr.material.SetColor("_BaseColor", Color.red);
    }
    public void setColorGreen() {
        MeshRenderer mr = placingModel.GetComponent<MeshRenderer>();
        mr.material.SetColor("_BaseColor", Color.green);
    }

    public void RotatePlacement(int direction) {
        currentRotationY += direction * rotationStep;
        currentRotationY = Mathf.Repeat(currentRotationY, 360f);

        Quaternion rot = Quaternion.Euler(0f, currentRotationY, 0f);

        placingObject.transform.rotation = rot;
        mainObject.transform.rotation = rot;

        col.transform.rotation = rot;
    }


    public Vector3 SnapToGrid(Vector3 worldPos) {
        worldPos.x = Mathf.Round(worldPos.x / gridSize) * gridSize;
        worldPos.z = Mathf.Round(worldPos.z / gridSize) * gridSize;
        worldPos.y = 0f;
        return worldPos;
    }

    public void SetPlacementPosition(Vector3 targetPos) {
        targetPos = SnapToGrid(targetPos);

        // Lock height
        targetPos.y = 0f;
        transform.position = targetPos;

        // Lock rotation to Y only
        Vector3 rot = placingObject.transform.eulerAngles;
        placingObject.transform.rotation = Quaternion.Euler(0f, rot.y, 0f);
    }

}