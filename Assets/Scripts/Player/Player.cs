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
    protected float passiveGoldTimerMax = 1f;

    [SerializeField] private Vector3 spawnRotation;

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

    public void SpawnCharacter(GameObject characterToSpawn)
    {
        SubtractGold(characterToSpawn.GetComponent<Character>().GetCost());
        Transform character = Instantiate(characterToSpawn, transform).transform;
        float spawnPos = UnityEngine.Random.Range(-0.5f, 0.5f);
        character.transform.position = new Vector3(transform.position.x, spawnPos * 0.2f, spawnPos);
        character.GetComponent<Character>().InitializeCharacter(gameObject.layer, spawnRotation);
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
}
