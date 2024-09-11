using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRed : Player
{
    public static PlayerRed Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        AddGold(startingGold);
        foreach (CharacterPathSO CPSO in tempLoadout)
        {
            LoadoutCharacter temp = new LoadoutCharacter(CPSO, 1);
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
