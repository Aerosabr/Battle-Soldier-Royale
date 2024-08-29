using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int targetLayer;
    private int damage;

    private void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        float moveDistance = 4 * Time.deltaTime;
        transform.position += transform.forward * moveDistance;
    }

    private void HandleAttack()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1, targetLayer))
        {
            Debug.Log("Hit");
            if (hit.transform.GetComponent<Character>().GetCurrentHealth() > 0)
            {
                hit.transform.GetComponent<IDamageable>().Damaged(10);
                Destroy(gameObject);
            }
        }
    }

    public void InitializeArrow(int layer, int damage)
    {
        targetLayer = layer;
        this.damage = damage;
    }
}
