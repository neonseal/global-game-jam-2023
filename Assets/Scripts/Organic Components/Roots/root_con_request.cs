using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class root_con_request : MonoBehaviour
{

    [SerializeField]
    private int maxConnections = 4;
    
    [SerializeField]
    private int currentConnections = 0;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.UpdateEvent += connectRoots;
    }

    private void connectRoots(connectorObject)
    {
        if(currentConnections < maxConnections)
        {
            currentConnections += 1;
            
            share_resources(connectorObject);
        }
        
    }
    
    private void share_resources(connectorObject)
    {
        //connectorObject.sendMessage("want nutrients?") ("have nutrients?")
    }

    private void onDisable()
    {
        EventManager.UpdateEvent -= connectRoots;
    }
}
