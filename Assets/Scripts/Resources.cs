using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources: MonoBehaviour
{
   public static int totalEnergy = 0;
   public static int totalWater = 0;

   public Text EnergyText;
   public Text WaterText;

    // Start is called before the first frame update
    void Start()
    {
       // GetComponent<Text>().text = "Energy: " + totalEnergy + "\nWater: " + totalWater;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(totalEnergy+ " " + totalWater);
        EnergyText.text = "Energy: " + totalEnergy + "";
        WaterText.text = "Water: " + totalWater + "";
    }
}
