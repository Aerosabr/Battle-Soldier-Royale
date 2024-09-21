using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CharacterBarUI : MonoBehaviour
{
    public static CharacterBarUI Instance;
    [SerializeField] private Transform container;
    [SerializeField] private Transform charTemplate;
    [SerializeField] private Transform currentTemplateSelected;

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

        foreach (CardSO loadoutCard in PlayerBlue.Instance.GetLoadout())
        {
            Transform charTransform = Instantiate(charTemplate, container);
            charTransform.gameObject.SetActive(true);
            charTransform.GetComponent<CharacterBarSingleUI>().UpdateCard(loadoutCard);
        }
    }

    public void SetCurrentButtonSelected(Transform button)
    {
        currentTemplateSelected = button;
    }

    public Vector3[] GetCancelArea()
    {
        RectTransform rectTransform = currentTemplateSelected.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
			return corners;

		}
        return null;
	}
}
