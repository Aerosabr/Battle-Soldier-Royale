using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CharacterBarUI : MonoBehaviour
{
    public static CharacterBarUI Instance;
    [SerializeField] private Transform container;
    [SerializeField] private Transform charTemplate;

    private void Awake()
    {
        Instance = this;
        charTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == charTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach (LoadoutCharacter loadoutChar in PlayerBlue.Instance.GetLoadout())
        {
            Transform charTransform = Instantiate(charTemplate, container);
            charTransform.gameObject.SetActive(true);
            charTransform.GetComponent<CharacterBarSingleUI>().UpdateCard(loadoutChar.characterPathSO.characterSO[loadoutChar.Level - 1]);
        }
    }
}
