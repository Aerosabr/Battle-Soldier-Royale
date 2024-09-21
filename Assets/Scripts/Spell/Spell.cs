using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spell : MonoBehaviour
{
	[SerializeField] protected string spellName;
	[SerializeField] protected int damage;
	[SerializeField] protected int cost;
	[SerializeField] protected float duration;
	[SerializeField] protected int targetLayer = 6;

	[SerializeField] protected BoxCollider hitBox;
	[SerializeField] protected GameObject damageTextObject;
	[SerializeField] protected Transform transparentObject;
	[SerializeField] protected Transform visualObject;
	[SerializeField] protected List<Character> characters = new List<Character>();

	public virtual IEnumerator Project(int layer, int damage, int cost){	yield return false;   }

}
