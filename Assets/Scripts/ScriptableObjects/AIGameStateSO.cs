using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIGameStateSO : ScriptableObject
{
    public AlertLevel AOC;
    public AlertLevel EMS;
    public AlertLevel GPM;

    public List<int> DecisionTable;
}
