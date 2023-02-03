using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    [SerializeField]
    private Texture2D cursorTexture;

    void Start()
    {
        //Start generating energy.
        eventManagerScript.ExampleEvent += CursorFunction;
    }


    // Update is called once per frame
    void CursorFunction()
    {
    	Cursor.SetCursor(cursorTexture, new Vector2(10,10), CursorMode.Auto);

    }

    private void onDisable()
    {
        eventManagerScript.ExampleEvent -= CursorFunction;
    }
}
