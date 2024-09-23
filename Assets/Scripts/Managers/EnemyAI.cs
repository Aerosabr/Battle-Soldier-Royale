using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyAI : MonoBehaviour
{
    private State state;
    [SerializeField] private List<AIGameStateSO> gameStates;

    private float gameStateTimer;
    private float gameStateTimerMax = 1f;
    [SerializeField] private AlertMetrics alertMetrics;
    private ActionType actionType;
    //private LoadoutCard actionLoadout;

    private void Awake()
    {
        state = State.Idle;
    }

    private void Start()
    {
        PlayerRed.Instance.SpawnCharacter(PlayerRed.Instance.GetLoadout()[0]);
    }

    private void Update()
    {
        
        gameStateTimer += Time.deltaTime;
        if (gameStateTimer >= gameStateTimerMax)
        {
            DetermineGameState();
            gameStateTimer = 0f;
        }
        
    }

    private void DetermineGameState()
    {
        AlertMetrics newAlertMetrics;
        CalculateAOC(out newAlertMetrics.areaOfControl);
        CalculateEMS(out newAlertMetrics.effectiveMilitaryStrength);
        CalculateGPM(out newAlertMetrics.goldPerMinute);

        if (!alertMetrics.Equals(newAlertMetrics))
        {
            alertMetrics = newAlertMetrics;
            ChooseAction();
        }
    }

    private void ChooseAction()
    {
        int decision, randomNum = Random.Range(0, 100);
        List<int> decisionTable = new List<int>();

        foreach (AIGameStateSO gameStateSO in gameStates)
        {
            if (alertMetrics.areaOfControl == gameStateSO.AOC && alertMetrics.effectiveMilitaryStrength == gameStateSO.EMS && alertMetrics.goldPerMinute == gameStateSO.GPM)
            {
                decisionTable = gameStateSO.DecisionTable;
                break;
            }
        }

        if (randomNum < 60)
            decision = decisionTable[0];       
        else if (randomNum < 85)
            decision = decisionTable[1];
        else if (randomNum < 95)
            decision = decisionTable[2];
        else
            decision = decisionTable[3];

        switch (decision)
        {
            case 1: //Develop or Upgrade Economy
                if (PlayerRed.Instance.GetNumberOfWorkers() < GameManager.Instance.GetMaxWorkerAmount())
                {

                }
                break;
            case 2: //Upgrade Military

                break;
            case 3: //Build Defense

                break;
            case 4: //Develop Military or Cast Spell

                break;
        }
    }
    /*
    private void ExecuteAction()
    {
        int Gold = PlayerRed.Instance.GetGold();
        switch (actionType)
        {
            case ActionType.Spawn:
                if (Gold >= actionLoadout.cardPathSO.cards[actionLoadout.Level].cardCost)
                    PlayerRed.Instance.SpawnCharacter(actionLoadout.cardPathSO.cards[actionLoadout.Level].spawnableObject.gameObject);
                
                break;
            case ActionType.Upgrade:
                if (Gold >= actionLoadout.cardPathSO.upgradeCost[actionLoadout.Level - 1])
                {
                    PlayerRed.Instance.SubtractGold(actionLoadout.cardPathSO.upgradeCost[actionLoadout.Level - 1]);
                    PlayerRed.Instance.IncreaseLoadoutLevel(actionLoadout.cardPathSO);
                }
                break;
            case ActionType.Build:

                break;
            case ActionType.Cast:

                break;   
        }
    }
    */
    private void CalculateAOC(out AlertLevel areaOfControl)
    {
        float mapSizeMultiplier = GameManager.Instance.GetMapSize() / 30;
        float AOC = PlayerRed.Instance.GetFurthestControlledArea() / PlayerBlue.Instance.GetFurthestControlledArea();

        if (AOC > mapSizeMultiplier)
            areaOfControl = AlertLevel.Favored;
        else if (AOC < (1 / mapSizeMultiplier))
            areaOfControl = AlertLevel.Unfavored;
        else
            areaOfControl = AlertLevel.Even;
    }

    private void CalculateEMS(out AlertLevel effectiveMilitaryStrength)
    {
        float EMS = GetEMSFromList(PlayerRed.Instance.GetSpawnedMilitary()) / (float)GetEMSFromList(PlayerBlue.Instance.GetSpawnedMilitary());
        
        if (EMS > 2)
            effectiveMilitaryStrength = AlertLevel.Favored;
        else if (EMS < .5f)
            effectiveMilitaryStrength = AlertLevel.Unfavored;
        else
            effectiveMilitaryStrength = AlertLevel.Even;
    }

    private int GetEMSFromList(List<GameObject> MilitaryUnits)
    {
        int EMS = 0;
        foreach (GameObject unit in MilitaryUnits)
            EMS += unit.GetComponent<Character>().GetUnitStrength();

        return EMS;
    }

    private void CalculateGPM(out AlertLevel goldPerMinute)
    {
        float GPM = GetGPMFromList(PlayerRed.Instance.GetSpawnedEconomy()) / (float)GetGPMFromList(PlayerBlue.Instance.GetSpawnedEconomy());

        if (GPM > 1.5f)
            goldPerMinute = AlertLevel.Favored;
        else if (GPM < .666f)
            goldPerMinute = AlertLevel.Unfavored;
        else
            goldPerMinute = AlertLevel.Even;
    }

    private int GetGPMFromList(List<GameObject> EconomyUnits)
    {
        int GPM = 600;
        foreach (GameObject unit in EconomyUnits)
        {
            if (unit.GetComponent<Character>() != null)
                GPM += unit.GetComponent<Character>().GetAttack() * 3;
            else if (unit.GetComponent<Building>() != null)
                GPM += unit.GetComponent<Building>().GetAttack() * 60;
        }

        return GPM;
    }

    private void DevelopEconomy()
    {

    }

    private void DevelopMilitary()
    {

    }

    private void CreateDefense()
    {

    }

    private void LaunchAttack()
    {

    }

    private void UpgradeEconomy()
    {

    }

    private void UpgradeMilitary()
    {

    }
}

public enum AlertLevel
{
    Unfavored,
    Even,
    Favored
}

public enum State
{
    Idle,
    DevelopingEconomy,
    DevelopingMilitary,
    CreatingDefenses,
    LaunchingAttack,
    UpgradingEconomy,
    UpgradingMilitary,
}

[System.Serializable]
public struct AlertMetrics
{
    public AlertLevel areaOfControl;
    public AlertLevel effectiveMilitaryStrength;
    public AlertLevel goldPerMinute;
}

public enum ActionType
{
    Spawn,
    Upgrade,
    Build,
    Cast
}