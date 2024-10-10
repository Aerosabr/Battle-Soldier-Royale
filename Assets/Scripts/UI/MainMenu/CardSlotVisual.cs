using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardSlotVisual : MonoBehaviour
{
	[SerializeField] private Image icon;
	[SerializeField] private TMP_Text text;
	public GameObject equippedIcon;
	public CardSO cardSO;

	public void InitializeCard(CardSO cardSO)
	{
		this.cardSO = cardSO;
		if (cardSO != null)
		{
			icon.sprite = cardSO.backgroundVertical[0];
			text.text = cardSO.name;
		}
		else
		{
			icon.sprite = null;
			text.text = "";
		}
	}

	public void EnableVisualEquipCard()
	{
		equippedIcon.SetActive(true);
	}
	public void DisableVisualEquipCard()
	{
		equippedIcon.SetActive(false);
	}
	public bool CheckVisualEquipCard()
	{
		if(equippedIcon.activeSelf)
		{
			return true;
		}
		return false;
	}
}
