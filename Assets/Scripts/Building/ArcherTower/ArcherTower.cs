using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArcherTower : Building
{
    private const int IS_IDLE = 0;
    private const int IS_BUILDING = 1;
    private const int IS_ATTACKING = 2;
    private const int IS_DESTROYED = 3;

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
        HealthChangedVisual();

        float hpPercent = (float)currentHealth / maxHealth;
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

    public override void Damaged(int damage)
    {
        currentHealth -= damage;
        HealthChangedVisual();
        //DamageTakenVisual(damage);

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
        this.card = card as BuildingCardSO;
        this.card.OnLevelChanged += Card_OnLevelChanged;
        gameObject.layer = layerMask;
        this.buildingSlot = buildingSlot;
        buildingSlot.SetBuilding(this);

        if (gameObject.layer == 6)
        {
            player = PlayerBlue.Instance;
            targetLayer = 1 << 7;
            gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            player = PlayerRed.Instance;
            targetLayer = 1 << 6;
            gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        state = State.Building;
        SetStats();
    }

	public override IEnumerator Project(LayerMask layerMask, CardSO card, BuildingSlot buildingSlot)
	{
        bool OnPlaceable = false;
        BuildingSlot slot = this.buildingSlot;
		int neutralWallLayer = LayerMask.NameToLayer("NeutralWall");
		LayerMask neutralWallMask = 1 << neutralWallLayer;
		int buildableWallLayer = LayerMask.NameToLayer("BuildingWall");
		LayerMask buildableWallMask = 1 << buildableWallLayer;
		while (Mouse.current.leftButton.isPressed)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildableWallMask))
            {
                if (!hit.transform.parent.GetComponent<BuildingSlot>().ContainsBuilding())
                {
                    OnPlaceable = true;
                    slot = hit.transform.parent.GetComponent<BuildingSlot>();
					Transform hitTransform = hit.transform;
                    transform.position = hitTransform.position;
                }
			}
			else if (Physics.Raycast(ray, out hit, Mathf.Infinity, neutralWallMask))
			{
                OnPlaceable = false;
				Vector3 worldPosition = hit.point;
				transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);
			}
			yield return null;
		}
		if (IsMouseOverUI() || OnPlaceable == false)
		{
			Destroy(gameObject);
		}
		else
		{
			transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
			InitializeBuilding(layerMask, card, slot);
			CharacterBarUI.Instance.ActivateCooldown();
			player.SubtractGold(card.cardCost[card.level - 1]);
		}
        MapManager.Instance.HideAllBuildingSlotsIndicator();
		PlayerControlManager.Instance.CardHandled();

	}

	private void Card_OnLevelChanged(object sender, EventArgs e)
    {
        if (buildingProgress != 0)
            archerTowerVisual.ChangeBuildingVisual(card.level, buildingProgress);
        SetStats();
    }

    private void SetStats()
    {
        if (state != State.Building)
            currentHealth += card.Health[card.level - 1] - maxHealth;

        maxHealth = card.Health[card.level - 1];
        baseAttack = card.Attack[card.level - 1];
        attack = baseAttack;
        baseAttackSpeed = card.AttackSpeed[card.level - 1];
        attackSpeed = baseAttackSpeed;
        attackRange = card.AttackRange;
        buildTimer = card.BuildTimer[card.level - 1];
    }
}
