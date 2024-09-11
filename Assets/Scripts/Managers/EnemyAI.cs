using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
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

    private State state;

    private float goldPerMinute;
    private float numWorkers;
    private float effectiveMilitaryStrength;
    private float areaOfControl;
    private float enemiesInProximity;

    private void DetermineGameState()
    {
        
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