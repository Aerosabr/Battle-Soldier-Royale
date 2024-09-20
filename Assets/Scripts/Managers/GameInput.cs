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
        int mode = PlayerControlManager.Instance.CheckMode();

        if (mode == 0 || mode == 1)
        {

			if (Mouse.current.leftButton.isPressed)
			{
				Vector2 inputDir = -playerControls.PlayerCommandMode.Move.ReadValue<Vector2>();
				inputDir = inputDir.normalized;
				return inputDir;
			}
			else if (Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
			{
				Vector2 inputDir = playerControls.PlayerCommandMode.Move.ReadValue<Vector2>();
				inputDir = inputDir.normalized;
				return inputDir;
			}

		}
        else if (mode == 2)
        {
			if (Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
			{
				Vector2 inputDir = playerControls.PlayerCommandMode.Move.ReadValue<Vector2>();
				inputDir = inputDir.normalized;
				return inputDir;
			}
			else if (Mouse.current.leftButton.isPressed)
			{
				Vector2 mousePosition = Input.mousePosition;
				float screenWidth = Screen.width;
				float edgeThreshold = screenWidth * 0.1f;

				if (mousePosition.x < edgeThreshold)
				{
					Vector2 inputDir = new Vector2(-1, 0);
					return inputDir.normalized;
				}
				else if (mousePosition.x > screenWidth - edgeThreshold)
				{
					Vector2 inputDir = new Vector2(1, 0);
					return inputDir.normalized;
				}
			}

		}
        else if (mode == 3)
        {

        }

		return Vector2.zero;
    }

}
