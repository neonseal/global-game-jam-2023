using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestController : MonoBehaviour {

    [Header("Total Resource Counts")]
    [SerializeField] private float totalWater = 100.0f;
    [SerializeField] private float totalEnergy = 100.0f;
    [SerializeField] private float totalOrganic = 100.0f;

    [Header("Forest Component Collections")]
    private TreeComponent[] treeSupply;
    private SunflowerComponent[] sunflowerSupply;
    private DecomposerComponent[] decomposerSupply;
    private MushroomComponent[] mushroomSupply;

    [Header("Timer")]
    private float timer;
    [SerializeField] private float timerResetValue = 5.0f;

    [Header("Failing Resources")]
    [SerializeField] private List<string> activeSystems;

    private void Awake() {
        timer = timerResetValue;

        // Initialize Forest Component Collecitons
        treeSupply = new TreeComponent[0];
        sunflowerSupply = new SunflowerComponent[0];
        decomposerSupply = new DecomposerComponent[0];
        mushroomSupply = new MushroomComponent[0];

        // Set up Resource State Traacker
        activeSystems = new List<string>();
        activeSystems.Add("Water");
        activeSystems.Add("Energy");
        activeSystems.Add("Organic");
    }

    private void Update() {
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
        HandleWaterResourceGeneration();
        HandleEneryResourceGeneration();
        HandleOrganicResourceGeneration();
    }

    private void ApplyForestMaintenanceCost() {
        // Calculate total cost to maintain forest
        float treeCost = treeSupply.Length > 0 ? treeSupply.Length * treeSupply[0].maintenanceCost : 0;
        float sunflowerCost = sunflowerSupply.Length > 0 ? sunflowerSupply.Length * sunflowerSupply[0].maintenanceCost : 0;
        float decomposerCost = decomposerSupply.Length > 0 ? decomposerSupply.Length * decomposerSupply[0].maintenanceCost : 0;

        HandleWaterResourceConsumption(sunflowerCost, decomposerCost);
        HandleEnergyResourceConsumption(treeCost, decomposerCost);
        HandleOrganicResourceConsumption(treeCost, sunflowerCost);
    }

    #region Handle Resource Generation
    private void HandleWaterResourceGeneration() {
        // Capture current value to check for state change
        float lastTotalValue = totalWater;
        // Calculate total resource resource generation power
        float waterGenerationRate = 0.0f;

        foreach (TreeComponent tree in treeSupply) {
            waterGenerationRate += tree.GetCurrentGenerationRate();
        }

        totalWater += waterGenerationRate;

        // If we have most into the positive, remove resource from the failing states
        if (lastTotalValue == 0) {
            ActivateResourceState("Water");
        }
    }
    private void HandleEneryResourceGeneration() {
        // Capture current value to check for state change
        float lastTotalValue = totalEnergy;
        // Calculate total resource resource generation power
        float energyGenerationRate = 0.0f;

        foreach (SunflowerComponent sunflower in sunflowerSupply) {
            energyGenerationRate += sunflower.GetCurrentGenerationRate();
        }

        totalEnergy += energyGenerationRate;

        // If we have most into the positive, remove resource from the failing states
        if (lastTotalValue == 0) {
            ActivateResourceState("Energy");
        }
    }
    private void HandleOrganicResourceGeneration() {
        // Capture current value to check for state change
        float lastTotalValue = totalOrganic;
        // Calculate total resource generation power
        float organicGenerationRate = 0.0f;

        foreach (DecomposerComponent decomposer in decomposerSupply) {
            organicGenerationRate += decomposer.GetCurrentGenerationRate();
        }

        totalOrganic += organicGenerationRate;

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

    private float AttemptDecrement(float target, float decrement) {
        if (target - decrement < 0) {
            return 0;
        }

        return target -= decrement;
    }
    #endregion

    #region Resource State Manager
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

    private void TriggerGeneratorKillMode() {
        Debug.Log("GENERATOR IS EATING!");
    }
    #endregion
}
