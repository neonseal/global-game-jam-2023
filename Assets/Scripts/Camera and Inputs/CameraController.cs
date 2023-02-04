using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour {
    [Header("Speed Controls")]
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float movementTime = 5f;

    private InputActions inputActions;
    private Vector3 movementVector;

    private void Awake() {
        inputActions = new InputActions();
    }

    private void Update() {
        Vector3 inputVector = inputActions.ActionMap.CameraMovement.ReadValue<Vector2>().normalized;
        movementVector = new Vector3(inputVector.x, transform.position.y, inputVector.y);
    }

    private void FixedUpdate() {
        Vector3 newPosition = transform.position + movementVector * movementSpeed;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }

    private void OnEnable() {
        inputActions.Enable();
    }

    private void onDisable() {
        inputActions.Disable();
    }
}