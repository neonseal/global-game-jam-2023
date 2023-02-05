using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ForestComponent;
using Generator;
using System.Linq;

public class ForestController : MonoBehaviour {
    private GeneratorController generator;

    [SerializeField] private CounterHUD counterHUD;
    
    [Header("Total Resource Counts")]
    private static float defaultResourceAmount = 2000.0f;
    [SerializeField] private float totalWater = defaultResourceAmount;
    [SerializeField] private float totalEnergy = defaultResourceAmount;
    [SerializeField] private float totalOrganic = defaultResourceAmount;

    [Header("Forest Component Collections")]
    private static List<TreeComponent> treeSupply;
    private static List<SunflowerComponent> sunflowerSupply;
    private static List<DecomposerComponent> decomposerSupply;
    private static List<MushroomComponent> mushroomSupply;
    private float treeCost, sunflowerCost, decomposerCost = 0.0f;

    [Header("Resource Health")]
    [SerializeField] private float damageAmount = 10.0f;
    [SerializeField] private float lastTreeHealth;
    [SerializeField] private float lastSunflowerHealth;
    [SerializeField] private float lastDecomposerHealth;

    [Header("Timer")]
    private float timer;
    [SerializeField] private float timerResetValue = 5.0f;

    private void Awake() {
        generator = new GeneratorController();
        counterHUD.energyCount = counterHUD.waterCount = counterHUD.organicCount = defaultResourceAmount;
        timer = timerResetValue;

        // Initialize Forest Component Collecitons
        treeSupply = new List<TreeComponent>();
        sunflowerSupply = new List<SunflowerComponent>();
        decomposerSupply = new List<DecomposerComponent>();
        mushroomSupply = new List<MushroomComponent>();
    }

    private void Update() {
        lastTreeHealth = treeSupply.Count() > 0 ? treeSupply.LastOrDefault().health : 0.0f;
        lastSunflowerHealth = sunflowerSupply.Count() > 0 ? sunflowerSupply.LastOrDefault().health : 0.0f;
        lastDecomposerHealth = decomposerSupply.Count() > 0 ? decomposerSupply.LastOrDefault().health : 0.0f;

        timer -= Time.fixedDeltaTime;

        UpdateForestMaintenanceCosts();

        if (timer <= 0.0f) {
            UpdateWaterResourceSupply();
            UpdateEnergyResourceSupply();
            UpdateOrganicResourceSupply();
            AddMushroomGeneration();
            UpdateCounterHUD();

            // Apply damage
            if (generator.FailingCount > 0) {
                // Check which resources are failing
                if (totalWater <= 0) {
                    // Damage Sunflowers and Trees
                    DamageResourceSupply(ComponentType.Sunflower);
                    DamageResourceSupply(ComponentType.Tree);
                } else if (totalOrganic <= 0) {
                    // Damage trees and decomposers
                    DamageResourceSupply(ComponentType.Tree);
                    DamageResourceSupply(ComponentType.Mushroom);

                } else if (totalEnergy <= 0) {
                    // Damage trees and decomposers
                    DamageResourceSupply(ComponentType.Tree);
                    DamageResourceSupply(ComponentType.Decomposer);

                }

                // Damage mushrooms if any systems are 
                DamageResourceSupply(ComponentType.Mushroom);
            }

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

    private void AddMushroomGeneration() {
        float mushroomGeneration = 0.0f;

        foreach (MushroomComponent mushroom in mushroomSupply) {
            mushroomGeneration += mushroom.GetCurrentGenerationRate();
        }

        if (mushroomGeneration > 0.0f) {
            totalEnergy += mushroomGeneration;
            totalWater += mushroomGeneration;
            totalOrganic += mushroomGeneration;
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

    public void DamageResourceSupply(ComponentType type) {
        switch (type) {
            case ComponentType.Tree:
                if (treeSupply.Count() > 0) {
                    TreeComponent tree = treeSupply.LastOrDefault();
                    tree.health = AttemptDecrement(tree.health, damageAmount);

                    if (tree.health > 0) {
                        // Apply damage to most recently placed component
                        treeSupply.LastOrDefault().health = tree.health;
                    } else {
                        // Kill most recently placed 
                        treeSupply.Remove(treeSupply.LastOrDefault());
                        DestroyImmediate(sunflowerSupply.LastOrDefault());
                    }
                }
                break;
            case ComponentType.Sunflower:
                if (sunflowerSupply.Count() > 0) {
                    SunflowerComponent sunflower = sunflowerSupply.LastOrDefault();
                    sunflower.health = AttemptDecrement(sunflower.health, damageAmount);

                    if (sunflower.health > 0) {
                        // Apply damage to most recently placed component
                        sunflowerSupply.LastOrDefault().health = sunflower.health;
                    } else {
                        // Kill most recently placed component
                        sunflowerSupply.Remove(sunflowerSupply.LastOrDefault());
                        DestroyImmediate(sunflowerSupply.LastOrDefault());
                    }
                }                
                break;
            case ComponentType.Mushroom:
                if (mushroomSupply.Count() > 0) {
                    MushroomComponent mushroom = mushroomSupply.LastOrDefault();
                    // Apply smaller amount of damage to mushrooms
                    mushroom.health = AttemptDecrement(mushroom.health, damageAmount / 5);

                    if (mushroom.health > 0) {
                        // Apply damage to most recently placed component
                        mushroomSupply.LastOrDefault().health = mushroom.health;
                    } else {
                        // Kill most recently placed component
                        mushroomSupply.Remove(mushroomSupply.LastOrDefault());
                        DestroyImmediate(sunflowerSupply.LastOrDefault());
                    }
                }
                break;
            case ComponentType.Decomposer:
                if (decomposerSupply.Count() > 0) {
                    DecomposerComponent decomposer = decomposerSupply.LastOrDefault();
                    decomposer.health = AttemptDecrement(decomposer.health, damageAmount);


                    if (decomposer.health > 0) {
                        // Apply damage to most recently placed component
                        decomposerSupply.LastOrDefault().health = decomposer.health;
                    } else {
                        // Kill most recently placed component
                        decomposerSupply.Remove(decomposerSupply.LastOrDefault());
                        DestroyImmediate(decomposerSupply.LastOrDefault());
                    }
                }
                break;
        }
    }
    #endregion
}
