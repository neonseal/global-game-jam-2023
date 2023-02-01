using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class root_con_request : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        eventManagerScript.ExampleEvent += connectRoots;
    }

    private void connectRoots()
    {
        Debug.Log("Someone wants to connect with me?! really? ;(");
    }

    private void onDisable()
    {
        eventManagerScript.ExampleEvent -= connectRoots;
    }
}
