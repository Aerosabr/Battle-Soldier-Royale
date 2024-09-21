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
    public event EventHandler<IDamageable.OnDamageTakenEventArgs> OnDamageTaken;

    private enum State
    {
        None,
        Idle,
        Building,
        Attacking,
        Destroyed
    }

    private State state;
    [SerializeField] private ArcherTowerVisual archerTowerVisual;
    [SerializeField] private Animator buildingAnim;

    private int buildingProgress = 0;
    private bool canAttack = true;

    private void Awake()
    {
        state = State.None;
    }

    private void Update()
    {
        switch(state)
        {
            case State.None:
                break;
            case State.Idle:
                DetectEnemies();
                break;
            case State.Building:
                Building();
                break;
            case State.Attacking:
                DetectEnemies();
                break;
            case State.Destroyed:
                transform.position += new Vector3(0, -0.01f, 0);
                if (transform.position.y <= -4)
                {
                    buildingSlot.RemoveBuilding();
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void Building()
    {
        currentHealth += (int)((maxHealth / buildTimer) * Time.deltaTime);
        float hpPercent = (float)currentHealth / maxHealth;
        OnHealthChanged?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthPercentage = hpPercent
        });
		OnDamageTaken?.Invoke(this, new IDamageable.OnDamageTakenEventArgs
		{
			damage = 0
		});
		int progress = 0;
        for (int i = 1; i <= archerTowerVisual.GetEvolutionVisual(card.level).bodyParts.Count; i++)
        {
            if (hpPercent >= (i / (float)archerTowerVisual.GetEvolutionVisual(card.level).bodyParts.Count))
                progress++;
        }

        if (progress != buildingProgress)
        {
            buildingProgress = progress;
            if (progress == archerTowerVisual.GetEvolutionVisual(card.level).bodyParts.Count)
                archerTowerVisual.BuildingCompleted(true);

            archerTowerVisual.ChangeBuildingVisual(card.level, buildingProgress);
        }

        if (currentHealth >= maxHealth)
        {
            state = State.Idle;
            currentHealth = maxHealth;
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
        {
            //archerTowerVisual.AnimAction(IS_DESTROYED);
            archerTowerVisual.BuildingCompleted(false);
            archerTowerVisual.ChangeBuildingVisual(card.level, archerTowerVisual.GetEvolutionVisual(card.level).bodyParts.Count);
            state = State.Destroyed;
            GetComponent<BoxCollider>().enabled = false;
            player.RemoveFromMilitary(gameObject);
            buildingAnim.SetTrigger("Destroyed");
        }
    }

    private void DetectEnemies()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward * attackRange, Color.green);

        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, attackRange, targetLayer))
        {
            if (state == State.Idle)
            {
                state = State.Attacking;
                archerTowerVisual.AnimAction(IS_IDLE);
            }
            else if (canAttack)
            {
                StartCoroutine(ChargeAttack());
                archerTowerVisual.AnimAction(IS_ATTACKING);
            }
        }
        else
        {
            state = State.Idle;
            archerTowerVisual.AnimAction(IS_IDLE);
        }
    }

    private IEnumerator ChargeAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f / attackSpeed);
        canAttack = true;
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
        else if (state != State.Destroyed)
            state = State.Idle;
    }

    public override void InitializeBuilding(LayerMask layerMask, CardSO card, BuildingSlot buildingSlot)
    {
        this.card = card;
        this.card.OnLevelChanged += Card_OnLevelChanged;
        maxHealth = evolutionStats[card.level - 1].Health;
        attack = evolutionStats[card.level - 1].Attack;
        state = State.Building;
        gameObject.layer = layerMask;
        this.buildingSlot = buildingSlot;
        buildingSlot.SetBuilding(this);
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

    private void Card_OnLevelChanged(object sender, EventArgs e)
    {
        if (buildingProgress != 0)
            archerTowerVisual.ChangeBuildingVisual(card.level, buildingProgress);
        SetStats();
    }

    private void SetStats()
    {
        if (maxHealth < evolutionStats[card.level - 1].Health)
        {
            currentHealth += evolutionStats[card.level - 1].Health - maxHealth;
            maxHealth = evolutionStats[card.level - 1].Health;

            attack = evolutionStats[card.level - 1].Attack;
        }
    }
}
