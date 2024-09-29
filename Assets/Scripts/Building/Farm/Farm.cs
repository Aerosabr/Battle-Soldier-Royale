using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Building, IDamageable
{
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;
    public event EventHandler<IDamageable.OnDamageTakenEventArgs> OnDamageTaken;

    private enum State
    {
        None,
        Building,
        Farming,
        Destroyed
    }

    private State state;
    [SerializeField] private FarmVisual farmVisual;

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
        float hpPercent = (float)currentHealth / maxHealth;
        OnHealthChanged?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthPercentage = hpPercent
        });
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
            farmVisual.ChangeBuildingVisual(card.level, farmVisual.GetEvolutionVisual(card.level).bodyParts.Count);
            state = State.Destroyed;
            GetComponent<BoxCollider>().enabled = false;
            player.RemoveFromMilitary(gameObject);
            farmVisual.AnimAction("Destroyed");
        }
    }

    public override void InitializeBuilding(LayerMask layerMask, CardSO card, BuildingSlot buildingSlot)
    {
        this.card = card;
        this.card.OnLevelChanged += Card_OnLevelChanged;
        maxHealth = evolutionStats[card.level - 1].Health;
        attack = evolutionStats[card.level - 1].Attack;
        passiveGoldTimerMax = 1f / (float)attack;
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
            farmVisual.ChangeBuildingVisual(card.level, buildingProgress);
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
        passiveGoldTimerMax = 1f / (float)attack;
    }
}
