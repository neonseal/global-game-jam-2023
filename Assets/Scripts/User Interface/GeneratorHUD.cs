using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ForestComponent;

public class GeneratorHUD : MonoBehaviour
{
    public Image Panel1;
    public Image waterStateImage, energyStateImage, organicStateImage;
    public Sprite activeImage, depletedImage;
    private bool waterActive = true;
    private bool energyActive = true;
    private bool organicActive = true;

    private void Awake() {
        activeImage = Resources.Load<Sprite>("SystemRunning");
        depletedImage = Resources.Load<Sprite>("SystemFailed");
    }

    private void Update() {
        waterStateImage.sprite = waterActive ? activeImage : depletedImage;
        energyStateImage.sprite = energyActive ? activeImage : depletedImage;
        organicStateImage.sprite = organicActive ? activeImage : depletedImage;
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
