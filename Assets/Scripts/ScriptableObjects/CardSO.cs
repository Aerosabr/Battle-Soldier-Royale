using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSO : ScriptableObject
{
    public event EventHandler OnLevelChanged;
    public string Name;
    public enum CardType
	{
		Building,
        Character,
		Spell,
		Worker
	}
	public CardType cardType;
	public List<Sprite> backgrounds;
    public List<Sprite> backgroundVertical;
	public List<int> cardCost;
	public List<int> upgradeCost;
    public List<float> spawnCooldown;
	public GameObject upgradesTab;
	public Transform spawnableObject;
	public int level;
    public int timesCasted;

	public void newCardSO(CardSO CSO) 
	{
		cardType = CSO.cardType;
		backgrounds = CSO.backgrounds;
		cardCost = CSO.cardCost;
		upgradeCost = CSO.upgradeCost;
		upgradesTab = CSO.upgradesTab;
		spawnableObject = CSO.spawnableObject;
		level = CSO.level;
	}

    public void IncreaseCardLevel()
    {
        if (level == upgradeCost.Count)
            return;

        level++;
        OnLevelChanged?.Invoke(this, EventArgs.Empty);
    }

	public virtual List<string> ViewCard(int level) { return null; }
}
