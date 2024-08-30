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

    public void UpdateCard(CharacterSO character)
    {
        charCost.text = character.character.GetComponent<Character>().GetCost().ToString();
        charSprite.sprite = character.background;
        PlayerManager.Instance.AttachButton(button, character.character.gameObject);
    }
}
