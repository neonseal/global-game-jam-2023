using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IOrganicComponent {
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
}

public class OrganicComponentEventArgs : EventArgs {
    public IOrganicComponent OrganicComponent;
    public OrganicComponentEventArgs(float maint, float energy, float water, float organic, float hp) {
        OrganicComponent.maintenanceCost = maint;
        OrganicComponent.energyBuildCost = energy;
        OrganicComponent.waterBuildCost = water;
        OrganicComponent.organicBuildCost = organic;
        OrganicComponent.health = OrganicComponent.maxHealth = hp;
    }
}

