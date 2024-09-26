using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardSlotVisual : MonoBehaviour
{
	[SerializeField] private Image icon;
	[SerializeField] private Text text;

	public void InitializeCard(CardSO cardSO)
	{
		if (cardSO != null)
		{
			icon.sprite = cardSO.backgrounds[0];
			text.text = cardSO.name;
		}
		else
		{
			icon.sprite = null;
			text.text = "";
		}
	}
}
