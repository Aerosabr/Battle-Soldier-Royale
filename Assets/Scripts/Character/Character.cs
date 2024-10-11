using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Character : Entity, IDamageable, IEffectable
{
    public event EventHandler<IDamageable.OnHealthChangedEventArgs> OnHealthChanged;
    public event EventHandler<IDamageable.OnDamageTakenEventArgs> OnDamageTaken;

    protected float baseAttack;
    [SerializeField] protected float attack;
    protected float baseAttackSpeed;
    protected float attackSpeed;
    protected float baseMoveSpeed;
	[SerializeField] protected float moveSpeed;

    protected float attackRange;
    protected float deathTimer;
    protected float deathTimerMax = 3f;
    protected float poisonTimer = 0f;

    protected bool canAttack = true;

    protected CharacterCardSO card;

	[SerializeField] protected GameObject indicator;
	[SerializeField] protected Material allowed;
	[SerializeField] protected Material denied;

	public virtual void InitializeCharacter(LayerMask layerMask, Vector3 rotation, CardSO card) => Debug.Log("Initialize not implemented");
	public virtual IEnumerator Project(LayerMask layerMask, Vector3 rotation, CardSO card)
	{
		transform.GetComponent<BoxCollider>().enabled = false;
		transform.gameObject.layer = 0;
		indicator.SetActive(true);
		int neutralWallLayer = LayerMask.NameToLayer("NeutralWall");
		LayerMask neutralWallMask = 1 << neutralWallLayer;
		float hover = transform.position.y + 0.05f;
		transform.GetComponent<Rigidbody>().useGravity = false;
		MeshRenderer meshRenderer = indicator.GetComponent<MeshRenderer>();

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
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, neutralWallMask))
			{
				Vector3 worldPosition = hit.point;
				transform.position = new Vector3(worldPosition.x, hover, worldPosition.z);
				transform.rotation = Quaternion.Euler(rotation);
			}

			if (IsCharacterInSpawnArea())
				meshRenderer.material = allowed;
			else
				meshRenderer.material = denied;

			yield return null;
		}

		if (IsMouseOverUI() || !IsCharacterInSpawnArea())
		{
			Destroy(gameObject);
		}
		else
		{
			CharacterBarUI.Instance.ActivateCooldown();
			player.SpawnCharacter(card, transform.position);
			Destroy(gameObject);
		}
		CharacterBarUI.Instance.ShowCharacterBar();
		player.spawnArea.gameObject.SetActive(false);
		PlayerControlManager.Instance.CardHandled();
	}

	public float GetAttack() => attack;
    public CharacterCardSO GetCard() => card;
    public float GetUnitStrength()
    {
        float unitStrength = 0;
        unitStrength += currentHealth / 2;
        switch (card.AttackType)
        {
            case AttackType.None:
            case AttackType.Single:
                unitStrength += attack * attackSpeed * attackRange;
                break;
            case AttackType.AOE:
                unitStrength += attack * attackSpeed * attackRange * 2;
                break;
        }

        return unitStrength;
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

	protected bool IsCharacterInSpawnArea()
	{
		float distanceFromFurthest = 1.75f;
		if (player.transform.position.x < 0)
		{
			if (transform.position.x > player.transform.position.x && transform.position.x < (player.transform.position.x + player.GetFurthestControlledArea() - distanceFromFurthest))
				return true;
			else
				return false;
		}
		else
		{
			if (transform.position.x > player.transform.position.x && transform.position.x < (player.transform.position.x - player.GetFurthestControlledArea() - distanceFromFurthest))
				return true;
			else
				return false;
		}
	}

	#region IDamageable Components
	public virtual void Damaged(float damage, CardSO.CardType cardType) { }
    protected void DamageVisuals(float damage)
    {
        OnHealthChanged?.Invoke(this, new IDamageable.OnHealthChangedEventArgs
        {
            healthPercentage = currentHealth / maxHealth
        });
        /*
        OnDamageTaken?.Invoke(this, new IDamageable.OnDamageTakenEventArgs
        {
            damage = damage
        });
        */
    }
    #endregion

    #region IEffectable Components
    public void Slowed(int speed)
    {
		Slowed existingSlowed = transform.GetComponent<Slowed>();

		if (existingSlowed == null)
		{
			Slowed newSlowed = gameObject.AddComponent<Slowed>();
			moveSpeed = ((100 - (float)speed) / 100) * moveSpeed;
			attackSpeed = ((100 - (float)speed) / 100) * attackSpeed;
		}
    }
    public void UnSlowed(int speed)
    {
		Slowed existingSlowed = transform.GetComponent<Slowed>();

		if (existingSlowed != null)
		{
			moveSpeed = baseMoveSpeed;
			attackSpeed = baseAttackSpeed;
			Destroy(existingSlowed);
		}
    }
	public void Poisoned(float damage, float poisonDuration)
	{
		Poisoned existingPoisoned = transform.GetComponent<Poisoned>();

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
		if(existingAttack == null)
		{
			existingAttack = gameObject.AddComponent<AttackReduction>();
			attack = ((100 - damageReduced) / 100 ) * baseAttack;
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
