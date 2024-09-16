using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Character, IDamageable
{
    private const int IS_IDLE = 0;
    private const int IS_WALKING = 1;
    private const int IS_ATTACKING = 2;
    private const int IS_DEAD = 3;

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;

    private enum State
    {
        Idle,
        Walking,
        Attacking,
        Dead
    }

    [SerializeField] private GameObject arrow;
    [SerializeField] private ArcherVisual anim;
    [SerializeField] private GameObject bowPos;
    [SerializeField] private State state;

    private bool canAttack = true;
    private float attackSpeed = 2f;
    private float attackRange = 4f;
    private float deathTimer;
    private float deathTimerMax = 3;

    private void Awake()
    {
        characterType = CharacterType.Ranged;
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
                deathTimer += Time.deltaTime;
                if (deathTimer >= deathTimerMax)
                    Destroy(gameObject);
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
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward * attackRange, Color.green);

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
        Transform arrowFired = Instantiate(arrow, bowPos.transform.position, transform.rotation).transform;
        arrowFired.GetComponent<Arrow>().InitializeArrow(targetLayer, attack);
        anim.AnimAction(IS_IDLE);
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
            anim.AnimAction(IS_DEAD);
            state = State.Dead;
            GetComponent<BoxCollider>().enabled = false;
            player.RemoveFromMilitary(gameObject);
        }
    }

    public override void InitializeCharacter(LayerMask layerMask, Vector3 rotation)
    {
        gameObject.transform.rotation = Quaternion.Euler(rotation);
        gameObject.layer = layerMask;
        if (gameObject.layer == 6)
        {
            player = PlayerBlue.Instance;
            targetLayer = 1 << 7;
        }
        else
        {
            player = PlayerRed.Instance;
            targetLayer = 1 << 6;
        }
        player.AddToMilitary(gameObject);
    }
}

