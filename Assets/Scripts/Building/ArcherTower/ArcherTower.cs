using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : Building, IDamageable
{
    private const int IS_IDLE = 0;
    private const int IS_BUILDING = 1;
    private const int IS_ATTACKING = 2;
    private const int IS_DESTROYED = 3;

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;

    private enum State
    {
        Idle,
        Building,
        Attacking,
        Destroyed
    }

    private State state;
    [SerializeField] private ArcherTowerVisual archerTowerVisual;

    private void Awake()
    {
        state = State.Building;
    }

    private void Update()
    {
        switch(state)
        {
            case State.Idle:

                break;
            case State.Building:

                break;
            case State.Attacking:

                break;
            case State.Destroyed:

                break;
        }
    }

    public void Attack01()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out RaycastHit hit, attackRange, targetLayer))
        {
            if (hit.transform.GetComponent<Entity>().GetCurrentHealth() > 0)
            {
                hit.transform.GetComponent<IDamageable>().Damaged(attack);
                archerTowerVisual.AnimAction(IS_IDLE);
            }
        }
        else
            state = State.Idle;
    }

    public void Damaged(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthPercentage = (float)currentHealth / maxHealth
        });

        if (currentHealth <= 0)
        {
            //archerTowerVisual.AnimAction(IS_DESTROYED);
            state = State.Destroyed;
            GetComponent<BoxCollider>().enabled = false;
            player.RemoveFromMilitary(gameObject);
        }
    }
}
