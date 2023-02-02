using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour {
    [Header("Resource Variables")]
    [SerializeField] public float currentResourcePool = 1000.0f;
    [SerializeField] private float maxPoolValue = 2000.0f;
    [SerializeField] private float waterConsumptionRate = 10.0f;
    [SerializeField] private float energyConsumptionRate = 10.0f;
    [SerializeField] private float organicConsumptionRate = 10.0f;
    // Use global resource manager to track resource generation/input
    private ForestManager forestManager;


    private void Awake() {
        forestManager = new ForestManager();
    }

    private void Update() {
        if (currentResourcePool <= 0) {
            Debug.Log("")
        }

    }
}