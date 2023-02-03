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
    private Vector3 cursorDropPosition;
    private Vector3 cursorClickPosition;

    void Start() {
        //Start generating energy.
        eventManagerScript.ExampleEvent += CursorFunction;
    }

    public GameObject CalculateRayCastHitGameObject() {
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit)) {
            if (raycastHit.transform != null) {
                //Our custom method. 
                return raycastHit.transform.gameObject;
            }
        }

        return null;
    }

    private void CursorFunction() {
        // Check for mouse click release
        if (Input.GetMouseButtonUp(0)) {
            GameObject hitObject = CalculateRayCastHitGameObject();
            if (hitObject != null) {
                CurrentClickedGameObject(hitObject);
            }
        }
    }

    private void CurrentClickedGameObject(GameObject gameObject) {
        if (gameObject.tag == "Interactable") {
            cursorDropPosition = gameObject.transform.position;
        } else {
            StartCoroutine(BlockedCursor());
        }
    }

    IEnumerator BlockedCursor() {
        Cursor.SetCursor(cursorBlock, hotSpot, cursorMode);

        yield return new WaitForSeconds(1);

        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    public Vector3 CursorDropPosition {
        get { return cursorDropPosition; }
    }

    private void onDisable() {
        eventManagerScript.ExampleEvent -= CursorFunction;
    }
}
