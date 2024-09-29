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
        foreach (CardSO CSO in tempLoadout)
        {
            CardSO newCard = Instantiate(CSO);
            newCard.newCardSO(CSO);
            loadout.Add(newCard);
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
