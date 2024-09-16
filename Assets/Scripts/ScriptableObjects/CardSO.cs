using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class CardSO : ScriptableObject
{
	public enum CardType
	{
		Character,
		Spell,
		Building
	}
	public CardType cardType;
	public List<Sprite> backgrounds;
	public List<int> cardCost;
	public List<int> upgradeCost;
	public GameObject upgradesTab;
	public Transform spawnableObject;
	public int level;

	public CardSO(CardSO CSO) 
	{
		cardType = CSO.cardType;
		backgrounds = CSO.backgrounds;
		cardCost = CSO.cardCost;
		upgradeCost = CSO.upgradeCost;
		upgradesTab = CSO.upgradesTab;
		spawnableObject = CSO.spawnableObject;
		level = CSO.level;
	}
}
