using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Spell : MonoBehaviour
{
	[SerializeField] protected int targetLayer;
	[SerializeField] protected Player player;
	[SerializeField] protected SpellCardSO cardSO;

	private GraphicRaycaster raycaster;
	[SerializeField] protected BoxCollider hitBox;
	[SerializeField] protected Transform transparentObject;
	[SerializeField] protected Transform visualObject;

	[SerializeField] protected List<Character> characters = new List<Character>();

	public virtual void InitializeSpell(LayerMask layerMask, SpellCardSO cardSO) 
	{
		transparentObject.gameObject.SetActive(false);
		visualObject.gameObject.SetActive(true);
		this.cardSO = cardSO;
		if (layerMask == 6)
		{
			player = PlayerBlue.Instance;
			targetLayer = 7;
		}
		else
		{
			player = PlayerRed.Instance;
			targetLayer = 6;
		}
		hitBox = GetComponent<BoxCollider>();
		raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
		hitBox.enabled = true;
		StartCoroutine(HandleHitBox());
		StartCoroutine(HandleAttack());
	}

	public virtual IEnumerator Project(LayerMask layerMask, CardSO cardSO)
	{
		float cameraDistance = 0.75f;
		transparentObject.gameObject.SetActive(true);
		visualObject.gameObject.SetActive(false);
		while (Mouse.current.leftButton.isPressed)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				Vector3 worldPosition = hit.point;
				transform.position = new Vector3(worldPosition.x, transform.position.y, cameraDistance);
			}
			yield return null;
		}
		if (layerMask == 6)
		{
			player = PlayerBlue.Instance;
			targetLayer = 1 << 7;
		}
		else
		{
			player = PlayerRed.Instance;
			targetLayer = 1 << 6;
		}
		if (IsMouseOverUI())
		{
			Destroy(gameObject);

		}
		else
		{
			player.SpawnSpell(cardSO, transform.position);
			Destroy(gameObject);

		}
		PlayerControlManager.Instance.CardHandled();
	}
	private bool IsMouseOverUI()
	{
		Vector3[] corners = CharacterBarUI.Instance.GetCancelArea();
		if (corners == null)
		{
			return false;
		}
		Vector3 mousePosition = Input.mousePosition;
		if (mousePosition.x >= corners[0].x && mousePosition.x <= corners[2].x && mousePosition.y >= corners[0].y && mousePosition.y <= corners[2].y)
		{
			return true;
		}

		return false;
	}
	protected virtual IEnumerator HandleHitBox() { yield return null; }
	protected virtual IEnumerator HandleAttack() { yield return null; }
}
