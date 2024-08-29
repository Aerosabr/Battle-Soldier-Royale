using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour
{
    protected int currentHealth;
    protected int maxHealth;
    protected float moveSpeed = 1f;
    protected int targetLayer;

    public virtual void InitializeCharacter(LayerMask layerMask, Vector3 rotation) => Debug.Log("Initialize not implemented");
    public int GetCurrentHealth() => currentHealth;
}
