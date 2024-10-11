using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Spell : MonoBehaviour
{
	protected int targetLayer;
	protected Player player;
	protected SpellCardSO cardSO;

	private GraphicRaycaster raycaster;
	[SerializeField] protected BoxCollider hitBox;
	[SerializeField] protected Transform transparentObject;
	[SerializeField] protected Transform visualObject;

	[SerializeField] protected List<IDamageable> characters = new List<IDamageable>();

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
		float cameraDistance = 0f;
		transparentObject.gameObject.SetActive(true);
		visualObject.gameObject.SetActive(false);

		while (Mouse.current.leftButton.isPressed || (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed))
		{
			Vector2 inputPosition = Vector2.zero;

			if (Mouse.current.leftButton.isPressed)
			{
				inputPosition = Mouse.current.position.ReadValue();
			}
			else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
			{
				inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
			}

			Ray ray = Camera.main.ScreenPointToRay(inputPosition);
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
            CharacterBarUI.Instance.ActivateCooldown();
            player.SpawnSpell(cardSO, transform.position);
			Destroy(gameObject);
		}
		CharacterBarUI.Instance.ShowCharacterBar();
		PlayerControlManager.Instance.CardHandled();
	}

	private bool IsMouseOverUI()
	{
		Vector3[] corners = CharacterBarUI.Instance.GetCancelArea();
		if (corners == null)
		{
			return false;
		}

		Vector3 inputPosition = Vector3.zero;

		if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
		{
			inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
		}
		else
		{
			inputPosition = Input.mousePosition;
		}

		if (inputPosition.x >= corners[0].x && inputPosition.x <= corners[2].x &&
			inputPosition.y >= corners[0].y && inputPosition.y <= corners[2].y)
		{
			return true;
		}

		return false;
	}

	protected virtual IEnumerator HandleHitBox() { yield return null; }
	protected virtual IEnumerator HandleAttack() { yield return null; }
}
