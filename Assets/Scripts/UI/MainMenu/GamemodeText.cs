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
		float difficulty = GameManager.Instance.GetDifficulty();
		if (difficulty == 1)
			this.difficulty = "Easy";
		else if (difficulty == 1.5f)
			this.difficulty = "Medium";
		else if (difficulty == 2)
			this.difficulty = "Hard";
		UpdateText();

	}
	public void UpdateDifficulty(string difficulty)
	{
		this.difficulty = difficulty;
		if (difficulty == "Easy")
			GameManager.Instance.SetDifficulty(1);
		else if(difficulty == "Medium")
			GameManager.Instance.SetDifficulty(1.5f);
		else if(difficulty == "Hard")
			GameManager.Instance.SetDifficulty(2);
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
