using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Worker : Character
{
    private const int IS_IDLE = 0;
    private const int IS_WALKING = 1;
    private const int IS_DEAD = 2;

    private enum State
    {
        Ghost,
        Idle,
        Walking,
        Mining,
        Dead
    }
 
    [SerializeField] private WorkerVisual anim;
    [SerializeField] private GameObject hpBar;

    private bool isDepositing = false;
    private GameObject mine;
    private State state;
    private float miningTimer;

    private void Awake()
    {
        state = State.Ghost;
        miningTimer = 0;
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
			if (hit.collider.gameObject.tag == "Mine" && hit.collider.gameObject == mine && !isDepositing)
            {
                StartMining();
                transform.rotation = Quaternion.Euler(0, -transform.localEulerAngles.y, 0);
            }
            else if (hit.collider.gameObject.tag == "Base" && isDepositing)
            {
                Deposit();
                transform.rotation = Quaternion.Euler(0, -transform.localEulerAngles.y, 0);
            }
        }
    }

    private void StartMining()
    { 
        state = State.Mining;
        GetComponent<BoxCollider>().enabled = false;
        anim.gameObject.SetActive(false);
        hpBar.SetActive(false);
        anim.AnimAction(IS_WALKING);
    }

    private void FinishMining()
    {
        state = State.Idle;
        miningTimer = 0f;
        GetComponent<BoxCollider>().enabled = true;
        anim.gameObject.SetActive(true);
        if (currentHealth < maxHealth)
            hpBar.SetActive(true);
        anim.AnimAction(IS_WALKING);
        isDepositing = true;
        targetLayer = 1 << gameObject.layer;
    }

    private void Deposit()
    {
        // transform.rotation = Quaternion.Euler(0, -transform.rotation.y, 0);
        isDepositing = false;
        if (player.playerColor == Player.PlayerColor.Blue)
            PlayerBlue.Instance.AddGold(attack);
        else
            PlayerRed.Instance.AddGold(attack);

        targetLayer = 1 << 8;
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

    public void InitializeWorker(LayerMask layerMask, Vector3 rotation, CardSO card, GameObject mine)
    {
        this.mine = mine;
        InitializeCharacter(layerMask, rotation, card);
    }

    public override void InitializeCharacter(LayerMask layerMask, Vector3 rotation, CardSO card)
    {
        this.card = card as CharacterCardSO;
        this.card.OnLevelChanged += Card_OnLevelChanged;
        anim.ActivateEvolutionVisual(card.level);
        SetStats();
        gameObject.transform.rotation = Quaternion.Euler(rotation);
        gameObject.layer = layerMask;
        state = State.Idle;
        if (gameObject.layer == 6)
        {
            player = PlayerBlue.Instance;
            //targetLayer = 1 << 7;
        }
        else
        {
            player = PlayerRed.Instance;
            //targetLayer = 1 << 6;
        }
        targetLayer = (1 << 8);
        //targetLayer = (1 << 8) | (1 << 9);
        player.AddToEconomy(gameObject, true);
    }

	public override IEnumerator Project(LayerMask layerMask, Vector3 rotation, CardSO card)
	{
		transform.GetComponent<BoxCollider>().enabled = false;
        transform.gameObject.layer = 0;
		int neutralWallLayer = LayerMask.NameToLayer("NeutralWall");
		LayerMask neutralWallMask = 1 << neutralWallLayer;
		int buildableWallLayer = LayerMask.NameToLayer("BuildingWall");
		LayerMask buildableWallMask = 1 << buildableWallLayer;
        MeshRenderer meshRenderer = indicator.GetComponent<MeshRenderer>();
		while (Mouse.current.leftButton.isPressed)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildableWallMask))
			{
                mine = hit.transform.parent.gameObject;
				Transform hitTransform = hit.transform;
				transform.position = hitTransform.position;
                meshRenderer.material = allowed;
			}
			else if (Physics.Raycast(ray, out hit, Mathf.Infinity, neutralWallMask))
			{
                mine = null;
				Vector3 worldPosition = hit.point;
				transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);
				transform.rotation = Quaternion.Euler(rotation);
                meshRenderer.material = denied;
			}
			yield return null;
		}
		if (layerMask == 6)
		{
			player = PlayerBlue.Instance;
			targetLayer = 1 << 7;
		}
		else
		{
			player = PlayerRed.Instance;
			targetLayer = 1 << 6;
		}
		if (IsMouseOverUI() || mine == null)
        {
            Destroy(gameObject);
        }
        else
        {
            CharacterBarUI.Instance.ActivateCooldown();
            player.SpawnWorker(card, mine);
            Destroy(gameObject);
        }
		MapManager.Instance.HideAllMineSlotsIndicator();
		PlayerControlManager.Instance.CardHandled();
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
