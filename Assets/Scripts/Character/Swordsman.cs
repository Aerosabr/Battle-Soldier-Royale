using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Swordsman : Character
{
    private bool Movable;
    private bool isWalking;
    private bool isAttacking;

    [SerializeField] private CharacterAnimator anim;

    private void Start()
    {
        Movable = true;
    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        Vector3 moveDir = new Vector3(1, 0f, 0).normalized;     
        float moveDistance = moveSpeed * Time.deltaTime;

        if (Movable)
            transform.position += moveDir * moveDistance;

        if (isWalking != Movable)
        {
            isWalking = Movable;
            anim.AnimAction("isWalking", isWalking);
        }
        
    }

    private void HandleAttack()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), Color.green);
        bool canMove = !Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), out RaycastHit hit, 1);
        if (isAttacking == canMove)
        {
            isAttacking = !canMove;
            anim.AnimAction("isAttacking", isAttacking);
            if (isAttacking)
            {
                Movable = false;
                Debug.Log(hit.collider.gameObject.name);
            }
            else
                Movable = true;
        }
    }
}
