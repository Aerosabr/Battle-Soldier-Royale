using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private float MapSize;
    private int MaxWorkerAmount;

    private void Awake()
    {
        Instance = this;
        MapSize = 60f;
        MaxWorkerAmount = 15;
    }

    public float GetMapSize() => MapSize;
    public int GetMaxWorkerAmount() => MaxWorkerAmount; 
}
