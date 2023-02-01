using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//The ResourceManager class is responsible for managing the resources of the game.
public class ResourceManager : MonoBehaviour
{
    //Static variables that keep track of the total amount of energy and water.
    public static int TotalEnergyStatic = 0;
    public static int TotalWaterStatic = 0;

    //Text components used to display the energy and water resources.
    public Text EnergyText;
    public Text WaterText;

    // Update is called once per frame
    void Update()
    {
        //Update the text components with the current value of the energy and water resources.
        EnergyText.text = "Energy: " + TotalEnergyStatic;
        WaterText.text = "Water: " + TotalWaterStatic;
    }
}
