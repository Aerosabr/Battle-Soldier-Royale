using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class CharacterCardSO : CardSO
{
    public List<int> Health;
    public List<int> Attack;
    public List<float> AttackSpeed;
    public List<float> MoveSpeed;
    public float AttackRange;
    public AttackType AttackType;
}
