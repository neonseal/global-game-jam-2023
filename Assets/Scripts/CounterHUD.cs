using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterHUD : MonoBehaviour
{
    public TextMeshProUGUI energyText, waterText, organicText;
    //public TextMeshPro ENG;
    int energyCount;
    int waterCount;
    int organicCount;

    // Start is called before the first frame update
    void Start()
    {
        energyText.text = "ENG:" + energyCount.ToString();
        waterText.text = "WTR:" + waterCount.ToString();
        organicText.text = "ORG:" + organicCount.ToString();
    }

    public void addEnergy()
    {
        
        energyCount = energyCount + /*(Can add int from another script here)*/ 1;
        energyText.text = "ENG:" + energyCount.ToString();
        
    }

    public void addWater()
    {

        waterCount = waterCount + /*(Can add int from another script here)*/ 1;
        waterText.text = "WTR:" + waterCount.ToString();

    }

    public void addOrganic()
    {

        organicCount = organicCount + /*(Can add int from another script here)*/ 1;
        organicText.text = "ORG:" + organicCount.ToString();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
