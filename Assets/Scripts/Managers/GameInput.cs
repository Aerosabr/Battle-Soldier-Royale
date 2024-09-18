using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameInput : MonoBehaviour
{
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.PlayerCommandMode.Enable();
        playerControls.PlayerCommandMode.CharacterBar.performed += CharacterBar_performed;
        Debug.Log(playerControls.PlayerCommandMode.CharacterBar.bindings);
    }

    private void CharacterBar_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        switch(obj.control.displayName)
        {
            default:
                Debug.Log(obj.control.displayName);
                break;
        }
        
    }

    public Vector2 GetCameraMovement()
    {
        if (Mouse.current.leftButton.isPressed || Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            Vector2 inputDir = playerControls.PlayerCommandMode.Move.ReadValue<Vector2>();
            inputDir = inputDir.normalized;
            return inputDir;
        }
        return Vector2.zero;
    }

}
