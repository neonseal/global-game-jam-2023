using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerComponent : MonoBehaviour
{
    private OrganicComponent organicComponent;
    private ResourceGenerator resourceGenerator;

    [Header("Health/Maintenance")]
    [SerializeField] private float maintenanceCost = 10f;
    [SerializeField] private float energyBuildCost = 0f;
    [SerializeField] private float waterBuildCost = 5f;
    [SerializeField] private float organicBuildCost = 5;
    [SerializeField] private float maxHealth = 100;

    private void Awake() {
        resourceGenerator = new ResourceGenerator();
        organicComponent = new OrganicComponent(maintenanceCost, energyBuildCost, waterBuildCost, organicBuildCost, maxHealth);
    }
}
