using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public event EventHandler OnGoldChanged;

    public Spawner spawner;

    [SerializeField] private List<LoadoutCharacter> loadout = new List<LoadoutCharacter>();

    [SerializeField] private List<CharacterPathSO> tempLoadout;

    [SerializeField] private int startingGold;
    private int Gold;

    private float passiveGoldTimer;
    private float passiveGoldTimerMax = 1f;

    private void Awake()
    {
        Instance = this;
        AddGold(startingGold);
        foreach (CharacterPathSO CPSO in tempLoadout)
        {
            LoadoutCharacter temp = new LoadoutCharacter(CPSO, 1);
            loadout.Add(temp);
        }
    }

    private void Update()
    {
        passiveGoldTimer += Time.deltaTime;
        if (passiveGoldTimer >= passiveGoldTimerMax)
        {
            AddGold(1);
            passiveGoldTimer = 0;
        }
    }

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

}
