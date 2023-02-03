using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube44 : MonoBehaviour, IInventoryItem
{
    
    public string Name
    {
        get
        {
            return "Cube44";
        }
    }

    public Sprite _Image = null;

    public Sprite Image
    {
        get
        {
            return _Image;
        }
    }

    public void OnPickup()
    {
        //Can edit for when player pick ups cube/item
        gameObject.SetActive(false);
    }

    public void OnDrop()
    {
        // may need to move this to a base class or helper method
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 1000))
        {
            gameObject.SetActive(true);
            gameObject.transform.position = hit.point;
        }
    }
   
}
