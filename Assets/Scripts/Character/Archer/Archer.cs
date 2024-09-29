using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Character, IDamageable, IEffectable
{
    private const int IS_IDLE = 0;
    private const int IS_WALKING = 1;
    private const int IS_ATTACKING = 2;
    private const int IS_DEAD = 3;

    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;
    public event EventHandler<IDamageable.OnDamageTakenEventArgs> OnDamageTaken;

    private enum State
    {
        Idle,
        Walking,
        Attacking,
        Dead
    }

    [SerializeField] private ArcherVisual anim;
    private State state;

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

        OnDamageTaken?.Invoke(this, new IDamageable.OnDamageTakenEventArgs
        {
            damage = damage
        });

        if (currentHealth <= 0)
        {
            anim.AnimAction(IS_DEAD);
            state = State.Dead;
            GetComponent<BoxCollider>().enabled = false;
            player.RemoveFromMilitary(gameObject);
        }
    }

	#region IEffectable Handler
	public void Slowed(int speed)
	{
		if (!isSlowed)
		{
			isSlowed = true;
			moveSpeed = moveSpeed - ((float)speed / 50);
			attackSpeed = attackSpeed - ((float)speed / 50);
		}
	}
	public void UnSlowed(int speed)
	{
		if (isSlowed)
		{
			isSlowed = false;
			moveSpeed = baseMoveSpeed;
			attackSpeed = baseAttackSpeed;
		}
	}

	public void Poisoned(int damage, int poisonDuration)
	{
		if (!isPoisoned)
		{
			StartCoroutine(HandlePoisonDamage(damage, poisonDuration));
		}
		else
		{
			poisonTimer = 0f;
		}
	}
	private IEnumerator HandlePoisonDamage(int damage, float duration)
	{
		isPoisoned = true;
		float poisonDamageInterval = 1f;
		while (poisonTimer < duration)
		{
			Damaged(damage);
			yield return new WaitForSeconds(poisonDamageInterval);
			poisonTimer += poisonDamageInterval;
		}
		isPoisoned = false;
	}
	#endregion

	public override void InitializeCharacter(LayerMask layerMask, Vector3 rotation, CardSO card)
    {
        this.card = card;
        this.card.OnLevelChanged += Card_OnLevelChanged;
        baseMoveSpeed = moveSpeed;
        baseAttackSpeed = attackSpeed;
        anim.ActivateEvolutionVisual(card.level);
        SetStats();
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

    private void Card_OnLevelChanged(object sender, EventArgs e)
    {
        anim.ActivateEvolutionVisual(card.level);
        SetStats();
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
}

