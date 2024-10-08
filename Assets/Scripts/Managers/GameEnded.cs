using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnded : MonoBehaviour
{
    public static GameEnded Instance;

    public event EventHandler<OnGameEndedEventArgs> OnGameEnded;
    public class OnGameEndedEventArgs : EventArgs
    {
        public Player.PlayerColor color;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void EndGame(Player.PlayerColor color)
    {
        OnGameEnded?.Invoke(this, new OnGameEndedEventArgs
        {
            color = color
        });
    }
}
