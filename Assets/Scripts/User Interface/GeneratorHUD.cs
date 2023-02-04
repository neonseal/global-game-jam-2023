using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorHUD : MonoBehaviour
{
    public Image Panel1;
    public Image waterStateImage, energyStateImage, organicStateImage;
    public Color activeColor, depletedColor;
    public bool waterActive, energyActive, organicActive = false;

    private void Update() {
        waterStateImage.color = waterActive ? activeColor : depletedColor;
        energyStateImage.color = energyActive ? activeColor : depletedColor;
        organicStateImage.color = organicActive ? activeColor : depletedColor;
    }

    public void PanelcolorChange()
    {
        Panel1.color = activeColor;
    }
}
