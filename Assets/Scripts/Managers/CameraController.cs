using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameInput gameInput;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementTime;

    private Vector3 newPosition;
    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;

    private void Start()
    {
        newPosition = transform.position;
    }

    private void Update()
    {
        HandleMovementInput();
        //HandleMouseInput();
    }

    private void HandleMovementInput()
    {
        Vector2 moveDir = gameInput.GetCameraMovement();
        newPosition += new Vector3(moveDir.x, 0f, 0f);

        // Clamp the X position between -20 and 20
        newPosition.x = Mathf.Clamp(newPosition.x, -30f, 30f);

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }
}
