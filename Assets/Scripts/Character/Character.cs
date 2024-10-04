using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;


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
    public virtual IEnumerator Project(LayerMask layerMask, Vector3 rotation, CardSO card) { yield return null; }
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

	protected bool IsMouseOverUI()
	{
		Vector3[] corners = CharacterBarUI.Instance.GetCancelArea();
		if (corners == null)
		{
			return false;
		}
		Vector3 mousePosition = Input.mousePosition;
		if (mousePosition.x >= corners[0].x && mousePosition.x <= corners[2].x && mousePosition.y >= corners[0].y && mousePosition.y <= corners[2].y)
		{
			return true;
		}

		return false;
	}

	protected bool IsCharacterInSpawnArea()
	{
		float distanceFromFurthest = 1.75f;
		if (player.transform.position.x < 0)
		{
			if (transform.position.x > player.transform.position.x && transform.position.x < (player.transform.position.x + player.GetFurthestControlledArea() - distanceFromFurthest))
				return true;
			else
				return false;
		}
		else
		{
			if (transform.position.x > player.transform.position.x && transform.position.x < (player.transform.position.x - player.GetFurthestControlledArea() - distanceFromFurthest))
				return true;
			else
				return false;
		}
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
