using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
	public static CameraAnimation Instance;
	private Animator animator;
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private float fadeDuration = 1f;
	private void Awake()
	{
		Instance = this;
		animator = GetComponent<Animator>();
		animator.Play("CameraMoveDown", 0, 0f);
		FadeIn();
	}
	public void MoveUp(string sceneName)
	{
		animator.Play("CameraMoveUp", 0, 0f);
		StartCoroutine(TransitionAfterMovingUp(sceneName));
	}
	private IEnumerator TransitionAfterMovingUp(string sceneName)
	{
		while (!animator.GetCurrentAnimatorStateInfo(0).IsName("CameraMoveUp"))
		{
			yield return null;
		}
		while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
		{
			yield return null;
		}
		SceneLoader.Instance.TransitionScene(sceneName);
	}

	public void FadeIn()
	{
		float startAlpha = 0f;
		float endAlpha = 1f;
		StartCoroutine(FadeCoroutine(startAlpha, endAlpha));
	}

	private IEnumerator FadeCoroutine(float startAlpha, float endAlpha)
	{
		float elapsedTime = 0f;
		float delay = 1f;

		while(elapsedTime < delay)
		{
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		elapsedTime = 0f;

		while (elapsedTime < fadeDuration)
		{
			elapsedTime += Time.deltaTime;
			float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
			canvasGroup.alpha = alpha;
			yield return null;
		}

		canvasGroup.alpha = endAlpha;
	}
}
