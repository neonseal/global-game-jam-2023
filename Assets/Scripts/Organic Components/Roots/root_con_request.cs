using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class root_con_request : MonoBehaviour
{

    [SerializeField]
    private int maxConnections = 4;
    
    [SerializeField]
    private int currentConnections = 0;

    private void connectRoots()
    {
        if(currentConnections < maxConnections)
        {
            currentConnections += 1;
            
            share_resources();
        }
        
    }
    
    private void share_resources()
    {
        //connectorObject.sendMessage("want nutrients?") ("have nutrients?")
    }
}
