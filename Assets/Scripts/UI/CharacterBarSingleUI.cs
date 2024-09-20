using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterBarSingleUI : MonoBehaviour, IPointerDownHandler
{ 
    [SerializeField] private TextMeshProUGUI charCost;
    [SerializeField] private Image charSprite;
    [SerializeField] private Button button;
    [SerializeField] private GameObject shadow;
    [SerializeField] private GameInput gameInput;
    private int cost;
    private CardSO cardSO;
    public void UpdateCard(CardSO cardSO)
    {
        cost = cardSO.cardCost[cardSO.level - 1];
        charCost.text = cost.ToString();
        charSprite.sprite = cardSO.backgrounds[cardSO.level - 1];
        this.cardSO = cardSO;
		PlayerBlue.Instance.OnGoldChanged += PlayerManager_OnGoldChanged;

        if(cardSO.cardType == CardSO.CardType.Character)
        {
			button.onClick.AddListener(() =>
			{
				PlayerControlManager.Instance.CardSelected(cardSO);
			});
		}

	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if(cardSO.cardType == CardSO.CardType.Spell)
        {
			PlayerControlManager.Instance.CardSelected(cardSO);
            CharacterBarUI.Instance.SetCurrentButtonSelected(this.transform);
		}
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
