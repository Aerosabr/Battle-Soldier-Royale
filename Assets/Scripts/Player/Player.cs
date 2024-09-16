using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LoadoutCharacter
{
    public CharacterPathSO characterPathSO;
    public int Level;

    public LoadoutCharacter(CharacterPathSO characterPathSO, int Level)
    {
        this.characterPathSO = characterPathSO;
        this.Level = Level;
    }

    public void IncreaseLevel()
    {
        Level += 1;
    }
}

public class Player : MonoBehaviour
{
    public event EventHandler OnGoldChanged;

    public enum PlayerColor
    {
        Blue,
        Red
    }
    public PlayerColor playerColor;

    [SerializeField] protected List<LoadoutCharacter> loadout = new List<LoadoutCharacter>();
    [SerializeField] protected List<CharacterPathSO> tempLoadout;

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

    public List<LoadoutCharacter> GetLoadout() => loadout;
    public void IncreaseLoadoutLevel(CharacterPathSO CPSO)
    {
        for (int i = 0; i < loadout.Count; i++)
        {
            if (loadout[i].characterPathSO == CPSO)
                loadout[i] = new LoadoutCharacter(loadout[i].characterPathSO, loadout[i].Level + 1);
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
