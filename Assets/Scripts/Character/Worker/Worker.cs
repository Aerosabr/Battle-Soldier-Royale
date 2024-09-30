using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Character
{
    private const int IS_IDLE = 0;
    private const int IS_WALKING = 1;
    private const int IS_DEAD = 2;

    private enum State
    {
        Idle,
        Walking,
        Mining,
        Dead
    }
 
    [SerializeField] private WorkerVisual anim;

    private State state;
    private float miningTimer;
    private float time;
    private void Awake()
    {
        characterType = CharacterType.Worker;
        state = State.Idle;
        miningTimer = 0;
        time = Time.time;
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
                DetectWorkable();
                break;
            case State.Mining:
                miningTimer += Time.deltaTime;
                if (miningTimer >= attackSpeed)
                    FinishMining();
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

    private void DetectWorkable()
    {
        float detectDistance = .2f;
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward * detectDistance, Color.green);
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out RaycastHit hit, detectDistance, targetLayer))
        {
            if (hit.collider.gameObject.tag == "Mine")
            {
                StartMining();
                transform.rotation = Quaternion.Euler(0, -transform.localEulerAngles.y, 0);
            }
            else if (hit.collider.gameObject.tag == "Base")
            {
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
        anim.AnimAction(IS_WALKING);
    }

    private void FinishMining()
    {
        state = State.Idle;
        miningTimer = 0f;
        GetComponent<BoxCollider>().enabled = true;
        anim.gameObject.SetActive(true);
        anim.AnimAction(IS_WALKING);
    }

    private void Deposit()
    {
        // transform.rotation = Quaternion.Euler(0, -transform.rotation.y, 0);
        if (player.playerColor == Player.PlayerColor.Blue)
            PlayerBlue.Instance.AddGold(attack);
        else
            PlayerRed.Instance.AddGold(attack);
    }

    public override void Damaged(int damage)
    {
        currentHealth -= damage;
        DamageVisuals(damage);
		if (currentHealth <= 0)
        {
            anim.AnimAction(IS_DEAD);
            state = State.Dead;
            GetComponent<BoxCollider>().enabled = false;
            player.RemoveFromEconomy(gameObject, true);
        }
    }

	public override void InitializeCharacter(LayerMask layerMask, Vector3 rotation, CardSO card)
    {
        this.card = card;
        this.card.OnLevelChanged += Card_OnLevelChanged;
        baseAttackSpeed = attackSpeed;
        baseMoveSpeed = moveSpeed;
        anim.ActivateEvolutionVisual(card.level);
        SetStats();
        gameObject.transform.rotation = Quaternion.Euler(rotation);
        gameObject.layer = layerMask;
        if (layerMask == 6)
        {
            player = PlayerBlue.Instance;
            targetLayer = 1 << 6 | 1 << 8;
        }
        else if (layerMask == 7)
        {
            player = PlayerRed.Instance;
            targetLayer = 1 << 7 | 1 << 8;
        }
        player.AddToEconomy(gameObject, true);
    }

    private void SetStats()
    {
        if (maxHealth < card.evolutionStats[card.level - 1].Health)
        {
            currentHealth += card.evolutionStats[card.level - 1].Health - maxHealth;
            maxHealth = card.evolutionStats[card.level - 1].Health;

            attack = card.evolutionStats[card.level - 1].Attack;
        }
    }

    private void Card_OnLevelChanged(object sender, EventArgs e)
    {
        anim.ActivateEvolutionVisual(card.level);
        SetStats();
    }
}
