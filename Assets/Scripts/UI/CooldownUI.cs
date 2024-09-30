using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    private float cooldownTimer;
    private float cooldownTimerMax;
    [SerializeField] private Image image;
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        image.fillAmount = 1f - (cooldownTimer /  cooldownTimerMax);
        if (cooldownTimer >= cooldownTimerMax)
        {
            gameObject.SetActive(false);
        }
    }

    public void InitializeCooldownUI(float cdTimer)
    {
        cooldownTimerMax = cdTimer;
        //gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        cooldownTimer = 0;
        image.fillAmount = 1;
    }
}
