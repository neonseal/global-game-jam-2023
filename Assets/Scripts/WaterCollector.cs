using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The WaterCollector class is responsible for generating water over time.
public class WaterCollector : MonoBehaviour
{
    //The amount of water generated per cycle.
    [SerializeField] private int _waterAmount = 5;

    //The speed at which water is generated.
    [SerializeField] private int _waterWaitTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        //Start generating water.
        StartCoroutine(GenerateWater());
    }

    //Generates water over time.
    IEnumerator GenerateWater()
    {
        //Wait for the specified amount of time before generating water.
        yield return new WaitForSeconds(_waterWaitTime);

        //Increase the total amount of water.
        ResourceManager.TotalWaterStatic += _waterAmount;

        //Start generating water again.
        StartCoroutine(GenerateWater());
    }
}
