using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Camera Components")]
    private Rigidbody rb;
    private PlayerControls playerControls;

    [Header("Movement Variables")]
    [SerializeField] private float speed = 4f;
    private Vector2 inputVector;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        playerControls = new PlayerControls();
    }

    private void Update() {
        inputVector = playerControls.Player.Movement.ReadValue<Vector2>().normalized;
    }

    private void FixedUpdate() {
        Vector3 movementVector = new Vector3(inputVector.x, rb.position.y, inputVector.y);
        rb.MovePosition(rb.position + movementVector * speed * Time.fixedDeltaTime);
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }
}
