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

    public void UpdateCard(CharacterSO character)
    {
        cost = character.character.GetComponent<Character>().GetCost();
        charCost.text = cost.ToString();
        charSprite.sprite = character.background;

        button.onClick.AddListener(() =>
        {
            PlayerManager.Instance.spawner.SpawnCharacter(character.character.gameObject);
        });

        PlayerManager.Instance.OnGoldChanged += PlayerManager_OnGoldChanged;
    }

    private void PlayerManager_OnGoldChanged(object sender, System.EventArgs e)
    {
        if (PlayerManager.Instance.GetGold() >= cost)
            shadow.SetActive(false);
        else
            shadow.SetActive(true);
    }

    private void OnDestroy()
    {
        Debug.Log("Destroyed");
        PlayerManager.Instance.OnGoldChanged -= PlayerManager_OnGoldChanged;
    }
}
