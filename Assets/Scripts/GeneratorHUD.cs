using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorHUD : MonoBehaviour
{
    public Image Panel1;
    public Image Image1, Image2, Image3;
    public Color color1,color2,color3,color4,color5,color6;
    public bool Gen1, Gen2, Gen3 = false;

    public void PanelcolorChange()
    {
        Panel1.color = color1;
    }

    private void Slot1color()
    {
        if(Gen1 == false)
        {
            Image1.color = color5;
        }
        else if (Gen1 == true)
        {
            Image1.color = color6;
            Gen1 = false;
        }
    }

    private void Slot2color()
    {
        if (Gen2 == false)
        {
            Image1.color = color5;
        }
        else if (Gen2 == true)
        {
            Image1.color = color6;
            Gen2 = false;
        }
    }

    private void Slot3color()
    {
        if (Gen3 == false)
        {
            Image1.color = color5;
        }
        else if (Gen3 == true)
        {
            Image1.color = color6;
            Gen3 = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
