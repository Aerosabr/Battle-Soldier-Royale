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

public enum AttackType
{
    None,
    Single,
    AOE
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

    protected bool canAttack = true;
    protected float deathTimer;
    protected float deathTimerMax = 3f;
    protected float moveSpeed = 1f;

    [SerializeField] protected List<CharacterStats> evolutionStats;
    [SerializeField] protected LayerMask targetLayer;
    public CharacterType characterType;
    public AttackType attackType;
    protected Player player;
    protected CardSO card;

    public virtual void InitializeCharacter(LayerMask layerMask, Vector3 rotation, CardSO card) => Debug.Log("Initialize not implemented");
    protected virtual void CharacterSpawned() => Debug.Log("Spawned not implemented");
    protected virtual void CharacterDied() => Debug.Log("Died not implemented");
    public int GetUnitStrength()
    {
        int unitStrength = 0;
        unitStrength += currentHealth / 2;
        switch (attackType)
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
}
