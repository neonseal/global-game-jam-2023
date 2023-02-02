using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestManager : MonoBehaviour {
    [Header("Component Counts")]
    [SerializeField] private int treeCount = 2;
    [SerializeField] private int sunflowerCount = 1;
    [SerializeField] private int decomposerCount = 1;
    [SerializeField] private int mushroomCount = 7;

    [Header("Forest Component Collections")]
    [SerializeField] private OrganicComponent[] treeSupply;

    [Header("Total Resource Counts")]
    private float totalWater = 0.0f;
    private float totalEnergy = 0.0f;
    private float totalOrganic = 0.0f;

    [Header("Timer")]
    private float timer = 1.0f;
    private float timerResetValue = 1.0f;

    private void Update() {
        // Update current resource generation 

        timer -= Time.fixedDeltaTime;

        if (timer <= 0.0f) {
            ApplyForestResourceGeneration();
            ApplyForestMaintenanceCost();
        }
    }

    private void ApplyForestResourceGeneration() {


    }

    public void ApplyForestMaintenanceCost() {
        // Iterate through each collection of organic components
        // Call calculate maintenance cost function 
        // Calculate total cost to maintain forest
        float treeCost = 5 * treeCount;
        float sunflowerCost = 3 * sunflowerCount;
        float decomposerCost = 1 * decomposerCount;


        totalWater -= (sunflowerCost + decomposerCost) / 2;
        totalEnergy -= (treeCost + decomposerCost) / 2;
        totalOrganic -= (treeCost + sunflowerCost) / 2;
    } 


}
