using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoisonable
{
	public void Poisoned(int damage, int poisonDuration);
}
