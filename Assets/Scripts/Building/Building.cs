using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BuildingType
{
    Farm,
    Defense
}

[Serializable]
public struct BuildingStats
{
    public int Health;
    public int Attack;
}

public class Building : Entity
{
    [SerializeField] protected int attack;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float buildTimer;

    [SerializeField] protected List<BuildingStats> evolutionStats;
    [SerializeField] protected LayerMask targetLayer;
    public BuildingType buildingType;
    protected Player player;
    protected CardSO card;
    protected BuildingSlot buildingSlot;

    public virtual void InitializeBuilding(LayerMask layerMask, CardSO card, BuildingSlot buildingSlot) => Debug.Log("Initialize not implemented");
    protected virtual void BuildingBuilt() => Debug.Log("Built not implemented");
    protected virtual void BuildingDestroyed() => Debug.Log("Destroyed not implemented");
    public int GetAttack() => attack;
}
