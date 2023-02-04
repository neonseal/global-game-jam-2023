using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action UpdateEvent;

    private void Update()
    {
        if (UpdateEvent != null)
        {
            UpdateEvent();
        }
        
    }
}
