using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedLoadoutManager : MonoBehaviour
{
    public static EquippedLoadoutManager Instance;
    private List<CardSO> equippedCardSOList;
    [SerializeField] private List<EquippedCardSlotVisual> equippedVisualList;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        equippedCardSOList = new List<CardSO>();
    }

    public void AddCard(CardSO cardSO)
    {
        if (!equippedCardSOList.Contains(cardSO))
        {
            equippedCardSOList.Add(cardSO);
            UpdateEquippedLoadout();
        }
    }

    public void RemoveCard(CardSO cardSO)
    {
        if(equippedCardSOList.Contains(cardSO))
        {
            equippedCardSOList.Remove(cardSO);
            UpdateEquippedLoadout();
        }
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


}
