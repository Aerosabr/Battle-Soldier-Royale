using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAreaIndicator : MonoBehaviour
{
	[SerializeField] private Player player;
	[SerializeField] private float distanceAwayfromCharacter = 0f;

	void Start()
	{
	}
	private void Update()
	{
		transform.position = transform.parent.transform.position;
		float furthestControlledArea = player.GetFurthestControlledArea() - distanceAwayfromCharacter;
		Vector3 newScale = transform.localScale;
		newScale.x = furthestControlledArea;
		transform.localScale = newScale;
		transform.position = new Vector3(transform.parent.transform.position.x + (furthestControlledArea / 2),
										 transform.position.y,
										 transform.position.z);
	}
}
