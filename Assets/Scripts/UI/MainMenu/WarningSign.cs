using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningSign : MonoBehaviour
{
    public static WarningSign Instance;
	public GameObject targetGameObject;
	public Button PlayButton;


	void Start()
    {
        Instance = this;
    }

	public void ActivateWithTimer()
	{
		float duration = 3;
		targetGameObject.SetActive(true);
		StartCoroutine(DeactivateAfterDelay(duration));
	}

	private IEnumerator DeactivateAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		targetGameObject.SetActive(false);
		PlayButton.interactable = true;

	}

}
