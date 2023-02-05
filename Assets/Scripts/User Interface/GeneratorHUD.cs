using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ForestComponent;

public class GeneratorHUD : MonoBehaviour
{
    public Image Panel1;
    public Image waterStateImage, energyStateImage, organicStateImage;
    public Sprite decreasingComsumption, increasingConsumption, depletedImage, systemStable;
    private float waterRate = 0.0f;
    private float energyRate = 0.0f;
    private float organicRate = 0.0f;

    private void Awake() {
        decreasingComsumption = Resources.Load<Sprite>("Decreasing");
        increasingConsumption = Resources.Load<Sprite>("Increasing");
        depletedImage = Resources.Load<Sprite>("SystemFailed");
        systemStable = Resources.Load<Sprite>("SystemStable");

        waterStateImage.sprite = systemStable;
        energyStateImage.sprite = systemStable;
        organicStateImage.sprite = systemStable;
    }

    private void Update() {
        if (waterRate > 0) {
            if (waterRate > 0.2) {
                waterStateImage.sprite = waterRate >= 0.5 ? increasingConsumption : decreasingComsumption;
            } else {
                waterStateImage.sprite = systemStable;
            }
        } else {
            waterStateImage.sprite = depletedImage;
        }

        if (energyRate > 0) {
            if (energyRate > 0.2) {
                energyStateImage.sprite = energyRate >= 0.5 ? increasingConsumption : decreasingComsumption;
            } else {
                energyStateImage.sprite = systemStable;
            }
        } else {
            energyStateImage.sprite = depletedImage;
        }

        if (organicRate > 0) {
            if (organicRate > 0.2) {
                organicStateImage.sprite = organicRate >= 0.5 ? increasingConsumption : decreasingComsumption;
            } else {
                organicStateImage.sprite = systemStable;
            }
        } else {
            organicStateImage.sprite = depletedImage;
        }
    }

    public void SetResourceState(ComponentType type, float rate) {
        switch (type) {
            case ComponentType.Tree:
                waterRate = rate;
                break;
            case ComponentType.Sunflower:
                energyRate = rate;
                break;
            case ComponentType.Decomposer:
                organicRate = rate;
                break;
        }
    }
}
