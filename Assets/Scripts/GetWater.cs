using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWater : MonoBehaviour
{
    [SerializeField] private int WaterAmount = 5;
    [SerializeField] private int WaterSpeed = 2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(energyGenerator());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator energyGenerator()
    {
        yield return new WaitForSeconds(WaterSpeed);
        Resources.totalWater += WaterAmount;
        StartCoroutine(energyGenerator());
    }
}
