using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {

    [SerializeField]
    private Texture2D cursorTexture;

    [SerializeField]
    private Texture2D cursorBlock;

    [SerializeField]
    private Vector2 hotSpot;

    private CursorMode cursorMode = CursorMode.Auto;

    void Start() {
        //Start generating energy.
        eventManagerScript.ExampleEvent += CursorFunction;
    }


    // Update is called once per frame
    void CursorFunction() {
        //Check for mouse click 
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit)) {
                if (raycastHit.transform != null) {
                    //Our custom method. 
                    CurrentClickedGameObject(raycastHit.transform.gameObject);
                }
            }
        }
    }

    void CurrentClickedGameObject(GameObject gameObject) {
        if (gameObject.tag == "Interactable") {
            Debug.Log(gameObject.transform.position);
        } else {
            StartCoroutine(BlockedCursor());
        }
    }

    IEnumerator BlockedCursor() {
        Cursor.SetCursor(cursorBlock, hotSpot, cursorMode);

        yield return new WaitForSeconds(1);

        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }


    private void onDisable() {
        eventManagerScript.ExampleEvent -= CursorFunction;
    }
}
