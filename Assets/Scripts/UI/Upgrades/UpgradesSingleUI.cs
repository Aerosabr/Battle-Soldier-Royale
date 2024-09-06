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

    [SerializeField] protected LoadoutCharacter loadoutCharacter;

    public void SetCPSO(LoadoutCharacter loadoutCharacter)
    {
        this.loadoutCharacter = loadoutCharacter;
    }
}
