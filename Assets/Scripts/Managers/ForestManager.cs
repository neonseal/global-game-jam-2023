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
    private TreeComponent[] treeSupply;

    [Header("Total Resource Counts")]
    [SerializeField] private float totalWater = 100.0f;
    [SerializeField] private float totalEnergy = 100.0f;
    [SerializeField] private float totalOrganic = 100.0f;

    [Header("Timer")]
    private float timer = 1.0f;
    private float timerResetValue = 1.0f;

    private void Awake() {

    }

    private void Update() {
        // Update current resource generation 

        timer -= Time.fixedDeltaTime;

        if (timer <= 0.0f) {
            ApplyForestResourceGeneration();
            ApplyForestMaintenanceCost();

            if (totalWater == 0 && totalEnergy == 0 && totalOrganic == 0) {
                TriggerGameOverState();
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
        float treeCost = 3 * treeCount;
        float sunflowerCost = 2 * sunflowerCount;
        float decomposerCost = 1 * decomposerCount;


        totalWater += (sunflowerCost + decomposerCost);
        totalEnergy += (treeCost + decomposerCost);
        totalOrganic += (treeCost + sunflowerCost);

    }

    private void ApplyForestMaintenanceCost() {
        // Iterate through each collection of organic components
        // Call CalculateMaintenanceCost function 
        // Calculate total cost to maintain forest
        // Substract from each total
        // Check if each total == 0, trigger generator state change
        float treeCost = 5 * treeCount;
        float sunflowerCost = 3 * sunflowerCount;
        float decomposerCost = 1 * decomposerCount;


        totalWater -= (sunflowerCost + decomposerCost) / 2;
        totalEnergy -= (treeCost + decomposerCost) / 2;
        totalOrganic -= (treeCost + sunflowerCost) / 2;
    } 

    private void TriggerGameOverState() {
        Debug.Log("GAME OVER!");
    }
}
