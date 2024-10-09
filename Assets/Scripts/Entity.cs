using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int maxHealth;

    [SerializeField] protected HealthBarUI healthBarUI;
    protected Player player;
    protected LayerMask targetLayer;

    public int GetCurrentHealth() => currentHealth;
    public LayerMask GetTargetLayer() => targetLayer;
}
