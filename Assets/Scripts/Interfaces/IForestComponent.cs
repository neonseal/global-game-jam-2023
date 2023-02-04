using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IForestComponent {
    float maintenanceCost { get; set; }
    float energyBuildCost { get; set; }
    float waterBuildCost { get; set; }
    float organicBuildCost { get; set; }
    float health { get; set; }
    float maxHealth { get; set; }

    public void ApplyDamage(float damage) {
        health -= damage;
    }

    public void AddHealth(float amount) {
        health += amount;
    }

    // Calculate cost to sustain this tree based on its current health
    public float CalculateMaintenanceCost() {
        return maintenanceCost * (health / 100);
    }
}


