using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.ComponentModel;

public class CharacterInfoUI : MonoBehaviour
{
    //Stat Components
    [SerializeField] private Transform container;
    [SerializeField] private Transform infoTemplate;
    [SerializeField] private TextMeshProUGUI Name;

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
                SoundManager.Instance.CardUpgraded();
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
            level = buildingCard.level;
        else
            level = buildingCard.level + 1;

        HideStats();

        Name.text = buildingCard.Name;
        CardImage.sprite = buildingCard.backgroundVertical[level - 1];
        UpgradeCost.text = buildingCard.upgradeCost[buildingCard.level - 1].ToString();

        foreach (string info in buildingCard.ViewCard(level))
        {
            Transform infoTransform = Instantiate(infoTemplate, container);
            infoTransform.gameObject.SetActive(true);
            string[] infoArray = info.Split(": ");
            infoTransform.GetComponent<CharacterInfoSingleUI>().SetInfo(infoArray[0], infoArray[1]);
        }
    }

    private void LoadCharacter()
    {
        CharacterCardSO characterCard = currentCard as CharacterCardSO;
        int level = 0;
        if (showingCurrent)
            level = characterCard.level;
        else
            level = characterCard.level + 1;

        HideStats();

        Name.text = characterCard.Name;
        CardImage.sprite = characterCard.backgroundVertical[level - 1];
        UpgradeCost.text = characterCard.upgradeCost[characterCard.level - 1].ToString();

        foreach (string info in characterCard.ViewCard(level))
        {
            Transform infoTransform = Instantiate(infoTemplate, container);
            infoTransform.gameObject.SetActive(true);
            string[] infoArray = info.Split(": ");
            infoTransform.GetComponent<CharacterInfoSingleUI>().SetInfo(infoArray[0], infoArray[1]);
        }
    }

    private void LoadSpell()
    {
        SpellCardSO spellCard = currentCard as SpellCardSO;
        int level = 0;
        if (showingCurrent)
            level = spellCard.level;
        else
            level = spellCard.level + 1;

        HideStats();

        Name.text = spellCard.Name;
        CardImage.sprite = spellCard.backgroundVertical[level - 1];
        UpgradeCost.text = spellCard.upgradeCost[spellCard.level - 1].ToString();

        foreach (string info in spellCard.ViewCard(level))
        {
            Transform infoTransform = Instantiate(infoTemplate, container);
            infoTransform.gameObject.SetActive(true);
            string[] infoArray = info.Split(": ");
            infoTransform.GetComponent<CharacterInfoSingleUI>().SetInfo(infoArray[0], infoArray[1]);
        }
    }

    private void LoadWorker()
    {
        CharacterCardSO characterCard = currentCard as CharacterCardSO;
        int level = 0;
        if (showingCurrent)
            level = characterCard.level;
        else
            level = characterCard.level + 1;

        HideStats();

        Name.text = characterCard.Name;
        CardImage.sprite = characterCard.backgroundVertical[level - 1];
        UpgradeCost.text = characterCard.upgradeCost[characterCard.level - 1].ToString();

        foreach (string info in characterCard.ViewCard(level))
        {
            Transform infoTransform = Instantiate(infoTemplate, container);
            infoTransform.gameObject.SetActive(true);
            string[] infoArray = info.Split(": ");
            infoTransform.GetComponent<CharacterInfoSingleUI>().SetInfo(infoArray[0], infoArray[1]);
        }
    }

    private void HideStats()
    {
        foreach (Transform child in container)
        {
            if (child == infoTemplate) continue;

            Destroy(child.gameObject);
        }
    }

    private void SwitchStatView()
    {
        showingCurrent = !showingCurrent;

        if (showingCurrent)
        {
            LevelViewText.text = "Next Lv";
            SoundManager.Instance.ButtonPressed();
        }
        else
        {
            LevelViewText.text = "Current Lv";
            SoundManager.Instance.TabClosed();
        }

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
