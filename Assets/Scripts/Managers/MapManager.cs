using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public List<GameObject> buildingSlots = new List<GameObject>();
    public List<GameObject> mines = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }
}
