using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ForestComponent;

public class GeneratorController : MonoBehaviour {
    private static GeneratorHUD generatorHUD;

    [Header("Audio Controller")]
    [SerializeField] AudioController audioController;
    private AudioSource[] soundEffects;

    [Header("Consumption Costs")]
    [SerializeField] private float energyConsumptionRate = 20f;
    [SerializeField] private float waterConsumptionRate = 20f;
    [SerializeField] private float organicConsumptionRate = 20f;

    [Header("Resource States")]
    // Dictionary <ResourceName, Active>
    private static bool[] resourceStates;
    private int failingCount;

    private void Awake() {
        generatorHUD = gameObject.GetComponent<GeneratorHUD>();
        soundEffects = gameObject.GetComponents<AudioSource>();
        audioController = new AudioController();
        // Set initial resource states for Water, Energy, Organic
        resourceStates = new bool[3] { true, true, true };
    }

    private void Update() {
        UpdateFailingCount();
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
        UpdateFailingCount();

        if (failingCount == 3) {
            //  Trigger Game Over
        } else {
            // Update Music
        }
    }

    private void UpdateFailingCount() {
        failingCount = resourceStates.Count(state => state == false);
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