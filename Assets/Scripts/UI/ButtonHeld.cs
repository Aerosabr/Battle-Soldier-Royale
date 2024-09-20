using UnityEngine;
using UnityEngine.EventSystems; // Needed for the event interfaces

public class ButtonHeld : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	private bool isPressed = false;

	void Update()
	{
		if (isPressed)
		{
			// Perform the action while the button is pressed down
			Debug.Log("Button is being held down");
			// You can call the function you want to execute here
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		isPressed = true; // Set flag when button is pressed
		Debug.Log("Button Pressed");
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isPressed = false; // Clear flag when button is released
		Debug.Log("Button Released");
	}
}
