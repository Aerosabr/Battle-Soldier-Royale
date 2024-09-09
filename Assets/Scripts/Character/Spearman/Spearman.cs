using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman : Character, IDamageable
{
    private const int IS_IDLE = 0;
    private const int IS_WALKING = 1;
    private const int IS_ATTACKING = 2;

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;

    private enum State
    {
        Idle,
        Walking,
        Attacking,
        Dead
    }

    [SerializeField] private SpearmanVisual anim;
    [SerializeField] private State state;

    private bool canAttack = true;
    private float attackSpeed = 1.5f;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                state = State.Walking;
                anim.AnimAction(IS_WALKING);
                break;
            case State.Walking:
                Movement();
                DetectEnemies();
                break;
            case State.Attacking:
                DetectEnemies();
                break;
            case State.Dead:

                break;
        }
    }

    private void Movement()
    {
        float moveDistance = moveSpeed * Time.deltaTime;

        transform.position += transform.forward * moveDistance;
    }

    private void DetectEnemies()
    {
        float attackRange = 1.5f;
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward, Color.green);

        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, attackRange, targetLayer))
        {
            if (state == State.Walking)
            {
                state = State.Attacking;
                anim.AnimAction(IS_IDLE);
            }
            else if (canAttack)
            {
                StartCoroutine(ChargeAttack());
                anim.AnimAction(IS_ATTACKING);
            }
        }
        else
        {
            state = State.Walking;
            anim.AnimAction(IS_WALKING);
        }
    }

    private IEnumerator ChargeAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }

    public void Attack01()
    {
        float attackRange = 1.5f;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out RaycastHit hit, attackRange, targetLayer))
        {
            if (hit.transform.GetComponent<Character>().GetCurrentHealth() > 0)
            {
                hit.transform.GetComponent<IDamageable>().Damaged(attack);
                anim.AnimAction(IS_IDLE);
            }
        }
        else
            state = State.Walking;
    }

    public void Damaged(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthPercentage = (float)currentHealth / maxHealth
        });

        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    public override void InitializeCharacter(LayerMask layerMask, Vector3 rotation)
    {
        gameObject.transform.rotation = Quaternion.Euler(rotation);
        gameObject.layer = layerMask;
        if (gameObject.layer == 6)
            targetLayer = 1 << 7;
        else
            targetLayer = 1 << 6;
    }
}
