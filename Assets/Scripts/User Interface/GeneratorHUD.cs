using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorHUD : MonoBehaviour
{
    public Image Panel1;
    public Image Image1, Image2, Image3;
    public Color color1,color2,color3,color4,color5,color6;
    public bool waterActive, energyActive, organicActive = false;

    private void Update() {
        CheckWaterState();
        CheckEnergyState();
        CheckOrganicState();
    }

    public void PanelcolorChange()
    {
        Panel1.color = color1;
    }

    private void CheckWaterState()
    {
        if(waterActive == false)
        {
            Image1.color = color5;
        }
        else if (waterActive == true)
        {
            Image1.color = color6;
            waterActive = false;
        }
    }

    private void CheckEnergyState()
    {
        if (energyActive == false)
        {
            Image1.color = color5;
        }
        else if (energyActive == true)
        {
            Image1.color = color6;
            energyActive = false;
        }
    }

    private void CheckOrganicState()
    {
        if (organicActive == false)
        {
            Image1.color = color5;
        }
        else if (organicActive == true)
        {
            Image1.color = color6;
            organicActive = false;
        }
    }
}
