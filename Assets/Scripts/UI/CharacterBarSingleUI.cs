using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBarSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI charCost;
    [SerializeField] private Image charSprite;
    [SerializeField] private Button button;
    [SerializeField] private GameObject shadow;
    private int cost;

    public void UpdateCard(CardSO cardSO)
    {
        cost = cardSO.cardCost[cardSO.level - 1];
        charCost.text = cost.ToString();
        charSprite.sprite = cardSO.backgrounds[cardSO.level - 1];

        button.onClick.AddListener(() =>
        {
            PlayerBlue.Instance.SpawnCharacter(cardSO);
        });

        PlayerBlue.Instance.OnGoldChanged += PlayerManager_OnGoldChanged;
    }

    private void PlayerManager_OnGoldChanged(object sender, System.EventArgs e)
    {
        if (PlayerBlue.Instance.GetGold() >= cost)
            shadow.SetActive(false);
        else
            shadow.SetActive(true);
    }

    private void OnDestroy()
    {
        Debug.Log("Destroyed");
        PlayerBlue.Instance.OnGoldChanged -= PlayerManager_OnGoldChanged;
    }
}
