using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CharacterPathSO : ScriptableObject
{
    public List<CharacterSO> characterSO;
    public GameObject upgradesTab;
    public int unitCost;
    public List<int> upgradeCost;
}
