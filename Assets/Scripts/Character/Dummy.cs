using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character
{
    public override void Damaged(int damage)
    {
        currentHealth -= damage;
        DamageVisuals(damage);
        Debug.Log("Dummy took " + damage + " damage!");
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
