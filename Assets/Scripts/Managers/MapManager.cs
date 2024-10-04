using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public List<GameObject> buildingSlots = new List<GameObject>();
    public List<GameObject> mines = new List<GameObject>();
    public Player currentPlayer;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowPossibleBuildingSlotsIndicator(float start, float end)
    {
        end = start + end;
        for(int i = 0; i < buildingSlots.Count; i++)
        { 
            if (buildingSlots[i].transform.position.x >= start && buildingSlots[i].transform.position.x <= end && !buildingSlots[i].transform.GetComponent<BuildingSlot>().ContainsBuilding())
            {
                buildingSlots[i].transform.GetComponent<BuildingSlot>().ShowPlacementIndicator();
            }
        }
    }
	public void HideAllBuildingSlotsIndicator()
	{
		for (int i = 0; i < buildingSlots.Count; i++)
		{
			buildingSlots[i].transform.GetComponent<BuildingSlot>().HidePlacementIndicator();
		}
	}

    public void ShowPossibleMineSlotsIndicator(float start, float end)
    {
		end = start + end;
		for (int i = 0; i < mines.Count; i++)
		{
			if (mines[i].transform.position.x >= start && mines[i].transform.position.x <= end)
			{
                mines[i].transform.GetChild(1).gameObject.SetActive(true);
			}
		}
	}
	public void HideAllMineSlotsIndicator()
	{
		for (int i = 0; i < mines.Count; i++)
		{
			mines[i].transform.GetChild(1).gameObject.SetActive(false);
		}
	}
}
