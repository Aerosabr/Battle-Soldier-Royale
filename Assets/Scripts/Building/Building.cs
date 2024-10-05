using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Entity, IDamageable
{
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;
    public event EventHandler<IDamageable.OnDamageTakenEventArgs> OnDamageTaken;

    protected int baseAttack;
    protected int attack;
    protected float baseAttackSpeed;
    protected float attackSpeed;
    protected float attackRange;
    protected float buildTimer;

    protected Player player;
    protected BuildingCardSO card;
    protected BuildingSlot buildingSlot;
    protected LayerMask targetLayer;

    public virtual void Damaged(int damage) { }
    protected void HealthChangedVisual()
    {
        OnHealthChanged?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthPercentage = (float)currentHealth / maxHealth
        });
    }
    protected void DamageTakenVisual(int damage)
    {
        OnDamageTaken?.Invoke(this, new IDamageable.OnDamageTakenEventArgs
        {
            damage = damage
        });
    }

    public virtual void InitializeBuilding(LayerMask layerMask, CardSO card, BuildingSlot buildingSlot) => Debug.Log("Initialize not implemented");
    public virtual IEnumerator Project(LayerMask layerMask, CardSO card) { yield return null; }
    protected virtual void BuildingBuilt() => Debug.Log("Built not implemented");
    protected virtual void BuildingDestroyed() => Debug.Log("Destroyed not implemented");
    public int GetAttack() => attack;
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
    public BuildingCardSO GetCard() => card;
}
