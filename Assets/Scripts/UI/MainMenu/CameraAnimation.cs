using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		animator.Play("CameraMoveDown", 0, 0f);
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
}
