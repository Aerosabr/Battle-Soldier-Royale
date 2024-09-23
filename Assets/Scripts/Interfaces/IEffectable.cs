using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable
{
	public void Poisoned(int damage, int poisonDuration);
	public void Slowed(int speed);
	public void UnSlowed(int speed);
}
