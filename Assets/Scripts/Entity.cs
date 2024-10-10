using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float maxHealth;

    [SerializeField] protected HealthBarUI healthBarUI;
    protected Player player;
    protected LayerMask targetLayer;

    public float GetCurrentHealth() => currentHealth;
    public LayerMask GetTargetLayer() => targetLayer;
}
