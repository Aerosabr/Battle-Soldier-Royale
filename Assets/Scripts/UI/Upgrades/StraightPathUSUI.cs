using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class StraightPathUSUI : UpgradesSingleUI
{
    [SerializeField] private List<UpgradeButton> buttonList;

    private void Start()
    {
        ActivateButton();
    }

    private void ActivateButton()
    {
        buttonList[loadoutCard.level].button.onClick.AddListener(() =>
        {
            UpgradeCharacter();
        });
    }

    private void UpgradeCharacter()
    {
        if (PlayerBlue.Instance.GetGold() >= loadoutCard.upgradeCost[loadoutCard.level - 1])
        {
            buttonList[loadoutCard.level].levelText.color = Color.green;
            buttonList[loadoutCard.level].costText.enabled = false;
            buttonList[loadoutCard.level].button.onClick.RemoveAllListeners();
            PlayerBlue.Instance.SubtractGold(int.Parse(buttonList[loadoutCard.level].costText.text));
            PlayerBlue.Instance.IncreaseCardLevel(loadoutCard);
            CharacterBarUI.Instance.UpdateVisual();
            if (loadoutCard.level < buttonList.Count)
                ActivateButton();
        }
    }
}
