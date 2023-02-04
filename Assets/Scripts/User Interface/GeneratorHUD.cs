using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ForestComponent;

public class GeneratorHUD : MonoBehaviour
{
    public Image Panel1;
    public Image waterStateImage, energyStateImage, organicStateImage;
    public Color activeColor, depletedColor;
    private bool waterActive = true;
    private bool energyActive = true;
    private bool organicActive = true;

    private void Awake() {
        activeColor = Color.blue;
        depletedColor = Color.red;
    }

    private void Update() {
        waterStateImage.color = waterActive ? activeColor : depletedColor;
        //energyStateImage.color = energyActive ? activeColor : depletedColor;
        //organicStateImage.color = organicActive ? activeColor : depletedColor;
    }

    public void SetResourceState(ComponentType type, bool activeState) {
        switch (type) {
            case ComponentType.Tree:
                waterActive = activeState;
                break;
            case ComponentType.Sunflower:
                energyActive = activeState;
                break;
            case ComponentType.Decomposer:
                organicActive = activeState;
                break;
        }
    }
}
