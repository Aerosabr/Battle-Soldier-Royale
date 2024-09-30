using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BuildingType
{
    Farm,
    Defense
}

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
    public BuildingType buildingType;
    public AttackType attackType;

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
    protected virtual void BuildingBuilt() => Debug.Log("Built not implemented");
    protected virtual void BuildingDestroyed() => Debug.Log("Destroyed not implemented");
    public int GetAttack() => attack;
}
