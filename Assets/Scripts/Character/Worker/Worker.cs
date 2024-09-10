using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Character, IDamageable
{
    private const int IS_IDLE = 0;
    private const int IS_WALKING = 1;
    private const int IS_ATTACKING = 2;

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;

    private enum State
    {
        Idle,
        Walking,
        Mining,
        Dead
    }
 
    [SerializeField] private WorkerVisual anim;
    [SerializeField] private int mineYield = 10;

    private State state;
    private float miningTimer;
    private float miningTimerMax = 5f;

    private void Start()
    {
        state = State.Idle;
        miningTimer = 0;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                state = State.Walking;
                anim.AnimAction("isWalking", true);
                break;
            case State.Walking:
                Movement();
                DetectWorkable();
                break;
            case State.Mining:
                miningTimer += Time.deltaTime;
                if (miningTimer >= miningTimerMax)
                    FinishMining();
                break;
        }
    }

    private void Movement()
    {
        float moveDistance = moveSpeed * Time.deltaTime;

        transform.position += transform.forward * moveDistance;
    }

    private void DetectWorkable()
    {
        float detectDistance = .2f;
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward * detectDistance, Color.green);
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out RaycastHit hit, detectDistance, targetLayer))
        {
            if (hit.collider.gameObject.tag == "Mine")
                StartMining();
            else if (hit.collider.gameObject.tag == "Base")
                Deposit();

            transform.rotation = Quaternion.Euler(0, -transform.localEulerAngles.y, 0);
        }
    }

    private void StartMining()
    {
        Debug.Log("Started mining");
        state = State.Mining;
        GetComponent<BoxCollider>().enabled = false;
        anim.gameObject.SetActive(false);
        anim.AnimAction("isWalking", false);
    }

    private void FinishMining()
    {
        Debug.Log("Finished mining");
        state = State.Idle;
        miningTimer = 0f;
        GetComponent<BoxCollider>().enabled = true;
        anim.gameObject.SetActive(true);
        anim.AnimAction("isWalking", true);
    }

    private void Deposit()
    {
        Debug.Log("Deposited");
        // transform.rotation = Quaternion.Euler(0, -transform.rotation.y, 0);
        PlayerManager.Instance.AddGold(mineYield);
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
        if (layerMask == 6)
        {
            Debug.Log("temp");
            targetLayer = 1 << 6 | 1 << 8;
        }
        else if (layerMask == 7)
        {
            Debug.Log("temp2");
            targetLayer = 1 << 7 | 1 << 8;
        }
    }
}
