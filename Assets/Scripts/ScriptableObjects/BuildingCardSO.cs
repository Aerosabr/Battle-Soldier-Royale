using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BuildingType
{
    Economy,
    Defense
}

[CreateAssetMenu()]
public class BuildingCardSO : CardSO
{
    public List<int> Health;
    public List<int> Attack;
    public List<float> AttackSpeed;
    public int AttackRange;
    public List<float> BuildTimer;
    public BuildingType BuildingType;
    public AttackType AttackType;
}
