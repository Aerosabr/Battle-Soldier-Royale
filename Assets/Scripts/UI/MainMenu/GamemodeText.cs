using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamemodeText : MonoBehaviour
{
	[SerializeField] TMP_Text gamemodeText;
	private string difficulty = "Easy";
	private string gamemode = "Bot";

	private void Start()
	{
		UpdateText();
	}
	public void UpdateDifficulty(string difficulty)
	{
		this.difficulty = difficulty;
		UpdateText();
	}

	public void UpdateGamemode(string gamemode)
	{
		this.gamemode = gamemode;
		UpdateText();
	}

	public void UpdateText()
	{
		switch(gamemode)
		{
			case "Tutorial":
				gamemodeText.text = "Tutorial";
				break;
			case "Player":
				gamemodeText.text = "Vs. Player";
				break;
			case "Bot":
				gamemodeText.text = "Bot (" + difficulty + ")";
				break;
		}
	}
}
