using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CharacterType
{
    Worker,
    Melee,
    Ranged
}

public class Character : Entity
{
    [SerializeField] protected int attack;
    [SerializeField] protected int cost;

    protected float moveSpeed = 1f;
    [SerializeField] protected LayerMask targetLayer;
    public CharacterType characterType;
    protected Player player;

    public virtual void InitializeCharacter(LayerMask layerMask, Vector3 rotation) => Debug.Log("Initialize not implemented");
    public int GetCost() => cost;
    protected virtual void CharacterSpawned() => Debug.Log("Spawned not implemented");
    protected virtual void CharacterDied() => Debug.Log("Died not implemented");
}
