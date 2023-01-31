using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetEnergy : MonoBehaviour
{
    [SerializeField] private int EnergyAmount = 10;
    [SerializeField] private int EnergySpeed = 1;
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
        yield return new WaitForSeconds(EnergySpeed);
        Resources.totalEnergy += EnergyAmount;
        StartCoroutine(energyGenerator());
    }
}
