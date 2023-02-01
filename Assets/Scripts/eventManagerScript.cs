using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class eventManagerScript : MonoBehaviour
{
    public static event Action ExampleEvent;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (ExampleEvent != null)
            {
                ExampleEvent();
            }
        }
    }
}
