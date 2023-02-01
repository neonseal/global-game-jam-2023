using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class root_con_request : MonoBehaviour
{

    [SerializeField]
    private bool connected = false;

    // Start is called before the first frame update
    void Start()
    {
        eventManagerScript.ExampleEvent += connectRoots;
    }

    private void connectRoots()
    {
        if(connected == false)
        {
            connected = true;
        }
        
    }

    private void onDisable()
    {
        eventManagerScript.ExampleEvent -= connectRoots;
    }
}
