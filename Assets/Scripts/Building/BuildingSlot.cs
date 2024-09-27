using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSlot : MonoBehaviour
{
    private Building building;
    [SerializeField] private GameObject buildingSlotVisual;
    private bool containsBuilding;

    public void SetBuilding(Building building)
    {
        this.building = building;
        buildingSlotVisual.SetActive(false);
        containsBuilding = true;
    }

    public void RemoveBuilding()
    {
        building = null;
        buildingSlotVisual.SetActive(true);
        containsBuilding = false;
    }

    public bool ContainsBuilding() => containsBuilding;
    public Building GetBuilding() => building;
}
