using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour {
    // Use global resource manager to track resource generation/input
    private ForestController forestManager;

    private void Awake() {
        forestManager = new ForestController();
    }

    private void Update() {

    }
}