using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EquippedLoadoutManager : MonoBehaviour
{
    private const int MAXEQUIPPEDLOADOUT = 8;

	public static EquippedLoadoutManager Instance;
    [SerializeField] private List<CardSO> equippedCardSOList;
    [SerializeField] private List<EquippedCardSlotVisual> equippedVisualList;
	void Start()
    {
        Instance = this;
        equippedCardSOList = new List<CardSO>();

    }

    public void AddCard(CardSO cardSO)
    {
        if (!equippedCardSOList.Contains(cardSO) && equippedCardSOList.Count < MAXEQUIPPEDLOADOUT)
        {
            equippedCardSOList.Add(cardSO);
            UpdateEquippedLoadout();
        }
        PlayerManager.Instance.UpdatePlayerCardList(equippedCardSOList);
	}

    public void RemoveCard(CardSO cardSO)
    {
        if(equippedCardSOList.Contains(cardSO))
        {
            equippedCardSOList.Remove(cardSO);
            UpdateEquippedLoadout();
        }
		PlayerManager.Instance.UpdatePlayerCardList(equippedCardSOList);
	}

    private void UpdateEquippedLoadout()
    {
        for(int i = 0; i < equippedVisualList.Count; i++)
        {
            if (i < equippedCardSOList.Count)
            {
                equippedVisualList[i].InitializeCard(equippedCardSOList[i]);
            }
            else
            {
                equippedVisualList[i].InitializeCard(null);
            }
        }
    }

    public IEnumerator ReplaceAllCards(List<CardSO> cardList)
    {
        equippedCardSOList.Clear();
        foreach (var card in cardList)
        {
            equippedCardSOList.Add(card);
            yield return null;
        }
        UpdateEquippedLoadout();
        PlayerManager.Instance.UpdatePlayerCardList(equippedCardSOList);
	}


}
