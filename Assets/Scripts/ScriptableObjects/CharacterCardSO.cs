using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharacterType
{
    Worker,
    Melee,
    Ranged
}

public enum AttackType
{
    None,
    Single,
    AOE
}

[CreateAssetMenu()]
public class CharacterCardSO : CardSO
{
    public List<int> Health;
    public List<int> Attack;
    public List<float> AttackSpeed;
    public List<float> MoveSpeed;
    public float AttackRange;
    public CharacterType CharacterType;
    public AttackType AttackType;
}
