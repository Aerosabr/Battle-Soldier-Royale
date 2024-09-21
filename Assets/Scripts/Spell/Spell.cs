using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spell : MonoBehaviour
{
	[SerializeField] protected string spellName;
	[SerializeField] protected int damage;
	[SerializeField] protected int cost;
	[SerializeField] protected int targetLayer;
	[SerializeField] protected Player player;

	[SerializeField] protected BoxCollider hitBox;
	[SerializeField] protected Transform transparentObject;
	[SerializeField] protected Transform visualObject;
	[SerializeField] protected List<Character> characters = new List<Character>();

	public virtual IEnumerator Project(int layer, int damage, int cost){	yield return false;   }

}
