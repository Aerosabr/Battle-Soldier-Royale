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
    [SerializeField] private Transform cancelButton;
	private RectTransform characterBar;
	private float lerpDuration = 0.1f;
	private Vector2 hiddenPosition;
	private Vector2 visiblePosition;

	private void Awake()
    {
        Instance = this;
        charTemplate.gameObject.SetActive(false);
        characterBar = container.GetComponent<RectTransform>();
		visiblePosition = characterBar.anchoredPosition;
		hiddenPosition = new Vector2(visiblePosition.x, -Screen.height);
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

	public void ShowCharacterBar()
	{
		StartCoroutine(LerpPosition(visiblePosition));
        cancelButton.gameObject.SetActive(false);
	}

	public void HideCharacterBar()
	{
		StartCoroutine(LerpPosition(hiddenPosition));
		cancelButton.gameObject.SetActive(true);
	}

	private IEnumerator LerpPosition(Vector2 targetPosition)
	{
		float timeElapsed = 0;
		Vector2 startPosition = characterBar.anchoredPosition;

		while (timeElapsed < lerpDuration)
		{
			characterBar.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, timeElapsed / lerpDuration);
			timeElapsed += Time.deltaTime;
			yield return null;
		}

		characterBar.anchoredPosition = targetPosition;
	}

	public void SetCurrentButtonSelected(Transform button)
    {
        currentTemplateSelected = button;
    }

    public void ActivateCooldown()
    {
		currentTemplateSelected.GetComponent<CharacterBarSingleUI>().StartCooldown();
	}

    public Vector3[] GetCancelArea()
    {
		RectTransform rectTransform = cancelButton.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
			return corners;

		}
		ShowCharacterBar();
		return null;
	}
}
