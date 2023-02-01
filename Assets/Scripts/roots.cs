using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roots : MonoBehaviour
{
    [SerializeField]
    private float radius = 1f;

    [SerializeField]
    // int for max of colliders to be found in collider sphere
    private int KNN = 4;

    // Start is called before the first frame update
    void Start()
    {
        eventManagerScript.ExampleEvent += KNearestNeighbours;
    }

    private void KNearestNeighbours()
    {
        // physics.overlapsphre will check for collisions
        // first parameter is center of the sphere, which will be our object's position: https://docs.unity3d.com/ScriptReference/Transform-position.html
        // https://docs.unity3d.com/ScriptReference/Physics.OverlapSphereNonAlloc.html
        int maxColliders = KNN;
        Vector3 center = transform.position;
        
        Collider[] colliders = new Collider[maxColliders];

        int numColliders = Physics.OverlapSphereNonAlloc(center,radius,colliders);

        for (int i = 0; i < numColliders; i++)
        {
            colliders[i].SendMessage("connectRoots()");
        }

    }

    private void connectRoots()
    {
        Debug.Log("Someone wants to connect with me?! really? ;(");
    }


    private void onDisable()
    {
        eventManagerScript.ExampleEvent -= KNearestNeighbours;
    }
}
