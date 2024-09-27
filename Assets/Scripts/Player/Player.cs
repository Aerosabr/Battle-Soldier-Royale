using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private Vector3 spawnRotation;

    private int numberOfWorkers = 0;
    private List<GameObject> Economy = new List<GameObject>();
    private List<GameObject> Military = new List<GameObject>();

	private bool response = false;


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

    public void SpawnCharacter(CardSO CSO)
    {
        SubtractGold(CSO.cardCost[CSO.level - 1]);

        Transform character = Instantiate(CSO.spawnableObject, transform).transform;
        float spawnPos = UnityEngine.Random.Range(-0.5f, 0.5f);
        character.transform.position = new Vector3(transform.position.x, spawnPos * 0.2f, spawnPos);
        character.GetComponent<Character>().InitializeCharacter(gameObject.layer, spawnRotation, CSO);
        CSO.timesCasted++;
    }

    public void SpawnSpell(CardSO CSO)
    {
        Transform spell = Instantiate(CSO.spawnableObject, transform).transform;
        StartCoroutine(spell.GetComponent<Spell>().Project(gameObject.layer, CSO.damageOutput[CSO.level - 1], CSO.cardCost[CSO.level - 1]));
        CSO.timesCasted++;
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

    public void BuildBuilding(CardSO CSO, GameObject buildingSlot)
    {
        SubtractGold(CSO.cardCost[CSO.level - 1]);
        if (playerColor == PlayerColor.Blue && !MapManager.Instance.buildingSlots[0].GetComponent<BuildingSlot>().ContainsBuilding())
        {
            Transform building = Instantiate(CSO.spawnableObject, MapManager.Instance.buildingSlots[0].transform.position, Quaternion.Euler(0, spawnRotation.y, 0)).transform;
            building.GetComponent<Building>().InitializeBuilding(gameObject.layer, CSO, MapManager.Instance.buildingSlots[0].GetComponent<BuildingSlot>());
        }
        else if (!MapManager.Instance.buildingSlots[2].GetComponent<BuildingSlot>().ContainsBuilding())
        {
            Transform building = Instantiate(CSO.spawnableObject, MapManager.Instance.buildingSlots[2].transform.position, Quaternion.Euler(0, spawnRotation.y, 0)).transform;
            building.GetComponent<Building>().InitializeBuilding(gameObject.layer, CSO, MapManager.Instance.buildingSlots[2].GetComponent<BuildingSlot>());
        }

        CSO.timesCasted++;
    }

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
