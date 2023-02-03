using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k44Player : MonoBehaviour
{
    //Add this to the player script
    
    public Inventory inventoy;


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        IInventoryItem item = hit.collider.GetComponent<IInventoryItem>();
        if (item != null)
        {
            inventoy.AddItem(item);
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
