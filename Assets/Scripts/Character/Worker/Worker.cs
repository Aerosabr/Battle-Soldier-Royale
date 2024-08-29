using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Character, IDamageable
{
    private enum State
    {
        Idle,
        Walking,
        Mining,
        Hit
    }

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;
    [SerializeField] private WorkerVisual anim;
    [SerializeField] private LayerMask miningLayer;
    private bool Movable;
    private State state;

    private float miningTimer;
    private float miningTimerMax = 5f;

    private void Start()
    {
        Movable = true;
        currentHealth = 1000;
        maxHealth = 1000;
        state = State.Idle;
        miningTimer = 0;
        Debug.Log((int)miningLayer);
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
                HandleMovement();
                break;
            case State.Mining:
                miningTimer += Time.deltaTime;
                if (miningTimer >= miningTimerMax)
                    FinishMining();
                break;
        }
    }

    private void HandleMovement()
    {
        float moveDistance = moveSpeed * Time.deltaTime;

        if (Movable)
        {
            transform.position += transform.forward * moveDistance;
            Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward * .2f, Color.green);
            if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out RaycastHit hit, .2f, miningLayer))
            {
                if (hit.collider.gameObject.tag == "Mine")
                    StartMining();
                else if (hit.collider.gameObject.tag == "Base")
                    Deposit();

                transform.rotation = Quaternion.Euler(0, -transform.localEulerAngles.y, 0);
            }
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
        PlayerManager.Instance.AddGold(20);
    }

    public void Attack01()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out RaycastHit hit, 1))
        {
            if (hit.transform.GetComponent<Character>().GetCurrentHealth() > 0)
                hit.transform.GetComponent<IDamageable>().Damaged(10);
        }
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
    }
}
