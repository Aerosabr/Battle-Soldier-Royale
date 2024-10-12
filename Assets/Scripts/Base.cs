using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Entity, IDamageable
{
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;
    public event EventHandler<IDamageable.OnDamageTakenEventArgs> OnDamageTaken;

    [SerializeField] private Player.PlayerColor playerColor;

    private void Start()
    {
        healthBarUI.SetColor(playerColor);
    }

    public void Damaged(float damage, CardSO.CardType cardType)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthPercentage = currentHealth / maxHealth
        });
		OnDamageTaken?.Invoke(this, new IDamageable.OnDamageTakenEventArgs
		{
			damage = damage
		});
		if (currentHealth <= 0)
            StartCoroutine(Died());
    }

    private IEnumerator Died()
    {
        gameObject.layer = 0;
        GameEnded.Instance.EndGame(playerColor);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
