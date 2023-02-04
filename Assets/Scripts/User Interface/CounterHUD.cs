using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterHUD : MonoBehaviour {
    public TextMeshProUGUI energyText, waterText, organicText;
    public float energyCount { get; set; }
    public float waterCount { get; set; }
    public float organicCount { get; set; }

    public CounterHUD(float energy, float water, float organic) {
        energyCount = energy >= 0 ? energy : 0;
        waterCount = water >= 0 ? water : 0;
        organicCount = organic >= 0 ? organic : 0;
    }

    void Awake() {
        UpdateCounterText();
    }

    void Update() {
        UpdateCounterText();
    }

    private void UpdateCounterText() {
        energyText.text = "ENG:" + energyCount.ToString();
        waterText.text = "WTR:" + waterCount.ToString();
        organicText.text = "ORG:" + organicCount.ToString();
    }
}
