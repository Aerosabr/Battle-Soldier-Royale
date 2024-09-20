using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
	[SerializeField] protected string spellName;
	[SerializeField] protected float damage;
	[SerializeField] protected float duration;
	[SerializeField] protected bool isPlaced = false;
	[SerializeField] protected int targetLayer = 6;

	[SerializeField] protected BoxCollider hitBox;
	[SerializeField] protected Transform transparentObject;
	[SerializeField] protected Transform visualObject;
	[SerializeField] protected List<Character> characters = new List<Character>();

	public virtual IEnumerator Project(int layer){	yield return false;   }

	public virtual bool Placed(bool isPlaced){	return false;	}
}
