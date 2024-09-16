using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlue : Player
{
    public static PlayerBlue Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        AddGold(startingGold);
        foreach (CardSO CSO in tempLoadout)
        {
            CardSO temp = new CardSO(CSO);
            loadout.Add(temp);
        }
    }

    private void Update()
    {
        passiveGoldTimer += Time.deltaTime;
        if (passiveGoldTimer >= passiveGoldTimerMax)
        {
            AddGold(1);
            passiveGoldTimer = 0;
        }
    }
}
