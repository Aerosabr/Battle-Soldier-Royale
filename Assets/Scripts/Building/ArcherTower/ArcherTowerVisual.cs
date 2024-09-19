using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTowerVisual : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private ArcherTower archerTower;
    [SerializeField] private List<EvolutionVisual> archerEVs;
    [SerializeField] private List<EvolutionVisual> towerEVs;
    private int buildingProgress = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimAction(int state)
    {
        anim.SetInteger("State", state);
    }

    public void Attack01()
    {
        archerTower.Attack01();

    }

    public void BuildingInProgress(float healthPercentage, int buildingLevel)
    {
        int progress = 0;
        for (int i = towerEVs[buildingLevel].bodyParts.Count; i > 0; i--)
        {
            if (healthPercentage >= 1f / i)
                progress++;
        }

        if (progress != buildingProgress)
        {
            buildingProgress = progress;
            ChangeBuildingVisual(buildingLevel, progress);
        }
    }
    
    public void ChangeBuildingVisual(int buildingLevel, int buildingPhase)
    {
        Debug.Log(buildingLevel + " " + buildingPhase);
        foreach (EvolutionVisual visual in archerEVs)
        {
            foreach (GameObject buildingPart in visual.bodyParts)
                buildingPart.SetActive(false);
        }
        foreach (EvolutionVisual visual in towerEVs)
        {
            foreach (GameObject buildingPart in visual.bodyParts)
                buildingPart.SetActive(false);
        }

        towerEVs[buildingLevel - 1].bodyParts[buildingPhase - 1].SetActive(true);
        if (buildingPhase == towerEVs[buildingLevel].bodyParts.Count)
        {
            foreach (GameObject buildingPart in archerEVs[buildingLevel - 1].bodyParts)
            {
                buildingPart.SetActive(true);
            }
        }
    }
    
}
