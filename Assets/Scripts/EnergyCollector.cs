using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The EnergyCollector class is responsible for generating energy over time.
public class EnergyCollector : MonoBehaviour
{
    //The amount of energy the object has
    [SerializeField] 
    private float energyAmount = 0.0f;

    //The speed at which energy is generated.
    [SerializeField] 
    private float energyWaitTime = 1.0f;

    [SerializeField]
    private float timer = 0.0f;

    //Ammount of energy generated per cycle
    [SerializeField]
    private float energyPerCycle = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Start generating energy.
        eventManagerScript.ExampleEvent += GenerateEnergy;
    }

    //Generates energy over time.
    void GenerateEnergy()
    {
        timer += Time.deltaTime;

        if(timer >= energyWaitTime) 
        {
            energyAmount += energyPerCycle;
            timer = 0.0f;
        }
    }
    
    private void onDisable()
    {
        eventManagerScript.ExampleEvent -= GenerateEnergy;
    }
}
