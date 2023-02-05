
using UnityEngine;
using UnityEngine.EventSystems;

    public class ToggleGameObjectButton : MonoBehaviour
    {
        public GameObject ObjectToToggle;

        public void SetGameObjectActive()
        {
            ObjectToToggle.SetActive(true);
        }

        public void SetGameObjectUnactive()
        {
            ObjectToToggle.SetActive(false);
        }
        
    }
