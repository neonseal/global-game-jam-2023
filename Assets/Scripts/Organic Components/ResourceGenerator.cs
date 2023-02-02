using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The EnergyCollector class is responsible for generating energy over time.
public class ResourceGenerator : MonoBehaviour
{
    // The amount of resource the object has
    private float currentResourceAmount = 0.0f;
    // The speed at which resource is generated.
    private float cycleTime = 1.0f;
    // Ammount of resource generated per cycle
    private float baseAmountPerCycle = 10.0f;
    private float amountPerCycle = 10.0f;
    // Determines if component is in a valid state and able to generate
    private bool componentIsEnabled = true;

    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Start generating energy.
        eventManagerScript.UpdateEvent += GenerateResource;
    }

    //Generates rsource over time.
    void GenerateResource()
    {
        timer += Time.deltaTime;

        if(timer >= cycleTime) 
        {
            if (componentIsEnabled) {
                currentResourceAmount += amountPerCycle;
            }
            timer = 0.0f;
        }
    }
    
    // Getters and Setters
    public float CurrentResourceAmount {
        get { return currentResourceAmount; }
        set { currentResourceAmount = value;  }
    }
    public float CycleTime {
        get { return cycleTime; }
        set { cycleTime = value; }
    }
    public float BaseAmountPerCycle {
        get { return baseAmountPerCycle; }
        set { baseAmountPerCycle = value; }
    }
    public float AmountPerCycle {
        get { return amountPerCycle; }
        set { amountPerCycle = value; }
    }
    public bool ComponentIsEnabled {
        get { return componentIsEnabled; }
        set { componentIsEnabled = value; }
    }

    private void onDisable()
    {
        eventManagerScript.UpdateEvent -= GenerateResource;
    }
}
