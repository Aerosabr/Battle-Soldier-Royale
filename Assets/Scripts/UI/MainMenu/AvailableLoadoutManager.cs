using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableLoadoutManager : MonoBehaviour
{
    public static AvailableLoadoutManager Instance;
    [SerializeField] private List<CardSO> availableCardSOList;
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
        foreach (var card in availableCardSOList)
        {
            GameObject cardObject = Instantiate(CardVisual, CardContent, false);
            cardObject.GetComponent<CardSlotVisual>().InitializeCard(card);
        }
        CardVisual.SetActive(false);

	}
}
