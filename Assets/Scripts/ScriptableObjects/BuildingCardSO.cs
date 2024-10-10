using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BuildingType
{
    Economy,
    Defense
}

[CreateAssetMenu()]
public class BuildingCardSO : CardSO
{
    public List<float> Health;
    public List<float> Attack;
    public List<float> AttackSpeed;
    public float AttackRange;
    public List<float> BuildTimer;
    public BuildingType BuildingType;
    public AttackType AttackType;

	public override List<string> ViewCard(int level)
	{
		List<string> list = new List<string>();

		if (Health[level - 1] != 0)
			list.Add("Health: " + Health[level - 1]);
		if (Attack[level - 1] != 0)
		{
			if(BuildingType  == BuildingType.Economy)
				list.Add("Gold per second: " + Attack[level - 1]);
			else
				list.Add("Attack: " + Attack[level - 1]);
		}
		if (AttackSpeed[level - 1] != 0)
			list.Add("Attack Speed: " + AttackSpeed[level - 1]);
		if (AttackRange != 0)
			list.Add("Attack Range: " + AttackRange);
		if (cardCost[level - 1] != 0)
			list.Add("Cost: " + cardCost[level - 1]);
		if (BuildTimer[level - 1] != 0)
			list.Add("Build Timer: " + BuildTimer[level - 1] + "s");
		list.Add("Building Type: " + BuildingType);
		if(AttackType != AttackType.None)
			list.Add("Attack Type: " + AttackType);

		return list;
	}
}

