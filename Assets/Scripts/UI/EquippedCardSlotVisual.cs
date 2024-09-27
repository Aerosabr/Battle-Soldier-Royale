using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedCardSlotVisual : MonoBehaviour
{
	[SerializeField] private Image icon;
	public CardSO cardSO;

	public void InitializeCard(CardSO cardSO)
	{
		this.cardSO = cardSO;
		if (cardSO != null)
		{
			icon.sprite = cardSO.backgrounds[0];
		}
		else
		{
			icon.sprite = null;
			Color newColor;
			if (ColorUtility.TryParseHtmlString("#937A50", out newColor))
			{
				icon.color = newColor;
			}
		}
	}
}
