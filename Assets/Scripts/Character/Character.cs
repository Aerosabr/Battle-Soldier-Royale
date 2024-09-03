using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour
{
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int attack;
    [SerializeField] protected int cost;

    protected float moveSpeed = 1f;
    [SerializeField] protected LayerMask targetLayer;

    public virtual void InitializeCharacter(LayerMask layerMask, Vector3 rotation) => Debug.Log("Initialize not implemented");
    public int GetCurrentHealth() => currentHealth;
    public int GetCost() => cost;
}
