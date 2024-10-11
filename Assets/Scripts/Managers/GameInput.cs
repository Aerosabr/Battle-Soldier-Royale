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
		playerControls.Player.Enable();
		playerControls.Player.CharacterBar.performed += CharacterBar_performed;
		Debug.Log(playerControls.Player.CharacterBar.bindings);
	}

	private void CharacterBar_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		switch (obj.control.displayName)
		{
			default:
				Debug.Log(obj.control.displayName);
				break;
		}

	}

	public Vector2 GetCameraMovement()
	{
		int mode = PlayerControlManager.Instance.CheckMode();
		float speedMultiplier = 5.0f;
		if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
		{
			Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
			float screenWidth = Screen.width;
			float edgeThreshold = screenWidth * 0.1f;
			speedMultiplier = 3.0f;

			if (touchPosition.x < edgeThreshold)
			{
				Vector2 inputDir = new Vector2(-1, 0);
				return (inputDir.normalized * speedMultiplier);
			}
			else if (touchPosition.x > screenWidth - edgeThreshold)
			{
				Vector2 inputDir = new Vector2(1, 0);
				return (inputDir.normalized * speedMultiplier);
			}
		}
		else if (!Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed)
		{
			Vector2 mousePosition = Input.mousePosition;
			float screenWidth = Screen.width;
			float edgeThreshold = screenWidth * 0.1f;

			if (mousePosition.x < edgeThreshold)
			{
				Vector2 inputDir = new Vector2(-1, 0);
				CursorManager.Instance.ShowLeftCursor();
				return inputDir.normalized;
			}
			else if (mousePosition.x > screenWidth - edgeThreshold)
			{
				Vector2 inputDir = new Vector2(1, 0);
				CursorManager.Instance.ShowRightCursor();
				return inputDir.normalized;
			}
			else
			{
				CursorManager.Instance.ShowNormalCursor();
			}
		}
		else if (Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
		{
			Vector2 inputDir = playerControls.Player.Move.ReadValue<Vector2>();
			inputDir = inputDir.normalized;
			return inputDir;
		}
		
		return Vector2.zero;
	}
}
