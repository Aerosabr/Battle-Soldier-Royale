using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameInput : MonoBehaviour
{
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.CharacterBar.performed += CharacterBar_performed;
        Debug.Log(playerControls.Player.CharacterBar.bindings);
    }

    private void CharacterBar_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        switch(obj.control.displayName)
        {
            default:
                break;
        }
    }

    public Vector2 GetCameraMovement()
    {
        Vector2 inputDir = playerControls.Player.Move.ReadValue<Vector2>();

        inputDir = inputDir.normalized;
        return inputDir;
    }
}
