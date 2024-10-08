using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerChecker : MonoBehaviour
{
	[SerializeField] private GameObject gameManagerObject;
	private GameManager gameManager;
	void Awake()
	{
		gameManager = FindObjectOfType<GameManager>();
		if (gameManager == null)
		{
			gameManager = Instantiate(gameManagerObject).GetComponent<GameManager>();
		}
	}

	public void StartGame()
	{
		gameManager.GameStart();
	}
}

