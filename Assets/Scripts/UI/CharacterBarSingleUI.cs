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
    [SerializeField] private CooldownUI cooldownUI;

    [SerializeField] private GameObject numWorkers;

    private int cost;
    private CardSO cardSO;
    private bool activatable = true;

    public void UpdateCard(CardSO cardSO)
    {
        cost = cardSO.cardCost[cardSO.level - 1];
        charCost.text = cost.ToString();
        charSprite.sprite = cardSO.backgrounds[cardSO.level - 1];
        this.cardSO = cardSO;

        if (cardSO.cardType == CardSO.CardType.Worker)
        {
            numWorkers.SetActive(true);
            numWorkers.GetComponent<TextMeshProUGUI>().text = PlayerBlue.Instance.GetNumberOfWorkers().ToString();
            PlayerBlue.Instance.OnWorkerChanged += Player_OnWorkerChanged;
        }

        cooldownUI.InitializeCooldownUI(cardSO.spawnCooldown[cardSO.level - 1]);

		button.gameObject.SetActive(false);

		PlayerBlue.Instance.OnGoldChanged += PlayerManager_OnGoldChanged;
	}

    private void Player_OnWorkerChanged(object sender, System.EventArgs e)
    {
        numWorkers.GetComponent<TextMeshProUGUI>().text = PlayerBlue.Instance.GetNumberOfWorkers().ToString();

        if (PlayerBlue.Instance.GetNumberOfWorkers() == GameManager.Instance.GetMaxWorkerAmount())
        {
            activatable = false;
            shadow.SetActive(true);
        }
        else
        {
            activatable = true;
            shadow.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
	{
        if (!cooldownUI.gameObject.activeSelf && PlayerBlue.Instance.GetGold() >= cost && activatable)
        {
            PlayerControlManager.Instance.CardSelected(cardSO);
            CharacterBarUI.Instance.SetCurrentButtonSelected(this.transform);
        }
	}
    
    public void StartCooldown()
    {
		cooldownUI.gameObject.SetActive(true);
	}

	private void PlayerManager_OnGoldChanged(object sender, System.EventArgs e)
    {
        if (PlayerBlue.Instance.GetGold() >= cost && activatable)
            shadow.SetActive(false);
        else
            shadow.SetActive(true);
    }

    private void OnDestroy()
    {
        PlayerBlue.Instance.OnGoldChanged -= PlayerManager_OnGoldChanged;
    }
}
