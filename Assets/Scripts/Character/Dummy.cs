using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character, IDamageable
{
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;

    private void Start()
    {
        currentHealth = 100;
        maxHealth = 100;
    }

    public void Damaged(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthPercentage = (float)currentHealth / maxHealth
        });

        if (currentHealth <= 0)
            Destroy(gameObject);
    }
}
