using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CharacterPathSO : ScriptableObject
{
    public List<CharacterSO> characterSO;
    public int Level;
}
