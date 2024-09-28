using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public static SceneLoader Instance;
	void Start()
	{
		Instance = this;
	}

	void Update()
	{
	}

	public void TransitionScene(string sceneName)
	{
		switch (GameManager.Instance.GetGamemode())
		{
			case 0:	//Load Tutorial
				break;
			case 1: //Load PVP
				break;
			case 2: //Load Bots
				SceneManager.LoadScene(sceneName);
				break;
		}
	}
}