using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ForestComponent;

public class GeneratorController : MonoBehaviour {
    [SerializeField] private GeneratorHUD generatorHUD;

    [SerializeField] private float energyConsumptionRate = 20f;
    [SerializeField] private float waterConsumptionRate = 20f;
    [SerializeField] private float organicConsumptionRate = 20f;

    [Header("Resource States")]
    // Dictionary <ResourceName, Active>
    private Dictionary<string, bool> resourceStates;


    private void Awake() {
        generatorHUD = new GeneratorHUD();

        resourceStates = new Dictionary<string, bool>();
        resourceStates.Add("Water", true);
        resourceStates.Add("Energy", true);
        resourceStates.Add("Organic", true);
    }

    private void Update() {
        string[] failingResources = resourceStates.Where(kvp => kvp.Value == false).Select(kvp => kvp.Key) as string[];
        //Debug.Log(failingResources);
    }

    #region Resource State Management
    public void UpdateResourceState(ComponentType type, bool activeState) {
        switch(type) {
            case ComponentType.Tree:
                resourceStates["Water"] = activeState;
                generatorHUD.SetResourceState(ComponentType.Tree, activeState);
                Debug.Log("Setting Water State");
                break;
            case ComponentType.Sunflower:
                resourceStates["Energy"] = activeState;
                generatorHUD.SetResourceState(ComponentType.Sunflower, activeState);
                Debug.Log("Setting Energy State");
                break;
            case ComponentType.Decomposer:
                resourceStates["Organic"] = activeState;
                generatorHUD.SetResourceState(ComponentType.Decomposer, activeState);
                Debug.Log("Setting Organic State");
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