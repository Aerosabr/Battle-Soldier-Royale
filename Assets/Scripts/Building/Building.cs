using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Building : Entity, IDamageable, IEffectable
{
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;
    public event EventHandler<IDamageable.OnDamageTakenEventArgs> OnDamageTaken;

    protected float baseAttack;
    protected float attack;
    protected float baseAttackSpeed;
    protected float attackSpeed;
    protected float attackRange;
    protected float buildTimer;

    protected BuildingCardSO card;
    protected BuildingSlot buildingSlot;

    public virtual void Damaged(float damage, CardSO.CardType cardType) { }
    protected void HealthChangedVisual()
    {
        OnHealthChanged?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthPercentage = currentHealth / maxHealth
        });
    }
    protected void DamageTakenVisual(float damage)
    {
        OnDamageTaken?.Invoke(this, new IDamageable.OnDamageTakenEventArgs
        {
            damage = damage
        });
    }
	public virtual void InitializeBuilding(LayerMask layerMask, CardSO card, BuildingSlot buildingSlot) { }

	protected virtual void BuildingBuilt() => Debug.Log("Built not implemented");
    protected virtual void BuildingDestroyed() => Debug.Log("Destroyed not implemented");
    public float GetAttack() => attack;
	public virtual IEnumerator Project(LayerMask layerMask, CardSO card)
	{
		bool OnPlaceable = false;
		transform.GetComponent<BoxCollider>().enabled = false;
		transform.gameObject.layer = 0;
		BuildingSlot slot = null;
		int neutralWallLayer = LayerMask.NameToLayer("NeutralWall");
		LayerMask neutralWallMask = 1 << neutralWallLayer;
		int buildableWallLayer = LayerMask.NameToLayer("BuildingWall");
		LayerMask buildableWallMask = 1 << buildableWallLayer;
		if (layerMask == 6)
		{
			player = PlayerBlue.Instance;
		}
		else
		{
			player = PlayerRed.Instance;
		}
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

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildableWallMask))
			{
				if (!hit.transform.parent.GetComponent<BuildingSlot>().ContainsBuilding())
				{
					OnPlaceable = true;
					slot = hit.transform.parent.GetComponent<BuildingSlot>();
					Transform hitTransform = hit.transform;
					transform.position = hitTransform.position;
				}
			}
			else if (Physics.Raycast(ray, out hit, Mathf.Infinity, neutralWallMask))
			{
				OnPlaceable = false;
				Vector3 worldPosition = hit.point;
				transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);
			}
			player.CheckAvailableBuildingIndicator();
			yield return null;
		}

		if (IsMouseOverUI() || OnPlaceable == false)
		{
			Destroy(gameObject);
		}
		else
		{
			if (layerMask == 6)
			{
				PlayerBlue.Instance.BuildBuilding(card, slot.gameObject);
			}
			else
			{
				PlayerRed.Instance.BuildBuilding(card, slot.gameObject);
			}
			CharacterBarUI.Instance.ActivateCooldown();
			Destroy(gameObject);
		}
		CharacterBarUI.Instance.ShowCharacterBar();
		MapManager.Instance.HideAllBuildingSlotsIndicator();
		PlayerControlManager.Instance.CardHandled();
	}
	protected bool IsMouseOverUI()
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

	public BuildingCardSO GetCard() => card;
	#region IEffectable Components
	public void Slowed(int speed)
	{
		Slowed existingSlowed = GetComponent<Slowed>();

		if (existingSlowed == null)
		{
			Slowed newSlowed = gameObject.AddComponent<Slowed>();
			attackSpeed = ((100 - (float)speed) / 100) * attackSpeed;
		}
	}
	public void UnSlowed(int speed)
	{
		Slowed existingSlowed = GetComponent<Slowed>();

		if (existingSlowed == null)
		{
			attackSpeed = baseAttackSpeed;
			Destroy(existingSlowed);
		}
	}

	public void Poisoned(float damage, float poisonDuration)
	{
		Poisoned existingPoisoned = GetComponent<Poisoned>();

		if (existingPoisoned == null)
		{
			Poisoned newPoisoned = gameObject.AddComponent<Poisoned>();
			newPoisoned.UpdatePoison(damage, poisonDuration);
		}
		else
		{
			existingPoisoned.UpdatePoison(damage, poisonDuration);
		}
	}
	public void ReduceAttack(float damageReduced)
	{
		AttackReduction existingAttack = GetComponent<AttackReduction>();
		if (existingAttack == null)
		{
			existingAttack = gameObject.AddComponent<AttackReduction>();
			attack = ((100 - damageReduced) / 100) * attack;
		}

	}
	public void UnReduceAttack()
	{
		AttackReduction existingAttack = GetComponent<AttackReduction>();
		if (existingAttack != null)
		{
			attack = baseAttack;
			Destroy(existingAttack);
		}
	}
	#endregion
}
