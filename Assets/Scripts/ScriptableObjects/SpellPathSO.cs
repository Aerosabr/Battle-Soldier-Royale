using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SpellPathSO : ScriptableObject
{
	public List<SpellSO> spellSO;
	public GameObject upgradesTab;
}
