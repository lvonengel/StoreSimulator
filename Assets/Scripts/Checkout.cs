using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Controls the register that is used by the player
/// to check customers out.
/// </summary>
public class Checkout : MonoBehaviour {
    public static Checkout instance;
    
    public TMP_Text priceText;
    public GameObject checkoutScreen;

    public Transform queuePoint;

    public List<Customer> customersInQueue = new List<Customer>();

    private void Awake() {
        instance = this;
    }


    private void Start() {
        HidePrice();
    }


    private void Update(){
        if (customersInQueue.Count > 0 && checkoutScreen.activeSelf == false) {
            if (Vector3.Distance(customersInQueue[0].transform.position, queuePoint.position) < .1f) {
                ShowPrice(customersInQueue[0].GetTotalSpend());
            }
        }
    }

    public void ShowPrice(float priceTotal) {
        checkoutScreen.SetActive(true);
        priceText.text = "$" + priceTotal.ToString("F2");
    }

    public void HidePrice() {
        checkoutScreen.SetActive(false);
    }

    /// <summary>
    /// Checks out the customer and updates the line queue
    /// </summary>
    public void CheckoutCustomer() {
        if (checkoutScreen.activeSelf == true && customersInQueue.Count > 0) {
            HidePrice();
            StoreController.instance.AddMoney(customersInQueue[0].GetTotalSpend());
            customersInQueue[0].StartLeaving();
            customersInQueue.RemoveAt(0);
            UpdateQueue();

            if (AudioManager.instance != null) {
                AudioManager.instance.PlaySFX(3);
            }

        }
    }

    public void AddCustomerToQueue(Customer newCust) {
        customersInQueue.Add(newCust);
        UpdateQueue();
    }

    public void UpdateQueue() {
        for (int i = 0; i < customersInQueue.Count; i++) {
            customersInQueue[i].UpdateQueuePoint(queuePoint.position + (queuePoint.forward * i * .5f));
        }
    }
}