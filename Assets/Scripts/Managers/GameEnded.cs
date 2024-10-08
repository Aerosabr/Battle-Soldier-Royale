using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEnded : MonoBehaviour
{
    public static GameEnded Instance;

    public event EventHandler<OnGameEndedEventArgs> OnGameEnded;
    public class OnGameEndedEventArgs : EventArgs
    {
        public Player.PlayerColor color;
    }

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
            GameManager.Instance.GameEnded();
        });
    }

    public void EndGame(Player.PlayerColor color)
    { 
        background.SetActive(true);
        if (color == Player.PlayerColor.Blue)
        {
            //Defeat
            defeatText.SetActive(true);
        }
        else
        {
            //Victory
            victoryText.SetActive(true);
        }

        //Disable inputs
    }
}
