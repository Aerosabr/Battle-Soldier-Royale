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
}
