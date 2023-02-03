using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestController : MonoBehaviour {
    [Header("Component Counts")]
    [SerializeField] private int treeCount = 2;
    [SerializeField] private int sunflowerCount = 1;
    [SerializeField] private int decomposerCount = 1;
    [SerializeField] private int mushroomCount = 7;

    [Header("Forest Component Collections")]
    private TreeComponent[] treeSupply;

    [Header("Total Resource Counts")]
    [SerializeField] private float totalWater = 100.0f;
    [SerializeField] private float totalEnergy = 100.0f;
    [SerializeField] private float totalOrganic = 100.0f;

    [Header("Timer")]
    private float timer;
    [SerializeField] private float timerResetValue = 5.0f;

    [Header("Failing Resources")]
    [SerializeField] private List<string> activeSystems;

    private void Awake() {
        timer = timerResetValue;

        // Initialize Forest Component Collecitons
        treeSupply = new TreeComponent[0];

        // Set up Resource State Traacker
        activeSystems = new List<string>();
        activeSystems.Add("Water");
        activeSystems.Add("Energy");
        activeSystems.Add("Organic");
    }

    private void Update() {
        // Update current resource generation 

        timer -= Time.fixedDeltaTime;

        if (timer <= 0.0f) {
            ApplyForestResourceGeneration();
            ApplyForestMaintenanceCost();

            if (totalWater == 0 && totalEnergy == 0 && totalOrganic == 0) {
                TriggerGeneratorKillMode();
            }

            timer = timerResetValue;
        }
    }

    private void ApplyForestResourceGeneration() {
        // Capture current totalValue;
        // Iterate through each collection of organic components
        // Call GetCurrentGenerationRate function
        // Total generation rate and add to total
        // If previous total was zero, trigger generator state change 

        HandleWaterResourceGeneration();
        HandleEneryResourceGeneration();
        HandleOrganicResourceGeneration();
    }

    private void ApplyForestMaintenanceCost() {
        // Iterate through each collection of organic components
        // Call CalculateMaintenanceCost function 
        // Calculate total cost to maintain forest
        // Substract from each total
        // Check if each total == 0, trigger generator state change
        float treeCost = 3 * treeCount;
        float sunflowerCost = 2 * sunflowerCount;
        float decomposerCost = 1 * decomposerCount;

        HandleWaterResourceConsumption(sunflowerCost, decomposerCost);
        HandleEnergyResourceConsumption(treeCost, decomposerCost);
        HandleOrganicResourceConsumption(treeCost, sunflowerCost);
    }

    #region Handle Resource Generation
    private void HandleWaterResourceGeneration() {
        // Capture current value to check for state change
        float lastTotalValue = totalWater;
        // Calculate total resource resource generation power
        totalWater += 3 * treeCount;

        // If we have most into the positive, remove resource from the failing states
        if (lastTotalValue == 0) {
            ActivateResourceState("Water");
        }
    }
    private void HandleEneryResourceGeneration() {
        // Capture current value to check for state change
        float lastTotalValue = totalWater;
        // Calculate total resource resource generation power
        totalWater += 2 * sunflowerCount;

        // If we have most into the positive, remove resource from the failing states
        if (lastTotalValue == 0) {
            ActivateResourceState("Energy");
        }
    }
    private void HandleOrganicResourceGeneration() {
        // Capture current value to check for state change
        float lastTotalValue = totalWater;
        // Calculate total resource generation power
        totalWater += decomposerCount;

        // If we have most into the positive, remove resource from the failing states
        if (lastTotalValue == 0) {
            ActivateResourceState("Organic");
        }
    }
    #endregion

    #region Handle Resource Consumption
    private void HandleWaterResourceConsumption(float sunflowerCost, float decomposerCost) {
        float lastTotalValue = totalWater;
        totalWater = AttemptDecrement(totalWater, (sunflowerCost + decomposerCost) / 2);

        // Check if we were positive, and now hit  zero
        if (lastTotalValue > 0 && totalWater == 0) {
            DeactivateResourceState("Water");
        }
    }

    private void HandleEnergyResourceConsumption(float treeCost, float decomposerCost) {
        float lastTotalEnergy = totalEnergy;
        totalEnergy = AttemptDecrement(totalEnergy, (treeCost + decomposerCost) / 2);

        // Check if we were positive, and now hit  zero
        if (lastTotalEnergy > 0 && totalEnergy == 0) {
            DeactivateResourceState("Energy");
        }
    }

    private void HandleOrganicResourceConsumption(float treeCost, float sunflowerCost) {
        float lastTotalOrganic = totalOrganic;
        totalOrganic = AttemptDecrement(totalOrganic, (treeCost + sunflowerCost) / 2);

        // Check if we were positive, and now hit  zero
        if (lastTotalOrganic > 0 && totalOrganic == 0) {
            DeactivateResourceState("Organic");
        }
    }
    #endregion

    #region Activate/Deactivate Resource States
    private void ActivateResourceState(string resource) {
        if (!activeSystems.Contains(resource)) {
            activeSystems.Add(resource);
        }
        // Trigger Generator To Increase State
    }

    private void DeactivateResourceState(string resource) {
        if (activeSystems.Contains(resource)) {
            activeSystems.Remove(resource);
        }
        // Trigger Generator To Decrease State
    }
    #endregion



    private float AttemptDecrement(float target, float decrement) {
        if (target - decrement < 0) {
            return 0;
        }

        return target -= decrement;
    }

    private void TriggerGeneratorKillMode() {
        Debug.Log("GENERATOR IS EATING!");
    }
}
