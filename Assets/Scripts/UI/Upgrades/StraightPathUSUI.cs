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
        buttonList[loadoutCharacter.Level].button.onClick.AddListener(() =>
        {
            UpgradeCharacter();
        });
    }

    private void UpgradeCharacter()
    {
        if (PlayerManager.Instance.GetGold() >= int.Parse(buttonList[loadoutCharacter.Level].costText.text))
        {
            buttonList[loadoutCharacter.Level].levelText.color = Color.green;
            buttonList[loadoutCharacter.Level].costText.enabled = false;
            buttonList[loadoutCharacter.Level].button.onClick.RemoveAllListeners();
            PlayerManager.Instance.SubtractGold(int.Parse(buttonList[loadoutCharacter.Level].costText.text));
            PlayerManager.Instance.IncreaseLoadoutLevel(loadoutCharacter.characterPathSO);
            loadoutCharacter.Level++;
            CharacterBarUI.Instance.UpdateVisual();
            if (loadoutCharacter.Level < buttonList.Count)
                ActivateButton();
        }
    }
}
