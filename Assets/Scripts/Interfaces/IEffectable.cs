using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable
{
	public void Poisoned(float damage, float poisonDuration);
	public void ReduceAttack(float damageReduce);
	public void UnReduceAttack();
	public void Slowed(int speed);
	public void UnSlowed(int speed);
}
