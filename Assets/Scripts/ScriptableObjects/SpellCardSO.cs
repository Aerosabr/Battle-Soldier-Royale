using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType
{
    Immediate,
    Poison,
    Slow,
}


[CreateAssetMenu()]
public class SpellCardSO : CardSO
{
    public List<float> Attack;
    public float Size;
    public float Duration;
    public float PostSpellDuration;
	public SpellType SpellType;

	public override List<string> ViewCard(int level)
	{
		List<string> list = new List<string>();

		if (Attack[level - 1] != 0)
		{
			if (SpellType == SpellType.Immediate)
				list.Add("Damage: " + Attack[level - 1]);
			else if (SpellType == SpellType.Poison)
			{
				list.Add("Damage per second: " + Attack[level - 1]);
				list.Add("Attack Damage reduce: -" + ((float)Attack[level - 1]).ToString("F2") + "%");
			}
			else if (SpellType == SpellType.Slow)
			{
				list.Add("Movement reduce: -" + ((float)Attack[level - 1]).ToString("F2") + "%");
				list.Add("Attack Speed reduce: -" + ((float)Attack[level - 1]).ToString("F2") + "%");
			}
		}
		if (cardCost[level - 1] != 0)
			list.Add("Cost: " + cardCost[level - 1]);
		if (Size != 0)
			list.Add("Size: " + Size);
		if (Duration != 0)
			list.Add("Duration: " + Duration + "s");
		if (PostSpellDuration != 0)
			list.Add("Poison Duration: " + PostSpellDuration + "s");
		if (spawnCooldown[level - 1] != 0)
			list.Add("Cooldown: " + spawnCooldown[level - 1] + "s");

		return list;
	}

}

