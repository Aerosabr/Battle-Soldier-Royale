using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IDamageable character;

    private void Start()
    {
        character = hasProgressGameObject.GetComponent<IDamageable>();
        if (character == null)
            Debug.LogError("Gameobject does not have a component that implements Character");

        character.OnHealthChanged += Character_OnHealthChanged;
        barImage.fillAmount = 0f;

        Hide();
    }

    private void Character_OnHealthChanged(object sender, IDamageable.OnHealthChangedEventArgs e)
    {
        barImage.fillAmount = e.healthPercentage;

        if (e.healthPercentage <= 0f || e.healthPercentage == 1f)
            Hide();
        else
            Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
