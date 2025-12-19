
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the money and store level system in the game.
/// </summary>
public class StoreController : MonoBehaviour {
    public static StoreController instance {get; private set;}

    [SerializeField] private float currentMoney = 1000f;
    public event EventHandler<OnExperienceChangedEventArgs> OnExperienceChanged;
    public class OnExperienceChangedEventArgs : EventArgs {
        public float experienceNormalized;
    }
    
    public event Action<int> OnStoreLevelChanged;
    private int storeLevel = 1;
    private int currentExperience = 0;

    public Transform stockSpawnPoint, furnitureSpawnPoint;

    public List<FurnitureController> shelvingCases = new List<FurnitureController>();

    private void Awake() {
        instance = this;
    }

    private void Start() {
        StoreStatsUI.instance.UpdateMoney(currentMoney);
        StoreStatsUI.instance.UpdateStoreLevel(storeLevel);
        // AudioManager.instance.StartBGM();


    }

    private void Update() {
        if (Keyboard.current.iKey.wasPressedThisFrame) {
            AddMoney(2000f);
        }

        if (Keyboard.current.oKey.wasPressedThisFrame) {
            if (CheckMoneyAvailable(250f)) {
                SpendMoney(250f);
            }
        }
    }

    public void AddMoney(float amountToAdd) {
        currentMoney += amountToAdd;
        DayStatsController.instance.RegisterMoneyMade(amountToAdd);
        StoreStatsUI.instance.UpdateMoney(currentMoney);
    }

    public void SpendMoney(float amountToSpend) {
        currentMoney -= amountToSpend;

        if (currentMoney < 0) {
            currentMoney = 0;
        }
        DayStatsController.instance.RegisterMoneySpent(amountToSpend);
        StoreStatsUI.instance.UpdateMoney(currentMoney);
    }

    public bool CheckMoneyAvailable(float amountToCheck) {
        bool hasEnough = false;

        if(currentMoney >= amountToCheck) {
            hasEnough = true;
        }

        return hasEnough;
    }

    private int GetExperienceRequiredForNextLevel() {
        return 30 + (storeLevel - 1) * 20;
    }

    public void AddExperience(int amount) {
        currentExperience += amount;
        int expToNext = GetExperienceRequiredForNextLevel();

        while (currentExperience >= expToNext) {
            currentExperience -= expToNext;
            storeLevel++;

            StoreStatsUI.instance.UpdateStoreLevel(storeLevel);
            OnStoreLevelChanged?.Invoke(storeLevel);

            expToNext = GetExperienceRequiredForNextLevel();
        }

        float normalized = (float)currentExperience / expToNext;

        OnExperienceChanged?.Invoke(this, new OnExperienceChangedEventArgs {
            experienceNormalized = normalized
        });
    }


    public int GetStoreLevel() {
        return storeLevel;
    }
    
}