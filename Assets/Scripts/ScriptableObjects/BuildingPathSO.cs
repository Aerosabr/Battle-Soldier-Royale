using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuildingPathSO : ScriptableObject
{
	public List<BuildingSO> buildingSO;
	public GameObject upgradesTab;
}
