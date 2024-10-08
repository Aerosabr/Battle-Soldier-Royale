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
    }

    private void Start()
    {
        if (GameManager.Instance.GetGamemode() == 0)
        {
            foreach (CardSO CSO in tempLoadout)
            {
                CardSO newCard = Instantiate(CSO);
                newCard.newCardSO(CSO);
                loadout.Add(newCard);
            }
        }
        else
        {
            foreach (CardSO CSO in PlayerManager.Instance.GetPlayerLoadout())
            {
                CardSO newCard = Instantiate(CSO);
                newCard.newCardSO(CSO);
                loadout.Add(newCard);
            }
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
