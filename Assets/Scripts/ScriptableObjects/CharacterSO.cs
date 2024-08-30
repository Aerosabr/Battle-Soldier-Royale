using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CharacterSO : ScriptableObject
{
    public Transform character;
    public Sprite background;
    public int upgradeCost;
}
