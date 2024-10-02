using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Archer : Character
{
    public enum State
    {
        Ghost,
        Idle,
        Walking,
        Attacking,
        Dead
    }

    [SerializeField] private ArcherVisual anim;
    [SerializeField] private ArcherSound sound;
    private State state;

    private void Awake()
    {
        characterType = CharacterType.Ranged;
        state = State.Ghost;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                DetectEnemies();
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

    private void ChangeState(State newState)
    {
        if (state == State.Dead || state == newState)
            return;

        state = newState;
        anim.AnimAction(state);
    }

    private void Movement()
    {
        float moveDistance = moveSpeed * Time.deltaTime;

        transform.position += transform.forward * moveDistance;
    }

    private void DetectEnemies()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward, Color.green);

        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, attackRange, targetLayer))
        {
            if (canAttack)
            {
                ChangeState(State.Attacking);
                StartCoroutine(ChargeAttack());
            }
            else
                ChangeState(State.Idle);
        }
        else
            ChangeState(State.Walking);
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
                sound.Attack();
                hit.transform.GetComponent<IDamageable>().Damaged(attack);
            }
        }
        else
            ChangeState(State.Idle);
    }

    public override void Damaged(int damage)
    {
        currentHealth -= damage;
        DamageVisuals(damage);
        sound.Damaged();
        if (currentHealth <= 0)
        {
            GetComponent<BoxCollider>().enabled = false;
            ChangeState(State.Dead);
            sound.Died();
            player.RemoveFromMilitary(gameObject);
        }
    }

    public override void InitializeCharacter(LayerMask layerMask, Vector3 rotation, CardSO card)
    {
        this.card = card as CharacterCardSO;
        this.card.OnLevelChanged += Card_OnLevelChanged;
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
    }

	public override IEnumerator Project(LayerMask layerMask, Vector3 rotation, CardSO card)
	{
		float cameraDistance = 0.75f;
		InitializeCharacter(layerMask, rotation, card);
		while (Mouse.current.leftButton.isPressed)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				Vector3 worldPosition = hit.point;
				transform.position = new Vector3(worldPosition.x, transform.position.y, cameraDistance);
			}
			yield return null;
		}

		if (IsMouseOverUI())
		{
			Destroy(gameObject);
		}
		else
		{
            CharacterBarUI.Instance.ActivateCooldown();
            player.SubtractGold(card.cardCost[card.level - 1]);
			player.AddToMilitary(gameObject);
            state = State.Idle;

		}
		PlayerControlManager.Instance.CardHandled();

	}
	private bool IsMouseOverUI()
	{
		Vector3[] corners = CharacterBarUI.Instance.GetCancelArea();
		if (corners == null)
		{
			return false;
		}
		Vector3 mousePosition = Input.mousePosition;
		if (mousePosition.x >= corners[0].x && mousePosition.x <= corners[2].x && mousePosition.y >= corners[0].y && mousePosition.y <= corners[2].y)
		{
			return true;
		}

		return false;
	}

	private void Card_OnLevelChanged(object sender, EventArgs e)
    {
        anim.ActivateEvolutionVisual(card.level);
        SetStats();
    }

    private void SetStats()
    {
        currentHealth += card.Health[card.level - 1] - maxHealth;
        maxHealth = card.Health[card.level - 1];
        baseAttack = card.Attack[card.level - 1];
        attack = baseAttack;
        baseAttackSpeed = card.AttackSpeed[card.level - 1];
        attackSpeed = baseAttackSpeed;
        baseMoveSpeed = card.MoveSpeed[card.level - 1];
        moveSpeed = baseMoveSpeed;
        attackRange = card.AttackRange;
    }
}

