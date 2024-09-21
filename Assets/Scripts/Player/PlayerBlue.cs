using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlue : Player
{
    public static PlayerBlue Instance { get; private set; }

    private float temp = 0;

    private void Awake()
    {
        Instance = this;
        AddGold(startingGold);
        foreach (CardSO CSO in tempLoadout)
        {
            CardSO newCard = Instantiate(CSO);
            newCard.newCardSO(CSO);
            loadout.Add(newCard);
        }
    }

    private void Start()
    {
        
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
