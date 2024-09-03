using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
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
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }

        // Clamp the X position between -20 and 20
        newPosition.x = Mathf.Clamp(newPosition.x, -20f, 20f);

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }
}
