using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    [SerializeField] private CursorManager cursorManager;

    private void Awake() {
        cursorManager = new CursorManager();
    }

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            GameObject[] items = eventData.pointerDrag.gameObject.GetComponent<ItemDragHandler>().inventoryItems;
            if (items != null) {
                GameObject selectedTile = cursorManager.CalculateRayCastHitGameObject();
                PlaceGameObject(items, selectedTile.transform.position);
            }
        }
    }

    private void PlaceGameObject(GameObject[] inventoryItems, Vector3 tilePosition) {
        int selectedItemIndex = 0;
        if (inventoryItems.Length > 1) {
            selectedItemIndex = Random.Range(0, inventoryItems.Length - 1);
        }
        Vector3 itemPosition = new Vector3(tilePosition.x, tilePosition.y + 1, tilePosition.z);
        Debug.Log(itemPosition);
        Transform parentTransform = GameObject.FindGameObjectWithTag("ComponentLayer").transform;
        Quaternion rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
        Instantiate(inventoryItems[selectedItemIndex], itemPosition, rotation, parentTransform);
    }
}
