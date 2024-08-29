using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        PlayerManager.Instance.OnGoldChanged += PlayerManager_OnGoldChanged;
    }

    private void PlayerManager_OnGoldChanged(object sender, System.EventArgs e)
    {
        text.text = PlayerManager.Instance.GetGold().ToString();
    }
}
