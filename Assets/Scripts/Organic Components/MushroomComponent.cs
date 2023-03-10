using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ForestComponent;

public class MushroomComponent : MonoBehaviour, IForestComponent {
    private ResourceGenerator resourceGenerator;
    [SerializeField] public float maintenanceCost { get; set; } = 10f;
    [SerializeField] public float energyBuildCost { get; set; } = 100f;
    [SerializeField] public float waterBuildCost { get; set; } = 100f;
    [SerializeField] public float organicBuildCost { get; set; } = 100f;
    [SerializeField] public float maxHealth { get; set; } = 100;
    [SerializeField] public float health { get; set; }

    private void Awake() {
        resourceGenerator = new ResourceGenerator();
        resourceGenerator.AmountPerCycle = 2.0f;
        resourceGenerator.BaseAmountPerCycle = 25.0f;
        health = maxHealth;
    }

    void Update() {
        // Resource generators are less effective if they have taken damage
        resourceGenerator.AmountPerCycle = resourceGenerator.BaseAmountPerCycle * (health / 100);
    }

    public float GetCurrentGenerationRate() {
        return resourceGenerator.AmountPerCycle;
    }
}
