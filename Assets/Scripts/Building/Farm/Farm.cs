using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Farm : Building
{
    private enum State
    {
        None,
        Building,
        Farming,
        Destroyed
    }

    private State state;
    [SerializeField] private FarmVisual farmVisual;
    [SerializeField] private FarmSound sound;

    private int buildingProgress = 0;
    private float passiveGoldTimer;
    private float passiveGoldTimerMax;

    private void Awake()
    {
        state = State.None;
    }

    private void Update()
    {
        switch (state)
        {
            case State.None:
                break;
            case State.Building:
                Building();
                break;
            case State.Farming:
                passiveGoldTimer += Time.deltaTime;
                if (passiveGoldTimer >= passiveGoldTimerMax)
                {
                    player.AddGold(1);
                    passiveGoldTimer = 0;
                }
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

        float hpPercent = currentHealth / maxHealth;
        int progress = 0;
        for (int i = 1; i <= farmVisual.GetEvolutionVisual(card.level).bodyParts.Count; i++)
        {
            if (hpPercent >= (i / (float)farmVisual.GetEvolutionVisual(card.level).bodyParts.Count))
                progress++;
        }

        if (progress != buildingProgress)
        {
            buildingProgress = progress;
            farmVisual.ChangeBuildingVisual(card.level, buildingProgress);
        }

        if (currentHealth >= maxHealth)
        {
            state = State.Farming;
            sound.Building(false);
            sound.FinishBuilding();
            currentHealth = maxHealth;
        }
    }

    public override void Damaged(float damage, CardSO.CardType cardType)
    {
        currentHealth -= damage;
        HealthChangedVisual();
        DamageTakenVisual(damage);

        if (currentHealth <= 0)
        {
            farmVisual.ChangeBuildingVisual(card.level, farmVisual.GetEvolutionVisual(card.level).bodyParts.Count);
            state = State.Destroyed;
            GetComponent<BoxCollider>().enabled = false;
            player.RemoveFromEconomy(gameObject, false);
            farmVisual.AnimAction("Destroyed");
        }
    }

    public override void InitializeBuilding(LayerMask layerMask, CardSO card, BuildingSlot buildingSlot)
    {
		transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
		this.card = card as BuildingCardSO;
        this.card.OnLevelChanged += Card_OnLevelChanged;
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
        player.AddToEconomy(gameObject, false);

        healthBarUI.SetColor(player.playerColor);
        state = State.Building;
        sound.Building(true);
        SetStats();
    }
	private void Card_OnLevelChanged(object sender, EventArgs e)
    {
        if (buildingProgress != 0)
            farmVisual.ChangeBuildingVisual(card.level, buildingProgress);
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

        passiveGoldTimerMax = 1f / attack;
    }
}
