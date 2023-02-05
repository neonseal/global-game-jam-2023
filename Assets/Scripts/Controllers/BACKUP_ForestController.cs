using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ForestComponent;

public class Backup_ForestController : MonoBehaviour {
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
    private float treeCost, sunflowerCost, decomposerCost = 0.0f;

    [Header("Timer")]
    private float timer;
    [SerializeField] private float timerResetValue = 5.0f;

    [Header("Resource Tracking")]
    [SerializeField] private List<string> activeSystems;
    [SerializeField] private CounterHUD counterHUD;

    private void Awake() {
        generator = new GeneratorController();
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

    private void Update() {
        timer -= Time.fixedDeltaTime;

        UpdateForestMaintenanceCosts();

        if (timer <= 0.0f) {
            UpdateWaterResourceSupply();
            UpdateEnergyResourceSupply();
            UpdateOrganicResourceSupply();
            UpdateCounterHUD();

            timer = timerResetValue;
        }
    }

    private void UpdateForestMaintenanceCosts() {
        // Calculate total cost to maintain forest
        treeCost = treeSupply.Count > 0 ? treeSupply.Count * treeSupply[0].maintenanceCost : 0;
        sunflowerCost = sunflowerSupply.Count > 0 ? sunflowerSupply.Count * sunflowerSupply[0].maintenanceCost : 0;
        decomposerCost = decomposerSupply.Count > 0 ? decomposerSupply.Count * decomposerSupply[0].maintenanceCost : 0;
    }

    private void UpdateWaterResourceSupply() {
        // Capture current value to check for state change
        float lastTotalValue = totalWater;

        // Apply Resource Generation
        float waterGenerationRate = 0.0f;

        // Calculate total resource resource generation power
        foreach (TreeComponent tree in treeSupply) {
            waterGenerationRate += tree.GetCurrentGenerationRate();
        }
        totalWater += waterGenerationRate;

        // Apply Forest Maintenance Costs
        if (sunflowerCost > 0 || decomposerCost > 0) {
            totalWater = AttemptDecrement(totalWater, (sunflowerCost + decomposerCost) / 2);
        }

        // Apply Generation Consumption
        totalWater = AttemptDecrement(totalWater, generator.WaterConsumptionRate);

        // Check for Generator State Change
        if (lastTotalValue > 0 && totalWater <= 0) {
            // Water now depleted
            generator.UpdateResourceState(ComponentType.Tree, false);
        } else if (lastTotalValue <= 0 && totalWater > 0) {
            // Water Replenished
            generator.UpdateResourceState(ComponentType.Tree, true);
        }
    }

    private void UpdateEnergyResourceSupply() {
        // Capture current value to check for state change
        float lastTotalValue = totalEnergy;

        // Apply Resource Generation
        float energyGenerationRate = 0.0f;

        // Calculate total resource resource generation power
        foreach (SunflowerComponent sunflower in sunflowerSupply) {
            energyGenerationRate += sunflower.GetCurrentGenerationRate();
        }
        totalEnergy += energyGenerationRate;

        // Apply Forest Maintenance Costs
        if (treeCost > 0 || decomposerCost > 0) {
            totalEnergy = AttemptDecrement(totalEnergy, (treeCost + decomposerCost) / 2);
        }

        // Apply Generation Consumption
        totalEnergy = AttemptDecrement(totalEnergy, generator.EnergyConsumptionRate);

        // Check for Generator State Change
        if (lastTotalValue > 0 && totalEnergy <= 0) {
            // Energy now depleted
            generator.UpdateResourceState(ComponentType.Sunflower, false);
        } else if (lastTotalValue <= 0 && totalEnergy > 0) {
            // Energy Replenished
            generator.UpdateResourceState(ComponentType.Sunflower, true);
        }
    }

    private void UpdateOrganicResourceSupply() {
        // Capture current value to check for state change
        float lastTotalValue = totalOrganic;

        // Apply Resource Generation
        float organicGenerationRate = 0.0f;

        // Calculate total resource resource generation power
        foreach (DecomposerComponent decomposer in decomposerSupply) {
            organicGenerationRate += decomposer.GetCurrentGenerationRate();
        }
        totalOrganic += organicGenerationRate;

        // Apply Forest Maintenance Costs
        if (treeCost > 0 || sunflowerCost > 0) {
            totalOrganic = AttemptDecrement(totalOrganic, (treeCost + sunflowerCost) / 2);
        }

        // Apply Generation Consumption
        totalOrganic = AttemptDecrement(totalOrganic, generator.OrganicConsumptionRate);

        // Check for Generator State Change
        if (lastTotalValue > 0 && totalOrganic <= 0) {
            // Energy now depleted
            generator.UpdateResourceState(ComponentType.Decomposer, false);
        } else if (lastTotalValue <= 0 && totalOrganic > 0) {
            // Energy Replenished
            generator.UpdateResourceState(ComponentType.Decomposer, true);
        }
    }

    private float AttemptDecrement(float target, float decrement) {
        if (target - decrement < 0) {
            return 0;
        }

        return target -= decrement;
    }

    #region Resource State Manager

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
                Debug.Log(treeSupply.Count);
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