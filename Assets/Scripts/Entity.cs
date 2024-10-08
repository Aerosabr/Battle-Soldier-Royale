using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int maxHealth;

    protected LayerMask targetLayer;

    public int GetCurrentHealth() => currentHealth;
    public LayerMask GetTargetLayer() => targetLayer;
}
