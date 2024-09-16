using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesSingleUI : MonoBehaviour
{
    [System.Serializable]
    public struct UpgradeButton
    {
        public Button button;
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI costText;
    }

    [SerializeField] protected CardSO loadoutCard;

    public void SetCPSO(CardSO loadoutCard)
    {
        this.loadoutCard = loadoutCard;
    }
}
