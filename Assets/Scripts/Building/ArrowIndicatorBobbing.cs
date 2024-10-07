using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicatorBobbing : MonoBehaviour
{
	public float bobbingSpeed = 2f;
	public float bobbingHeight = 0.5f;

	private Vector3 startPosition;
	void Awake()
	{
		startPosition = transform.position;
	}
	void Update()
	{
		// Apply bobbing motion using sine wave based on time
		float newY = startPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
		transform.position = new Vector3(startPosition.x, newY, startPosition.z);
	}
}
