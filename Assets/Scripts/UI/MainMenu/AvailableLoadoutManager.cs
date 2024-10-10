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

	private List<CardSlotVisual> cardSlotVisualList = new List<CardSlotVisual>();
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
		CardSlotVisual CSV = null;

		foreach (var card in availableBuildingCardList)
        {
            cardObject = Instantiate(CardVisual, CardContent, false);
			CSV = cardObject.GetComponent<CardSlotVisual>();
			CSV.InitializeCard(card);
			cardSlotVisualList.Add(CSV);

		}
		cardObject = Instantiate(CardVisual, CardContent, false);
		CSV = cardObject.GetComponent<CardSlotVisual>();
		CSV.InitializeCard(worker);
		cardSlotVisualList.Add(CSV);
		foreach (var card in availableCharacterCardList)
		{
			cardObject = Instantiate(CardVisual, CardContent, false);
			CSV = cardObject.GetComponent<CardSlotVisual>();
			CSV.InitializeCard(card);
			cardSlotVisualList.Add(CSV);
		}
		foreach (var card in availableSpellCardList)
		{
			cardObject = Instantiate(CardVisual, CardContent, false);
			CSV = cardObject.GetComponent<CardSlotVisual>();
			CSV.InitializeCard(card);
			cardSlotVisualList.Add(CSV);
		}
		CardVisual.SetActive(false);

	}

    public void RandomLoadout()
    {
		ClearEquipVisualCard();
		int remainingloadoutSlot = 6;
		CardSO randomCard = null;
		List<CardSO> randomLoadout = new List<CardSO>();
		randomLoadout.Add(worker);
		EquipVisualCard(worker);
		List <CardSO> OffensiveCard = availableCharacterCardList.Concat(availableSpellCardList).ToList();
		for (int i = 0; i < remainingloadoutSlot; ++i)
		{
			randomCard = OffensiveCard[Random.Range(0, OffensiveCard.Count)];
			randomLoadout.Add(randomCard);
			EquipVisualCard(randomCard);
			OffensiveCard.Remove(randomCard);
		}
		randomCard = availableBuildingCardList[Random.Range(0, availableBuildingCardList.Count)];
		randomLoadout.Add(randomCard);
		EquipVisualCard(randomCard);
		StartCoroutine(EquippedLoadoutManager.Instance.ReplaceAllCards(randomLoadout));

	}

	public void ClearEquipVisualCard()
	{
		foreach (var visualSlot in cardSlotVisualList)
		{
			visualSlot.DisableVisualEquipCard();
		}
	}

	public void EquipVisualCard(CardSO cardSO)
	{
		foreach (var visualSlot in cardSlotVisualList)
		{
			if (visualSlot.cardSO == cardSO)
			{
				visualSlot.EnableVisualEquipCard();
				return;
			}
		}
	}

	public void UnequipVisualCard(CardSO cardSO)
	{
		foreach(var visualSlot in cardSlotVisualList)
		{
			if(visualSlot.cardSO == cardSO)
			{
				visualSlot.DisableVisualEquipCard();
				return;
			}
		}
	}
}
