using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlManager : MonoBehaviour
{
	private enum Mode
	{
		Idle,
		Command,
		Cast,
		Build,
	}

	public static PlayerControlManager Instance;
	private Mode mode;

	private void Start()
	{
		Instance = this;
	}

	public int CheckMode()
	{
		if (mode == Mode.Idle)
			return 0;
		else if(mode == Mode.Command)
			return 1;
		else if(mode == Mode.Cast)
			return 2;
		else if(mode == Mode.Build)
			return 3;
		return 4;
	}

	public void CardSelected(CardSO cardSO)
	{
		if (cardSO.cardType == CardSO.CardType.Character)
		{
			PlayerBlue.Instance.SpawnCharacter(cardSO);
			mode = Mode.Cast;
		}
		else if(cardSO.cardType == CardSO.CardType.Worker)
		{
			PlayerBlue.Instance.SpawnWorker(cardSO, null);
			mode = Mode.Cast;
		}
		else if (cardSO.cardType == CardSO.CardType.Spell)
		{
			PlayerBlue.Instance.SpawnSpell(cardSO);
			mode = Mode.Cast;
		}
		else if (cardSO.cardType == CardSO.CardType.Building)
		{
			PlayerBlue.Instance.BuildBuilding(cardSO, MapManager.Instance.buildingSlots[0]);
			mode = Mode.Build;
		}
	}
	
	public void CardHandled()
	{
		mode = Mode.Command;
	}
}
