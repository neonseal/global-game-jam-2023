using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OrganicComponent : MonoBehaviour {
    private float maintenanceCost;
    private float energyBuildCost;
    private float waterBuildCost;
    private float organicBuildCost;
    private float health;
    private float maxHealth;

    // Set Initial Values
    public OrganicComponent(float maint, float energy, float water, float organic, float hp) {
        maintenanceCost = maint;
        energyBuildCost = energy;
        waterBuildCost = water;
        organicBuildCost = organic;
        health = maxHealth = hp;
    }

    public void ApplyDamage(float damage) {
        health -= damage;
    }

    public void AddHealth(float amount) {
        health += amount;
    }

    // Getters and Setters
    public float MaintenanceCost {
        get { return maintenanceCost; }
        set { maintenanceCost = value; }
    }
    public float EnergyBuildCost {
        get { return energyBuildCost; }
        set { energyBuildCost = value; }
    }
    public float WaterBuildCost {
        get { return waterBuildCost; }
        set { waterBuildCost = value; }
    }
    public float OrganicBuildCost {
        get { return organicBuildCost; }
        set { organicBuildCost = value; }
    }
    public float Health {
        get { return health; }
        set { health = value; }
    }
    public float MaxHealth {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

}
