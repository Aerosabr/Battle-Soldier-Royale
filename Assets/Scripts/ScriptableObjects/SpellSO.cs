using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SpellSO : ScriptableObject
{
	public Transform spell;
	public Sprite background;
	public int upgradeCost;
}
