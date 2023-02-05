using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ForestComponent;

public class MushroomComponent : MonoBehaviour, IForestComponent {
    [SerializeField] public float maintenanceCost { get; set; } = 10f;
    [SerializeField] public float energyBuildCost { get; set; } = 0f;
    [SerializeField] public float waterBuildCost { get; set; } = 0f;
    [SerializeField] public float organicBuildCost { get; set; } = 0f;
    [SerializeField] public float maxHealth { get; set; } = 100;
    [SerializeField] public float health { get; set; }

    [SerializeField] public int maxConnections { get; set; } = 4;
    [SerializeField] public int actualConnections { get; set; } = 0;

    private void Awake() {
        health = maxHealth;
        Duplicate_NearNeighbours();
    }

    private void Duplicate_NearNeighbours()
    {

        if(actualConnections < maxConnections)
        {

            // physics.overlapsphre will check for collisions
            // first parameter is center of the sphere, which will be our object's position: https://docs.unity3d.com/ScriptReference/Transform-position.html
            // https://docs.unity3d.com/ScriptReference/Physics.OverlapSphereNonAlloc.html
            int maxColliders = 10;
            Vector3 center = transform.position;

            Collider[] colliders = new Collider[maxColliders];

            int numColliders = Physics.OverlapSphereNonAlloc(center,radius, colliders);

            for (int i = 0; i < numColliders; i++)
            {
                if(colliders[i].tag == "roots") 
                {
                    colliders[i].SendMessage("duplicateResources", gameObject.name);
                }
            }
        }
    }
}
