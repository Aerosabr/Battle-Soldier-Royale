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

[Serializable]
public struct CharacterStats
{
    public int Health;
    public int Attack;
}

public class Character : Entity
{
    [SerializeField] protected int attack;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;
	[SerializeField] protected float moveSpeed = 1f;

	protected bool canAttack = true;
    protected float deathTimer;
    protected float deathTimerMax = 3f;
    protected float baseMoveSpeed = 1f;
    protected float baseAttackSpeed = 1.5f;
    protected bool isSlowed = false;
    protected bool isPoisoned = false;
	protected float poisonTimer = 0f;


	[SerializeField] protected List<CharacterStats> evolutionStats;
    [SerializeField] protected LayerMask targetLayer;
    public CharacterType characterType;
    protected Player player;
    protected CardSO card;

    public virtual void InitializeCharacter(LayerMask layerMask, Vector3 rotation, CardSO card) => Debug.Log("Initialize not implemented");
    protected virtual void CharacterSpawned() => Debug.Log("Spawned not implemented");
    protected virtual void CharacterDied() => Debug.Log("Died not implemented");

}
