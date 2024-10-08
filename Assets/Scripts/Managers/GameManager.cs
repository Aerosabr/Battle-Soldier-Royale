using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private float MapSize;
    private int MaxWorkerAmount;
    private float soundVolume;
    private float musicVolume;
    private int gamemode;
    private int difficulty = 0;


    private void Awake()
    {
        soundVolume = 1;
        Instance = this;
        MapSize = 60f;
        MaxWorkerAmount = 10;
		DontDestroyOnLoad(gameObject);
	}

    public void SetGamemode(int gamemode) => this.gamemode = gamemode;
    public void SetDifficulty(int difficulty) => this.difficulty = difficulty;
    public int GetDifficulty() => difficulty;
    public int GetGamemode() => gamemode;
    public void SetSoundVolume(float volume) => soundVolume = volume;
    public void SetMusicVolume(float volume) => musicVolume = volume;
    public float GetSoundVolume() => soundVolume;
    public float GetMusicVolume() => musicVolume;
    public float GetMapSize() => MapSize;
    public int GetMaxWorkerAmount() => MaxWorkerAmount; 

    public void GameStart()
    {
        if (PlayerManager.Instance.CheckForOneAttackCharacter())
        {
            gamemode = 2;
            CameraAnimation.Instance.MoveUp("MainScene");
        }
        else
            WarningSign.Instance.ActivateWithTimer();
    }

    public void GameEnded()
    {
        CameraBattleAnimation.Instance.MoveUp("MainMenu");
    }
}
