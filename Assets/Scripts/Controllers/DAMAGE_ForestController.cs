using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ForestComponent;

public class ForestController : MonoBehaviour {
    private GeneratorController generator;

    [Header("Total Resource Counts")]
    private static float defaultResourceAmount = 100.0f;
    [SerializeField] private CounterHUD counterHUD;
    [SerializeField] private float totalWater = defaultResourceAmount;
    [SerializeField] private float totalEnergy = defaultResourceAmount;
    [SerializeField] private float totalOrganic = defaultResourceAmount;

    [Header("Forest Component Collections")]
    private static List<TreeComponent> treeSupply;
    private static List<SunflowerComponent> sunflowerSupply;
    private static List<DecomposerComponent> decomposerSupply;
    private static List<MushroomComponent> mushroomSupply;
    private float treeCost, sunflowerCost, decomposerCost = 0.0f;

    [SerializeField] private float totalCost = 0.0f;
    [SerializeField] private int amountOfSunflowers = 0;
    [SerializeField] private int amountOfTree = 0;
    [SerializeField] private int amountOfDecomposters = 0;
    

    [Header("Timer")]
    private float timer;
    [SerializeField] private float damage_timer;
    [SerializeField] private float timerResetValue = 5.0f;

    [Header("Tree Systems")]
    [SerializeField] private float treeMaintenanceCost;
    [SerializeField] private float treeEnergyBuildCost;
    [SerializeField] private float treeOrganicBuildCost;
    [SerializeField] private float treeResourceProduction;


    [Header("Sunflower Systems")]
    [SerializeField] private float sunflowerMaintenanceCost;
    [SerializeField] private float sunflowerWaterBuildCost;
    [SerializeField] private float sunflowerOrganicBuildCost;
    [SerializeField] private float sunflowerResourceProduction;


    [Header("Decomposer Systems")]
    [SerializeField] private float decomposerMaintenanceCost;
    [SerializeField] private float decomposerEnergyBuildCost;
    [SerializeField] private float decomposerWaterBuildCost;
    [SerializeField] private float decomposerResourceProduction;


    [Header("Mushroom Systems")]
    [SerializeField] private float mushroomMaintenanceCost;
    [SerializeField] private float mushroomEnergyBuildCost;
    [SerializeField] private float mushroomWaterBuildCost;
    [SerializeField] private float mushroomrOrganicBuildCost;
    [SerializeField] private float mushroomResourceProduction;


    [Header("Generator Systems")]
    [SerializeField] private float testEnergyConsumptionRate;
    [SerializeField] private float testWaterConsumptionRate;
    [SerializeField] private float testOrganicConsumptionRate;


    private void Awake() {
        generator = new GeneratorController();
        counterHUD.energyCount = counterHUD.waterCount = counterHUD.organicCount = defaultResourceAmount;
        timer = timerResetValue;
        damage_timer = timerResetValue;

        // Initialize Forest Component Collecitons
        treeSupply = new List<TreeComponent>();
        sunflowerSupply = new List<SunflowerComponent>();
        decomposerSupply = new List<DecomposerComponent>();
        mushroomSupply = new List<MushroomComponent>();

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
        treeCost = treeSupply.Count > 0 ? treeSupply.Count * treeMaintenanceCost : 0;
            //treeSupply[0].maintenanceCost : 0;
        sunflowerCost = sunflowerSupply.Count > 0 ? sunflowerSupply.Count * sunflowerMaintenanceCost : 0;
        //sunflowerSupply[0].maintenanceCost : 0;
        decomposerCost = decomposerSupply.Count > 0 ? decomposerSupply.Count * decomposerMaintenanceCost : 0; //decomposerSupply[0].maintenanceCost : 0;
    }

