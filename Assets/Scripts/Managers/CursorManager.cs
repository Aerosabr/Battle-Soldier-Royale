using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
	public static CursorManager Instance;

	[SerializeField] private Texture2D rightCursorTexture;
	[SerializeField] private Texture2D leftCursorTexture;
	[SerializeField] private Texture2D normalCursorTexture;

	private Vector2 cursorHotspot;

	private void Start()
	{
		Instance = this;
	}

	public void ShowRightCursor()
	{
		cursorHotspot = new Vector2(rightCursorTexture.width/2, rightCursorTexture.height/2);
		Cursor.SetCursor(rightCursorTexture, cursorHotspot, CursorMode.Auto);
	}
	public void ShowLeftCursor()
	{
		cursorHotspot = new Vector2(leftCursorTexture.width / 2, leftCursorTexture.height / 2);
		Cursor.SetCursor(leftCursorTexture, cursorHotspot, CursorMode.Auto);
	}
	public void ShowNormalCursor()
	{
		cursorHotspot = new Vector2(0 , 0);
		Cursor.SetCursor(normalCursorTexture, cursorHotspot, CursorMode.Auto);
	}
}
