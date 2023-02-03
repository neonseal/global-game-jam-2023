using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeComponent : MonoBehaviour, IOrganicComponent
{
    private ResourceGenerator resourceGenerator;
    private IOrganicComponent organicComponent;

    [Header("Health/Maintenance")]
    [SerializeField] private float maintenanceCost = 10f;
    [SerializeField] private float energyBuildCost = 5f;
    [SerializeField] private float waterBuildCost = 0f;
    [SerializeField] private float organicBuildCost = 5;
    [SerializeField] private float maxHealth = 100;

    private void Awake() {
        resourceGenerator = new ResourceGenerator();
        organicComponent =  OrganicComponentEventArgs(maintenanceCost, energyBuildCost, waterBuildCost, organicBuildCost, maxHealth);
    }

    void Update()
    {
        // Resource generators are less effective if they have taken damage
        resourceGenerator.AmountPerCycle = resourceGenerator.BaseAmountPerCycle * (organicComponent.Health / 100);
    }

    private float GetCurrentGenerationRate() {
        return resourceGenerator.AmountPerCycle;
    }

    // Calculate cost to sustain this tree based on its current health
    public float CalculateMaintenanceCost() {
        return organicComponent.MaintenanceCost * (organicComponent.Health / 100);
    }
}
