using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AvailableLoadoutManager : MonoBehaviour
{
    public static AvailableLoadoutManager Instance;
	[SerializeField] private List<CardSO> availableBuildingCardList;
	[SerializeField] private List<CardSO> availableCharacterCardList;
	[SerializeField] private List<CardSO> availableSpellCardList;
    [SerializeField] private CardSO worker;

    [SerializeField] private GameObject CardVisual;
    [SerializeField] private Transform CardContent;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
		CardVisual.SetActive(true);
		GenerateAvailableLoadout();
    }

    private void GenerateAvailableLoadout()
    {
		GameObject cardObject = null;

		foreach (var card in availableBuildingCardList)
        {
            cardObject = Instantiate(CardVisual, CardContent, false);
            cardObject.GetComponent<CardSlotVisual>().InitializeCard(card);
        }
		cardObject = Instantiate(CardVisual, CardContent, false);
		cardObject.GetComponent<CardSlotVisual>().InitializeCard(worker);
		foreach (var card in availableCharacterCardList)
		{
			cardObject = Instantiate(CardVisual, CardContent, false);
			cardObject.GetComponent<CardSlotVisual>().InitializeCard(card);
		}
		foreach (var card in availableSpellCardList)
		{
			cardObject = Instantiate(CardVisual, CardContent, false);
			cardObject.GetComponent<CardSlotVisual>().InitializeCard(card);
		}
		CardVisual.SetActive(false);

	}

    public void RandomLoadout()
    {
		int remainingloadoutSlot = 6;
		CardSO randomCard = null;
		List<CardSO> randomLoadout = new List<CardSO>();
		randomLoadout.Add(worker);
		List<CardSO> OffensiveCard = availableCharacterCardList.Concat(availableSpellCardList).ToList();
		for (int i = 0; i < remainingloadoutSlot; ++i)
		{
			randomCard = OffensiveCard[Random.Range(0, OffensiveCard.Count)];
			randomLoadout.Add(randomCard);
			OffensiveCard.Remove(randomCard);
		}
		randomLoadout.Add(availableBuildingCardList[Random.Range(0, availableBuildingCardList.Count)]);
		StartCoroutine(EquippedLoadoutManager.Instance.ReplaceAllCards(randomLoadout));

	}
}
