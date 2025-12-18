using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages how often and where the customer spawns.
/// </summary>
public class CustomerManager : MonoBehaviour {
    public static CustomerManager instance {get; private set;}

    [SerializeField] private List<Customer> customersToSpawn = new List<Customer>();

    [SerializeField] private float timeBetweenCustomers;
    private float spawnCounter;

    [SerializeField] private List<NavPoint> entryPointsLeft, entryPointsRight;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        SpawnCustomer();
    }

    private void Update() {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0) {
            SpawnCustomer();
        }
    }

    public void SetTimeBetweenCustomers(int time) {
        timeBetweenCustomers = time;
    }

    public void SpawnCustomer() {
        Instantiate(customersToSpawn[Random.Range(0, 16)]);

        spawnCounter = timeBetweenCustomers * Random.Range(.75f, 1.25f);
    }

    /// <summary>
    /// Gets the points where the customer spawns in.
    /// 50% chance for the left or right side.
    /// </summary>
    /// <returns>The points that the customer walks to</returns>
    public List<NavPoint> GetEntryPoints() {
        List<NavPoint> points = new List<NavPoint>();

        if (Random.value < .5f) {
            points.AddRange(entryPointsLeft);
        } else {
            points.AddRange(entryPointsRight);
        }

        return points;
    }

    /// <summary>
    /// Gets the points where the customer leaves.
    /// 50% chance for the left or right side.
    /// </summary>
    /// <returns>The points that the customer walks to</returns>
    public List<NavPoint> GetExitPoints() {
        List<NavPoint> points = new List<NavPoint>();

        List<NavPoint> temp = new List<NavPoint>();

        if (Random.value < .5f) {
            temp.AddRange(entryPointsLeft);
        } else {
            temp.AddRange(entryPointsRight);
        }

        for (int i = temp.Count - 1; i >= 0; i--) {
            points.Add(temp[i]);
        }

        return points;
    }
}