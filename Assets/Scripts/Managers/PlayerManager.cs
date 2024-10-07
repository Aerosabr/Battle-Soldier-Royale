using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

	[SerializeField] private List<CardSO> playerCardSO { get; set; } = new List<CardSO>();

	void Start()
    {
        Instance = this;
    }

    public void UpdatePlayerCardList(List<CardSO> cardList)
    {
        playerCardSO = cardList;
    }

    public List<CardSO> GetPlayerLoadout() { return playerCardSO; }
}
