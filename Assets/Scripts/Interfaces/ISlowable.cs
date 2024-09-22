using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlowable
{
	public void Slowed(int speed);
	public void UnSlowed(int speed);
}
