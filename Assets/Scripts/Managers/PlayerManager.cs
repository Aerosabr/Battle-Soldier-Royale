using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public event EventHandler OnGoldChanged;

    [SerializeField] private Spawner spawner;
    [SerializeField] private List<CharacterPathSO> loadout;

    private int Gold;

    private float passiveGoldTimer;
    private float passiveGoldTimerMax = 1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Gold = 0;
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

    public void AttachButton(Button button, GameObject character)
    {
        button.onClick.AddListener(() =>
        {
            spawner.SpawnCharacter(character);
        });
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

    public List<CharacterPathSO> GetLoadout() => loadout;
}
