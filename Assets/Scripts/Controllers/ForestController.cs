using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ForestComponent;

public class ForestController : MonoBehaviour {
    private GeneratorController generator;

    [Header("Total Resource Counts")]
    private static float defaultResourceAmount = 100.0f;
    [SerializeField] private float totalWater = defaultResourceAmount;
    [SerializeField] private float totalEnergy = defaultResourceAmount;
    [SerializeField] private float totalOrganic = defaultResourceAmount;

    [Header("Forest Component Collections")]
    private static List<TreeComponent> treeSupply;
    private static List<SunflowerComponent> sunflowerSupply;
    private static List<DecomposerComponent> decomposerSupply;
    private static List<MushroomComponent> mushroomSupply;

    [Header("Timer")]
    private float timer;
    [SerializeField] private float timerResetValue = 5.0f;

    [Header("Resource Tracking")]
    [SerializeField] private List<string> activeSystems;
    [SerializeField] private CounterHUD counterHUD;

    private void Awake() {
        generator = new GeneratorController();
        //counterHUD = new CounterHUD(defaultResourceAmount, defaultResourceAmount, defaultResourceAmount);
        counterHUD.energyCount = counterHUD.waterCount = counterHUD.organicCount = defaultResourceAmount;
        timer = timerResetValue;

        // Initialize Forest Component Collecitons
        treeSupply = new List<TreeComponent>();
        sunflowerSupply = new List<SunflowerComponent>();
        decomposerSupply = new List<DecomposerComponent>();
        mushroomSupply = new List<MushroomComponent>();

        // Set up Resource State Traacker
        activeSystems = new List<string>();
        activeSystems.Add("Water");
        activeSystems.Add("Energy");
        activeSystems.Add("Organic");
    }

    private void Start() {
    }

    private void Update() {
        timer -= Time.fixedDeltaTime;

        if (timer <= 0.0f) {
            ApplyForestResourceGeneration();
            ApplyForestMaintenanceCost();
            ApplyGeneratorConsumptionCost();
            UpdateCounterHUD();

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
        float treeCost = treeSupply.Count > 0 ? treeSupply.Count * treeSupply[0].maintenanceCost : 0;
        float sunflowerCost = sunflowerSupply.Count > 0 ? sunflowerSupply.Count * sunflowerSupply[0].maintenanceCost : 0;
        float decomposerCost = decomposerSupply.Count > 0 ? decomposerSupply.Count * decomposerSupply[0].maintenanceCost : 0;

        if (sunflowerCost > 0 || decomposerCost > 0) {
            HandleWaterResourceConsumption((sunflowerCost + decomposerCost) / 2);
        }

         if (treeCost > 0 || decomposerCost > 0) {
             HandleEnergyResourceConsumption((treeCost + decomposerCost) / 2);
         }

         if (treeCost > 0 || sunflowerCost > 0) {
             HandleOrganicResourceConsumption((treeCost + sunflowerCost) / 2);
         }
    }

    private void ApplyGeneratorConsumptionCost() {
        HandleWaterResourceConsumption(generator.EnergyConsumptionRate);
        HandleEnergyResourceConsumption(generator.WaterConsumptionRate);
        HandleOrganicResourceConsumption(generator.OrganicConsumptionRate);
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
        if (lastTotalValue == 0 && totalWater > 0) {
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
        if (lastTotalValue == 0 && totalEnergy > 0) {
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
        if (lastTotalValue == 0 && totalOrganic > 0) {
            ActivateResourceState("Organic");
        }
    }
    #endregion

    #region Handle Resource Consumption
    private void HandleWaterResourceConsumption(float consumptionAmount) {
        float lastTotalValue = totalWater;
        totalWater = AttemptDecrement(totalWater, consumptionAmount);

        // Check if we were positive, and now hit  zero
        if (lastTotalValue > 0 && totalWater == 0) {
            DeactivateResourceState("Water");
        }
    }

    private void HandleEnergyResourceConsumption(float consumptionAmount) {
        float lastTotalEnergy = totalEnergy;
        totalEnergy = AttemptDecrement(totalEnergy, consumptionAmount);

        // Check if we were positive, and now hit  zero
        if (lastTotalEnergy > 0 && totalEnergy == 0) {
            DeactivateResourceState("Energy");
        }
    }

    private void HandleOrganicResourceConsumption(float consumptionAmount) {
        float lastTotalOrganic = totalOrganic;
        totalOrganic = AttemptDecrement(totalOrganic, consumptionAmount);

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

    private void UpdateCounterHUD() {
        counterHUD.energyCount = totalEnergy;
        counterHUD.waterCount = totalWater;
        counterHUD.organicCount = totalOrganic;
    }
    #endregion

    #region Manage Organic Component Supplies
    public void AddOrganicComponent(GameObject newComponent, ComponentType type) {
        switch (type) {
            case ComponentType.Tree:
                TreeComponent treeComponent = newComponent.AddComponent(typeof(TreeComponent)) as TreeComponent;
                treeSupply.Add(treeComponent);
                break;
            case ComponentType.Sunflower:
                SunflowerComponent sunflowerComponent = newComponent.AddComponent(typeof(SunflowerComponent)) as SunflowerComponent;
                    sunflowerSupply.Add(sunflowerComponent);
                break;
            case ComponentType.Mushroom:
                MushroomComponent mushroomComponent = newComponent.AddComponent(typeof(MushroomComponent)) as MushroomComponent;
                mushroomSupply.Add(mushroomComponent);
                break;
            case ComponentType.Decomposer:
                DecomposerComponent decomposerComponent = newComponent.AddComponent(typeof(DecomposerComponent)) as DecomposerComponent;
                    decomposerSupply.Add(decomposerComponent);
                
                break;
        }
    }
    #endregion
}
