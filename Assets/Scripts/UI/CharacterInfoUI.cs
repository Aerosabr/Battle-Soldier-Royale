using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    //Stat Components
    [SerializeField] private Transform infoTemplate;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Level;
    [SerializeField] private TextMeshProUGUI Health; //Building, Character, Worker
    [SerializeField] private TextMeshProUGUI Attack; //Building.Defense
    [SerializeField] private TextMeshProUGUI AttackSpeed; //Character, Building.Defense
    [SerializeField] private TextMeshProUGUI AttackRange; //Character, Building.Defense
    [SerializeField] private TextMeshProUGUI AttackType; //Character, Building.Defense
    [SerializeField] private TextMeshProUGUI MoveSpeed; //Character, Worker
    [SerializeField] private TextMeshProUGUI Cost; //All
    [SerializeField] private TextMeshProUGUI Cooldown; //All
    [SerializeField] private TextMeshProUGUI Income; //Building.Economy, Worker
    [SerializeField] private TextMeshProUGUI BuildTime; //Building

    //Button Components
    [SerializeField] private Button UpgradeButton;
    [SerializeField] private TextMeshProUGUI UpgradeCost;
    [SerializeField] private Button LevelViewButton;
    [SerializeField] private TextMeshProUGUI LevelViewText;

    [SerializeField] private Image CardImage;

    public bool showingCurrent = true;
    private CardSO currentCard;

    private void Awake()
    {
        UpgradeButton.onClick.AddListener(() =>
        {
            if (PlayerBlue.Instance.GetGold() >= currentCard.upgradeCost[currentCard.level - 1])
            {
                PlayerBlue.Instance.SubtractGold(currentCard.upgradeCost[currentCard.level - 1]);
                currentCard.IncreaseCardLevel();
                showingCurrent = true;
                LoadCardStats(currentCard);
                CharacterBarUI.Instance.UpdateVisual();
            }
        });

        LevelViewButton.onClick.AddListener(() =>
        {
            SwitchStatView();
        });
    }

    public void LoadCardStats(CardSO card)
    {
        switch (card.cardType)
        {
            case CardSO.CardType.Building:
                currentCard = card;
                LoadBuilding();
                break;
            case CardSO.CardType.Character:
                currentCard = card;
                LoadCharacter();
                break;
            case CardSO.CardType.Spell:
                currentCard = card;
                LoadSpell();
                break;
            case CardSO.CardType.Worker:
                currentCard = card;
                LoadWorker();
                break;
        }
        CheckLevelMaxed();
    }

    private void LoadBuilding()
    {
        BuildingCardSO buildingCard = currentCard as BuildingCardSO;
        int level = 0;
        if (showingCurrent)
            level = buildingCard.level - 1;
        else
            level = buildingCard.level;

        HideStats();

        Name.text = buildingCard.Name;
        Level.text = (level + 1).ToString();
        CardImage.sprite = buildingCard.backgroundVertical[level];

        Health.text = buildingCard.Health[level].ToString();
        Health.transform.parent.gameObject.SetActive(true);

        if (buildingCard.BuildingType == BuildingType.Defense)
        {
            Attack.text = buildingCard.Attack[level].ToString();
            Attack.transform.parent.gameObject.SetActive(true);

            AttackSpeed.text = buildingCard.AttackSpeed[level].ToString();
            AttackSpeed.transform.parent.gameObject.SetActive(true);

            AttackRange.text = buildingCard.AttackRange.ToString();
            AttackRange.transform.parent.gameObject.SetActive(true);

            this.AttackType.text = buildingCard.AttackType.ToString();
            AttackType.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            Income.text = buildingCard.Attack[level].ToString() + "/s";
            Income.transform.parent.gameObject.SetActive(true);
        }

        Cost.text = buildingCard.cardCost[level].ToString();
        Cost.transform.parent.gameObject.SetActive(true);

        Cooldown.text = buildingCard.spawnCooldown[level].ToString();
        Cooldown.transform.parent.gameObject.SetActive(true);

        BuildTime.text = buildingCard.BuildTimer[level].ToString();
        BuildTime.transform.parent.gameObject.SetActive(true);

        UpgradeCost.text = buildingCard.upgradeCost[buildingCard.level - 1].ToString();
    }

    private void LoadCharacter()
    {
        CharacterCardSO characterCard = currentCard as CharacterCardSO;
        int level = 0;
        if (showingCurrent)
            level = characterCard.level - 1;
        else
            level = characterCard.level;

        HideStats();

        Name.text = characterCard.Name;
        Level.text = (level + 1).ToString();
        CardImage.sprite = characterCard.backgroundVertical[level];

        Health.text = characterCard.Health[level].ToString();
        Health.transform.parent.gameObject.SetActive(true);

        Attack.text = characterCard.Attack[level].ToString();
        Attack.transform.parent.gameObject.SetActive(true);

        AttackSpeed.text = characterCard.AttackSpeed[level].ToString();
        AttackSpeed.transform.parent.gameObject.SetActive(true);

        AttackRange.text = characterCard.AttackRange.ToString();
        AttackRange.transform.parent.gameObject.SetActive(true);

        this.AttackType.text = characterCard.AttackType.ToString();
        AttackType.transform.parent.gameObject.SetActive(true);

        MoveSpeed.text = characterCard.MoveSpeed[level].ToString();
        MoveSpeed.transform.parent.gameObject.SetActive(true);

        Cost.text = characterCard.cardCost[level].ToString();
        Cost.transform.parent.gameObject.SetActive(true);

        Cooldown.text = characterCard.spawnCooldown[level].ToString();
        Cooldown.transform.parent.gameObject.SetActive(true);

        UpgradeCost.text = characterCard.upgradeCost[characterCard.level - 1].ToString();
    }

    private void LoadSpell()
    {
        SpellCardSO spellCard = currentCard as SpellCardSO;
        int level = 0;
        if (showingCurrent)
            level = spellCard.level - 1;
        else
            level = spellCard.level;

        HideStats();

        Name.text = spellCard.Name;
        Level.text = (level + 1).ToString();
        CardImage.sprite = spellCard.backgroundVertical[level];

        Attack.text = spellCard.Attack[level].ToString();
        Attack.transform.parent.gameObject.SetActive(true);
    }

    private void LoadWorker()
    {
        CharacterCardSO characterCard = currentCard as CharacterCardSO;
        int level = 0;
        if (showingCurrent)
            level = characterCard.level - 1;
        else
            level = characterCard.level;

        HideStats();

        Name.text = characterCard.Name;
        Level.text = (level + 1).ToString();
        CardImage.sprite = characterCard.backgroundVertical[level];

        Health.text = characterCard.Health[level].ToString();
        Health.transform.parent.gameObject.SetActive(true);

        Attack.text = characterCard.Attack[level].ToString();
        Attack.transform.parent.gameObject.SetActive(true);

        AttackSpeed.text = characterCard.AttackSpeed[level].ToString();
        AttackSpeed.transform.parent.gameObject.SetActive(true);

        MoveSpeed.text = characterCard.MoveSpeed[level].ToString();
        MoveSpeed.transform.parent.gameObject.SetActive(true);

        Cost.text = characterCard.cardCost[level].ToString();
        Cost.transform.parent.gameObject.SetActive(true);

        Cooldown.text = characterCard.spawnCooldown[level].ToString();
        Cooldown.transform.parent.gameObject.SetActive(true);

        UpgradeCost.text = characterCard.upgradeCost[characterCard.level - 1].ToString();
    }

    private void HideStats()
    {
        Health.transform.parent.gameObject.SetActive(false);
        Attack.transform.parent.gameObject.SetActive(false);
        AttackSpeed.transform.parent.gameObject.SetActive(false);
        AttackRange.transform.parent.gameObject.SetActive(false);
        AttackType.transform.parent.gameObject.SetActive(false);
        MoveSpeed.transform.parent.gameObject.SetActive(false);
        Cost.transform.parent.gameObject.SetActive(false);
        Cooldown.transform.parent.gameObject.SetActive(false);
        Income.transform.parent.gameObject.SetActive(false);
        BuildTime.transform.parent.gameObject.SetActive(false);
    }

    private void SwitchStatView()
    {
        showingCurrent = !showingCurrent;

        if (showingCurrent)
            LevelViewText.text = "Next Lv";
        else
            LevelViewText.text = "Current Lv";

        LoadCardStats(currentCard);
    }

    private void CheckLevelMaxed()
    {
        if (currentCard.level == currentCard.upgradeCost.Count)
        {
            showingCurrent = true;
            LevelViewText.text = "Lv MAX";
            UpgradeCost.text = "Max";
            LevelViewButton.enabled = false;
            UpgradeButton.enabled = false;
        }
        else
        {
            if (showingCurrent)
                LevelViewText.text = "Next Lv";
            else
                LevelViewText.text = "Current Lv";
            LevelViewButton.enabled = true;
            UpgradeButton.enabled = true;
        }
    }
}
