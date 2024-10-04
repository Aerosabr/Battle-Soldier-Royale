using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : Entity, IDamageable, IEffectable
{
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;
    public event EventHandler<IDamageable.OnDamageTakenEventArgs> OnDamageTaken;

    protected int baseAttack;
    [SerializeField] protected int attack;
    protected float baseAttackSpeed;
    protected float attackSpeed;
    protected float baseMoveSpeed;
    protected float moveSpeed;

    protected float attackRange;
    protected float deathTimer;
    protected float deathTimerMax = 3f;
    protected float poisonTimer = 0f;

    protected bool canAttack = true;
    protected bool isSlowed = false;
    protected bool isPoisoned = false;

    protected Player player;
    protected CharacterCardSO card;
    protected LayerMask targetLayer;

    public virtual void InitializeCharacter(LayerMask layerMask, Vector3 rotation, CardSO card) => Debug.Log("Initialize not implemented");
    public int GetAttack() => attack;
    public CharacterCardSO GetCard() => card;
    public int GetUnitStrength()
    {
        int unitStrength = 0;
        unitStrength += currentHealth / 2;
        switch (card.AttackType)
        {
            case AttackType.None:
            case AttackType.Single:
                unitStrength += (int)(attack * attackSpeed * attackRange);
                break;
            case AttackType.AOE:
                unitStrength += (int)(attack * attackSpeed * attackRange * 2);
                break;
        }

        return unitStrength;
    }

    #region IDamageable Components
    public virtual void Damaged(int damage) { }
    protected void DamageVisuals(int damage)
    {
        OnHealthChanged?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthPercentage = (float)currentHealth / maxHealth
        });
        /*
        OnDamageTaken?.Invoke(this, new IDamageable.OnDamageTakenEventArgs
        {
            damage = damage
        });
        */
    }
    #endregion

    #region IEffectable Components
    public void Slowed(int speed)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            moveSpeed = moveSpeed - ((float)speed / 50);
            attackSpeed = attackSpeed - ((float)speed / 50);
        }
    }
    public void UnSlowed(int speed)
    {
        if (isSlowed)
        {
            isSlowed = false;
            moveSpeed = baseMoveSpeed;
            attackSpeed = baseAttackSpeed;
        }
    }

    public void Poisoned(int damage, int poisonDuration)
    {
        if (!isPoisoned)
        {
            StartCoroutine(HandlePoisonDamage(damage, poisonDuration));
        }
        else
        {
            poisonTimer = 0f;
        }
    }
    private IEnumerator HandlePoisonDamage(int damage, float duration)
    {
        isPoisoned = true;
        float poisonDamageInterval = 1f;
        while (poisonTimer < duration)
        {
            Damaged(damage);
            yield return new WaitForSeconds(poisonDamageInterval);
            poisonTimer += poisonDamageInterval;
        }
        isPoisoned = false;
    }
    #endregion
}
