using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour {
    // Use global resource manager to track resource generation/input
    private ForestManager forestManager;

    private void Awake() {
        forestManager = new ForestManager();
    }

    private void Update() {

    }
}