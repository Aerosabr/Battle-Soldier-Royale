using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Character, IDamageable
{
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;

    private bool Movable;
    private bool isWalking;
    private bool isAttacking;

    [SerializeField] private GameObject arrow;
    [SerializeField] private ArcherVisual anim;
    [SerializeField] private GameObject bowPos;

    private void Start()
    {
        Movable = true;
        currentHealth = 1000;
        maxHealth = 1000;
    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        float moveDistance = moveSpeed * Time.deltaTime;

        if (Movable)
            transform.position += transform.forward * moveDistance;

        if (isWalking != Movable)
        {
            isWalking = Movable;
            anim.AnimAction("isWalking", isWalking);
        }

    }

    private void HandleAttack()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward * 3, Color.green);
        bool canMove = !Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, 3, targetLayer);
        if (isAttacking == canMove)
        {
            isAttacking = !canMove;
            anim.AnimAction("isAttacking", isAttacking);
            if (isAttacking)
            {
                Movable = false;
            }
            else
                Movable = true;
        }
    }

    public void Attack01()
    {
        Transform arrowFired = Instantiate(arrow, bowPos.transform.position, transform.rotation).transform;
        arrowFired.GetComponent<Arrow>().InitializeArrow(targetLayer, 10);
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

