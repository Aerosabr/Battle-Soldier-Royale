using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public event EventHandler OnGoldChanged;

    public enum PlayerColor
    {
        Blue,
        Red
    }
    public PlayerColor playerColor;

    [SerializeField] protected List<CardSO> loadout = new List<CardSO>();
    [SerializeField] protected List<CardSO> tempLoadout;

    [SerializeField] protected int startingGold;
    protected int Gold;
    protected float passiveGoldTimer;
    protected float passiveGoldTimerMax = .1f;

    [SerializeField] private Base homeBase;
    [SerializeField] private Vector3 spawnRotation;
    public GameObject spawnArea;

    private int numberOfWorkers = 0;
    private List<GameObject> Economy = new List<GameObject>();
    private List<GameObject> Military = new List<GameObject>();

	public int GetGold() => Gold;

    public void AddGold(int gold)
    {
        Gold += gold;
        OnGoldChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool SubtractGold(int gold)
    {
        if (Gold < gold)
            return false;

        Gold -= gold;
        OnGoldChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public List<CardSO> GetLoadout() => loadout;
    public void IncreaseCardLevel(CardSO CSO )
    {
        for (int i = 0; i < loadout.Count; i++)
        {
            if (loadout[i] == CSO)
                loadout[i].level++;
        }
    }

    public void BuildBuilding(CardSO CSO, GameObject buildingSlot)
    {

        if (playerColor == PlayerColor.Blue && !MapManager.Instance.buildingSlots[0].GetComponent<BuildingSlot>().ContainsBuilding())
        {
            Transform building = Instantiate(CSO.spawnableObject, buildingSlot.transform.position, Quaternion.Euler(0, spawnRotation.y, 0)).transform;
			building.GetComponent<Building>().InitializeBuilding(gameObject.layer, CSO, buildingSlot.GetComponent<BuildingSlot>());
		}
        else if (!MapManager.Instance.buildingSlots[2].GetComponent<BuildingSlot>().ContainsBuilding())
        {
            Transform building = Instantiate(CSO.spawnableObject, buildingSlot.transform.position, Quaternion.identity).transform;
			building.GetComponent<Building>().InitializeBuilding(gameObject.layer, CSO, buildingSlot.GetComponent<BuildingSlot>());
		}
		SubtractGold(CSO.cardCost[CSO.level - 1]);
		CSO.timesCasted++;
    }

    public void SpawnCharacter(CardSO CSO, Vector3 position)
    {

		float spawnPos = UnityEngine.Random.Range(-0.5f, 0.5f);
		Vector3 placement = new Vector3(position.x, spawnPos * 0.2f, spawnPos);
		Transform character = Instantiate(CSO.spawnableObject, transform).transform;
		character.position = placement;
		character.GetComponent<Character>().InitializeCharacter(gameObject.layer, spawnRotation, CSO);
		SubtractGold(CSO.cardCost[CSO.level - 1]);
		AddToMilitary(character.gameObject);
		CSO.timesCasted++;
    }

    public void SpawnSpell(CardSO CSO, Vector3 position)
    {
		SpellCardSO SCSO = CSO as SpellCardSO;
		Transform spell = Instantiate(SCSO.spawnableObject, transform).transform;
        spell.position = position;
		spell.GetComponent<Spell>().InitializeSpell(gameObject.layer, SCSO);
		SubtractGold(SCSO.cardCost[SCSO.level - 1]);
		CSO.timesCasted++;
    }

    public void SpawnWorker(CardSO CSO, GameObject mine)
    {
		float spawnPos = UnityEngine.Random.Range(-0.5f, 0.5f);
		Vector3 placement = new Vector3(transform.position.x, spawnPos * 0.2f, spawnPos);
		Transform character = Instantiate(CSO.spawnableObject, transform).transform;
		character.position = placement;
        character.GetComponent<Worker>().InitializeWorker(gameObject.layer, spawnRotation, CSO, mine);
		SubtractGold(CSO.cardCost[CSO.level - 1]);
		CSO.timesCasted++;
    }

    public void ProjectCard(CardSO CSO)
    {
		if (CSO.cardType == CardSO.CardType.Character)
		{
			spawnArea.gameObject.SetActive(true);
			Transform character = Instantiate(CSO.spawnableObject, transform).transform;
			StartCoroutine(character.GetComponent<Character>().Project(gameObject.layer, spawnRotation, CSO));
		}
		else if (CSO.cardType == CardSO.CardType.Worker)
		{
			MapManager.Instance.ShowPossibleMineSlotsIndicator(transform.position.x, GetFurthestControlledArea());
			Transform character = Instantiate(CSO.spawnableObject, transform).transform;
			StartCoroutine(character.GetComponent<Worker>().Project(gameObject.layer, spawnRotation, CSO));
		}
		else if (CSO.cardType == CardSO.CardType.Spell)
		{
			SpellCardSO SCSO = CSO as SpellCardSO;
			Transform spell = Instantiate(CSO.spawnableObject, transform).transform;
			StartCoroutine(spell.GetComponent<Spell>().Project(gameObject.layer, CSO));
		}
		else if (CSO.cardType == CardSO.CardType.Building)
		{
			MapManager.Instance.ShowPossibleBuildingSlotsIndicator(transform.position.x, GetFurthestControlledArea());
			Transform building = Instantiate(CSO.spawnableObject, transform.position, Quaternion.Euler(0, spawnRotation.y, 0)).transform;
			StartCoroutine(building.GetComponent<Building>().Project(gameObject.layer, CSO));
		}

	}

    public bool CheckCardPosition(float currentXPosition)
    {
        float minXRange = transform.position.x;
        float maxXRange = CheckFarthestUnit();

        if(currentXPosition > minXRange && currentXPosition < maxXRange)
            return true;
        return false;
    }

    private float CheckFarthestUnit()
    {
        float maxXRange = transform.position.x - (transform.position.x / 3);
        if (playerColor == PlayerColor.Blue)
        {
            foreach (GameObject character in Military)
            {
                if (character.transform.position.x > maxXRange)
                {
                    maxXRange = character.transform.position.x;
                }
            }
        }
        else
        {
            foreach (GameObject character in Military)
            {
                if (character.transform.position.x < maxXRange)
                {
                    maxXRange = character.transform.position.x;
                }
            }
        }
        return maxXRange;
    }

    public Vector3 GetBaseLocation() => homeBase.transform.position;

    public void AddToEconomy(GameObject character, bool isWorker)
    {
        if (isWorker)
            numberOfWorkers++;

        Economy.Add(character);
    }
    public void RemoveFromEconomy(GameObject character, bool isWorker)
    {
        if (isWorker)
            numberOfWorkers--;

        Economy.Remove(character);
    }
    public List<GameObject> GetSpawnedEconomy() => Economy;
    public int GetNumberOfWorkers() => numberOfWorkers;

    public void AddToMilitary(GameObject character) => Military.Add(character);
    public void RemoveFromMilitary(GameObject character) => Military.Remove(character);
    public List<GameObject> GetSpawnedMilitary() => Military;   

    public Vector3 GetFurthestUnitPos()
    {
        Vector3 unitPos = homeBase.transform.position;
        if (playerColor == PlayerColor.Blue)
        {
            foreach (GameObject unit in Military)
            {
                if (unit.transform.position.x > unitPos.x)
                    unitPos = unit.transform.position;
            }
        }
        else
        {
            foreach (GameObject unit in Military)
            {
                if (unit.transform.position.x < unitPos.x)
                    unitPos = unit.transform.position;
            }
        }

        return unitPos;
    }

    public float GetFurthestControlledArea()
    {
        float area = 0;
        if (playerColor == PlayerColor.Blue)
        {
            float unitArea = 0;
            foreach (GameObject unit in Military)
            {
                float distance = Mathf.Abs(unit.transform.position.x - transform.position.x);
                if (distance > unitArea)
                    unitArea = distance;
            }
            area = Mathf.Max(unitArea, Mathf.Abs(MapManager.Instance.buildingSlots[0].transform.position.x));
        }
        else
        {
            float unitArea = 0;
            foreach (GameObject unit in Military)
            {
                float distance = transform.position.x - unit.transform.position.x;
                if (distance > unitArea)
                    unitArea = distance;
            }
            area = Mathf.Max(unitArea, MapManager.Instance.buildingSlots[2].transform.position.x);
        }

        return area;
    }
}
