using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharacterType
{
    Worker,
    Melee,
    Ranged
}

public enum AttackType
{
    None,
    Single,
    AOE
}

[CreateAssetMenu()]
public class CharacterCardSO : CardSO
{
    public List<int> Health;
    public List<int> Attack;
    public List<float> AttackSpeed;
    public List<float> MoveSpeed;
    public float AttackRange;
    public CharacterType CharacterType;
    public AttackType AttackType;

    public int GetCardStrength()
    {
        int cardStrength = 0;
        cardStrength += Health[level - 1] / 2;
        switch (AttackType)
        {
            case AttackType.None:
            case AttackType.Single:
                cardStrength += (int)(Attack[level - 1] * AttackSpeed[level - 1] * AttackRange);
                break;
            case AttackType.AOE:
                cardStrength += (int)(Attack[level - 1] * AttackSpeed[level - 1] * AttackRange);
                break;
        }

        return cardStrength;
    }

	public override List<string> ViewCard(int level)
	{
		List<string> list = new List<string>();

		if (Health[level - 1] != 0)
			list.Add("Health: " + Health[level - 1]);
		if (Attack[level - 1] != 0)
		{
			if (CharacterType == CharacterType.Worker)
				list.Add("Gold per second: " + Attack[level - 1]);
			else
				list.Add("Attack: " + Attack[level - 1]);
		}
		if (AttackSpeed[level - 1] != 0)
		{
			if (CharacterType == CharacterType.Worker)
				list.Add("Work Speed: " + AttackSpeed[level - 1]);
			else
				list.Add("Attack Speed: " + AttackSpeed[level - 1]);
		}
		if (MoveSpeed[level-1] != 0)
			list.Add("Movement Speed: " + MoveSpeed[level - 1]);
		if (AttackRange != 0)
			list.Add("Attack Range: " + AttackRange);
		if (cardCost[level - 1] != 0)
			list.Add("Cost: " + cardCost[level - 1]);
		if (spawnCooldown[level - 1] != 0)
			list.Add("Cooldown: " + spawnCooldown[level - 1] + "s");
		if (AttackType != AttackType.None)
			list.Add("Attack Type: " + AttackType);

		return list;
	}
}
