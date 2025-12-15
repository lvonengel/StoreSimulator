
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the money system in the game.
/// </summary>
public class StoreController : MonoBehaviour {
    public static StoreController instance;

    [SerializeField] private float currentMoney = 1000f;
    private int storeLevel = 1;

    public Transform stockSpawnPoint, furnitureSpawnPoint;

    public List<FurnitureController> shelvingCases = new List<FurnitureController>();

    private void Awake() {
        instance = this;
    }

    private void Start() {
        UIController.instance.UpdateMoney(currentMoney);
        UIController.instance.UpdateStoreLevel(storeLevel);
        // AudioManager.instance.StartBGM();
    }

    private void Update() {
        if (Keyboard.current.iKey.wasPressedThisFrame) {
            AddMoney(100f);
        }

        if (Keyboard.current.oKey.wasPressedThisFrame) {
            if (CheckMoneyAvailable(250f)) {
                SpendMoney(250f);
            }
        }
    }

    public void AddMoney(float amountToAdd) {
        currentMoney += amountToAdd;
        UIController.instance.UpdateMoney(currentMoney);
    }

    public void SpendMoney(float amountToSpend) {
        currentMoney -= amountToSpend;

        if (currentMoney < 0) {
            currentMoney = 0;
        }

        UIController.instance.UpdateMoney(currentMoney);
    }

    public bool CheckMoneyAvailable(float amountToCheck) {
        bool hasEnough = false;

        if(currentMoney >= amountToCheck) {
            hasEnough = true;
        }

        return hasEnough;
    }

    public int GetStoreLevel() {
        return storeLevel;
    }
    
}