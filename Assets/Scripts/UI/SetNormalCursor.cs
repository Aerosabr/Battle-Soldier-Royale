using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNormalCursor : MonoBehaviour
{
	[SerializeField] private Texture2D normalCursorTexture;

	private Vector2 cursorHotspot;

	public void ShowNormalCursor()
	{
		cursorHotspot = new Vector2(0, 0);
		Cursor.SetCursor(normalCursorTexture, cursorHotspot, CursorMode.Auto);
	}
}
