using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    //Stat Components
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Health; 
    [SerializeField] private TextMeshProUGUI Attack; 
    [SerializeField] private TextMeshProUGUI AttackSpeed;
    [SerializeField] private TextMeshProUGUI AttackRange; 
    [SerializeField] private TextMeshProUGUI AttackType;
    [SerializeField] private TextMeshProUGUI MoveSpeed;
    [SerializeField] private TextMeshProUGUI Cost; 
    [SerializeField] private TextMeshProUGUI Cooldown;
    [SerializeField] private TextMeshProUGUI GoldPerSecond;
    [SerializeField] private TextMeshProUGUI BuildTime;

    //Button Components
    [SerializeField] private Button UpgradeButton;
    [SerializeField] private TextMeshProUGUI UpgradeCost;
    [SerializeField] private Button LevelViewButton;
    [SerializeField] private TextMeshProUGUI LevelViewText;

    private bool showingCurrent = true;
    private CardSO currentCard;

    private void Awake()
    {
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
                break;
            case CardSO.CardType.Character:
                currentCard = card;
                LoadCharacter();
                break;
            case CardSO.CardType.Spell:
                break;
            case CardSO.CardType.Worker:
                break;
        }
    }

    private void LoadBuilding(BuildingCardSO buildingCard)
    {

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

    private void LoadSpell(SpellCardSO spellCard)
    {

    }

    private void LoadWorker(CharacterCardSO characterCard)
    {

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
        GoldPerSecond.transform.parent.gameObject.SetActive(false);
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
}
