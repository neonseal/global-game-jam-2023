using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The EnergyCollector class is responsible for generating energy over time.
public class EnergyCollector : MonoBehaviour
{
    //The amount of energy generated per cycle.
    [SerializeField] private int _energyAmount = 10;

    //The speed at which energy is generated.
    [SerializeField] private int _energyWaitTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        //Start generating energy.
        StartCoroutine(GenerateEnergy());
    }

    //Generates energy over time.
    IEnumerator GenerateEnergy()
    {
        //Wait for the specified amount of time before generating energy.
        yield return new WaitForSeconds(_energyWaitTime);

        //Increase the total amount of energy.
        ResourceManager.TotalEnergyStatic += _energyAmount;

        //Start generating energy again.
        StartCoroutine(GenerateEnergy());
    }
}
