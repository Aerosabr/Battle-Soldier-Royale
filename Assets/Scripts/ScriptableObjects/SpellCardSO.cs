using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType
{
    Immediate,
    Poison,
    Slow,
}


[CreateAssetMenu()]
public class SpellCardSO : CardSO
{
    public List<int> Attack;
    public int Size;
    public float Duration;
    public int PostSpellDuration;

}
