using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CharacterBarUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform charTemplate;

    private void Awake()
    {
        charTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == charTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach (CharacterPathSO charPathSO in PlayerManager.Instance.GetLoadout())
        {
            Transform recipeTransform = Instantiate(charTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            Debug.Log(charPathSO.characterSO[charPathSO.Level]);
            recipeTransform.GetComponent<CharacterBarSingleUI>().UpdateCard(charPathSO.characterSO[charPathSO.Level]);
        }
    }
}
