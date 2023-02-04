using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ForestComponent;

public class GeneratorController : MonoBehaviour {
    private static GeneratorHUD generatorHUD;

    [SerializeField] private float energyConsumptionRate = 20f;
    [SerializeField] private float waterConsumptionRate = 20f;
    [SerializeField] private float organicConsumptionRate = 20f;

    [Header("Resource States")]
    // Dictionary <ResourceName, Active>
    private static bool[] resourceStates;


    private void Awake() {
        generatorHUD = gameObject.GetComponent<GeneratorHUD>();
        // Set initial resource states for Water, Energy, Organic
        resourceStates = new bool[3] { true, true, true };
    }

    private void Update() {
        int failingCount = resourceStates.Count(state => state == false);
    }

    #region Resource State Management
    public void UpdateResourceState(ComponentType type, bool activeState) {
        switch(type) {
            case ComponentType.Tree:
                resourceStates[0] = activeState;
                generatorHUD.SetResourceState(ComponentType.Tree, activeState);
                break;
            case ComponentType.Sunflower:
                resourceStates[1] = activeState;
                generatorHUD.SetResourceState(ComponentType.Sunflower, activeState);
                break;
            case ComponentType.Decomposer:
                resourceStates[2] = activeState;
                generatorHUD.SetResourceState(ComponentType.Decomposer, activeState);
                break;
        }
    }


    #endregion

    #region Getters and Setters
    public float EnergyConsumptionRate {
        get { return energyConsumptionRate; }
        set { energyConsumptionRate = value; }
    }

    public float WaterConsumptionRate {
        get { return waterConsumptionRate; }
        set { waterConsumptionRate = value; }
    }

    public float OrganicConsumptionRate {
        get { return organicConsumptionRate; }
        set { organicConsumptionRate = value; }
    }
    #endregion
}