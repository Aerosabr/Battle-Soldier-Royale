using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character, IDamageable
{
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;

    private void Start()
    {

    }

    public void Damaged(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthPercentage = (float)currentHealth / maxHealth
        });

        if (currentHealth <= 0)
            StartCoroutine(Died());
    }

    private IEnumerator Died()
    {
        gameObject.layer = 0;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
