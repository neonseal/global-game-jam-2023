using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeComponent : MonoBehaviour, IOrganicComponent
{
    private ResourceGenerator resourceGenerator;

    [SerializeField] public float maintenanceCost { get; set; } = 10f;
    [SerializeField] public float energyBuildCost { get; set; } = 5f;
    [SerializeField] public float waterBuildCost { get; set; } = 0f;
    [SerializeField] public float organicBuildCost { get; set; } = 5;
    [SerializeField] public float maxHealth { get; set; } = 100;
    [SerializeField] public float health { get; set; };

    private void Awake() {
        resourceGenerator = new ResourceGenerator();
        health = maxHealth
    }

    void Update()
    {
        // Resource generators are less effective if they have taken damage
        resourceGenerator.AmountPerCycle = resourceGenerator.BaseAmountPerCycle * (health / 100);
    }

    private float GetCurrentGenerationRate() {
        return resourceGenerator.AmountPerCycle;
    }

    // Calculate cost to sustain this tree based on its current health
    public float CalculateMaintenanceCost() {
        return maintenanceCost * (health / 100);
    }
}