using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ForestComponent;
public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    private CursorManager cursorManager;
    [SerializeField] private ForestController forestController;

    private void Awake() {
        cursorManager = new CursorManager();
        forestController = new ForestController();
    }

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            GameObject[] items = eventData.pointerDrag.gameObject.GetComponent<ItemDragHandler>().inventoryItems;
            if (items != null) {   
                GameObject selectedTile = cursorManager.CalculateRayCastHitGameObject();
                GameObject selectRandom = GetRandomFromInventorySlow(items);
                GameObject newComponent = PlaceGameObject(selectRandom, selectedTile.transform.position);
                PlaySoundEffect(newComponent);
                forestController.AddOrganicComponent(newComponent, ConvertToComponentType(items[0].tag));
            }
        }
    }

    private GameObject GetRandomFromInventorySlow(GameObject[] inventoryItems) {
        int selectedItemIndex = 0;
        if (inventoryItems.Length > 1) {
            selectedItemIndex = Random.Range(0, inventoryItems.Length);
        }
        return inventoryItems[selectedItemIndex];
    }

    private GameObject PlaceGameObject(GameObject placedItem, Vector3 tilePosition) {
        Vector3 itemPosition = new Vector3(tilePosition.x, tilePosition.y + .35f, tilePosition.z);
        Transform parentTransform = GameObject.FindGameObjectWithTag("ComponentLayer").transform;
        Quaternion rotation = new Quaternion(60, 180, 270, 0);

        return Instantiate(placedItem, itemPosition, rotation, parentTransform);
    }

    private void PlaySoundEffect(GameObject component) {
        AudioSource audioSource = component.GetComponent<AudioSource>();
        if (audioSource != null) {
            audioSource.Play();
        }
    }

    private ComponentType ConvertToComponentType(string tag) {
        switch (tag) {
            case "Tree":
                return ComponentType.Tree;
            case "Sunflower":
                return ComponentType.Sunflower;
            case "Mushroom":
                return ComponentType.Mushroom;
            case "Decomposer":
                return ComponentType.Decomposer;
            default:
                return ComponentType.Default;
        }
    }
}
