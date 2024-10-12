using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEnded : MonoBehaviour
{
    public static GameEnded Instance;

    [SerializeField] private GameObject victoryText;
    [SerializeField] private GameObject defeatText;
    [SerializeField] private GameObject background;
    [SerializeField] private Button returnButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        returnButton.onClick.AddListener(() =>
        {
            //Scene transition here
            SoundManager.Instance.ButtonPressed();
            GameManager.Instance.GameEnded();
        });
    }

    public void EndGame(Player.PlayerColor color)
    { 
        background.SetActive(true);
        if (color == Player.PlayerColor.Blue)
        {
            //Defeat
            SoundManager.Instance.Defeat();
            defeatText.SetActive(true);
        }
        else
        {
            //Victory
            SoundManager.Instance.Victory();
            victoryText.SetActive(true);
        }

        //Disable inputs
    }
}
