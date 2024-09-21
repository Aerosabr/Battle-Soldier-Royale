using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IDamageable
{
    public void Damaged(int damage);

    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
    public class OnHealthChangedEventArgs : EventArgs
    {
        public float healthPercentage;
    }
    
    public event EventHandler<OnDamageTakenEventArgs> OnDamageTaken;
    public class OnDamageTakenEventArgs : EventArgs
    {
        public int damage;
    }
}