    private void UpdateWaterResourceSupply() {
        // Capture current value to check for state change
        float lastTotalValue = totalWater;

        // Apply Resource Generation
        float waterGenerationRate = 0.0f;

        // Calculate total resource resource generation power
        foreach (TreeComponent tree in treeSupply) {
            amountOfTree += 1;
            waterGenerationRate += treeResourceProduction;
        }
        totalWater += waterGenerationRate * mushroomSupply.Count;
        
        // Apply Forest Maintenance Costs
        if (sunflowerCost > 0 || decomposerCost > 0) {
            if( totalWater < (sunflowerCost + decomposerCost) / 2  )
            {
                // Goes through list of Trees to apply damage globally
                foreach (TreeComponent tree in treeSupply) {
                    difference = totalWater - (sunflowerCost + decomposerCost) / 2;

                    // global applied damage = 
                    // div(difference+1,amountOfTree)

                    dmg_each = (difference+1)/amountOfTree;

                    tree.ApplyDamage(dmg_each);
                }
            }
            totalCost = (sunflowerCost + decomposerCost) / 2;
            
            totalWater = AttemptDecrement(totalWater, totalCost);
        }

        // Apply Generation Consumption
        float failingModifier = generator.FailingCount > 0 ? generator.FailingCount * 2 : 1;
        totalWater = AttemptDecrement(totalWater, testWaterConsumptionRate * failingModifier);

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
            // used to calculate global-applied damage
            amountOfSunflowers += 1;
            energyGenerationRate += sunflowerResourceProduction;
        }
        totalEnergy += energyGenerationRate * mushroomSupply.Count;

        // Apply Forest Maintenance Costs
        if (treeCost > 0 || decomposerCost > 0) {
            if( totalEnergy < (treeCost + decomposerCost) / 2)
            {
                // Goes through list of sunflowers to apply damage globally
                foreach (SunflowerComponent sunflower in sunflowerSupply) {
                    difference = totalEnergy - (treeCost + decomposerCost) / 2;

                    // global applied damage = 
                    // div(difference+1,amountOfSunflowers)

                    dmg_each = (difference+1)/amountOfSunflowers;

                    sunflower.ApplyDamage(dmg_each);
                }
            }
            totalCost = (treeCost + decomposerCost) / 2;

            totalEnergy = AttemptDecrement(totalEnergy, totalCost);

        }

        // Apply Generation Consumption
        float failingModifier = generator.FailingCount > 0 ? generator.FailingCount * 2 : 1;
        totalEnergy = AttemptDecrement(totalEnergy, testEnergyConsumptionRate * failingModifier);

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
            amountOfDecomposters += 1;
            organicGenerationRate += decomposerResourceProduction;
        }
        totalOrganic += organicGenerationRate * mushroomSupply.Count;

        // Apply Forest Maintenance Costs
        if (treeCost > 0 || sunflowerCost > 0) {
            if( totalOrganic < (treeCost + sunflowerCost) / 2)
            {
                // Goes through list of sunflowers to apply damage globally
                foreach (DecomposerComponent decomposer in decomposerSupply) {
                    difference = totalOrganic - (treeCost + sunflowerCost) / 2;

                    // global applied damage = 
                    // div(difference+1,amountOfDecomposters)

                    dmg_each = (difference+1)/amountOfDecomposters;

                    decomposer.ApplyDamage(dmg_each);
                }
            }
            totalCost = (treeCost + sunflowerCost) / 2;

            totalOrganic = AttemptDecrement(totalOrganic, totalCost);
        }

        // Apply Generation Consumption
        float failingModifier = generator.FailingCount > 0 ? generator.FailingCount * 2 : 1;
        totalOrganic = AttemptDecrement(totalOrganic, testOrganicConsumptionRate * failingModifier);

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

                // Deduct build costs
                totalEnergy -= treeEnergyBuildCost;
                totalOrganic -= treeOrganicBuildCost;
                break;
            case ComponentType.Sunflower:
                SunflowerComponent sunflowerComponent = newComponent.AddComponent(typeof(SunflowerComponent)) as SunflowerComponent;
                sunflowerSupply.Add(sunflowerComponent);

                // Deduct build costs
                totalOrganic -= sunflowerOrganicBuildCost;
                totalWater -= sunflowerWaterBuildCost;
                break;
            case ComponentType.Mushroom:
                MushroomComponent mushroomComponent = newComponent.AddComponent(typeof(MushroomComponent)) as MushroomComponent;
                mushroomSupply.Add(mushroomComponent);

                // Deduct build costs
                totalEnergy -= mushroomEnergyBuildCost;
                totalWater -= mushroomWaterBuildCost;
                totalOrganic -= mushroomrOrganicBuildCost;
                break;
            case ComponentType.Decomposer:
                DecomposerComponent decomposerComponent = newComponent.AddComponent(typeof(DecomposerComponent)) as DecomposerComponent;
                decomposerSupply.Add(decomposerComponent);

                // Deduct build costs
                totalEnergy -= decomposerEnergyBuildCost;
                totalWater -=decomposerWaterBuildCost;
                break;
        }
    }
    #endregion
}
