using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTowerVisual : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private ArcherTower archerTower;
    [SerializeField] private List<EvolutionVisual> archerEVs;
    [SerializeField] private List<EvolutionVisual> towerEVs;
    private bool buildingCompleted;

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

    public void ChangeBuildingVisual(int buildingLevel, int buildingPhase)
    {
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
        if (buildingCompleted)
        {
            foreach (GameObject buildingPart in archerEVs[buildingLevel - 1].bodyParts)
            {
                buildingPart.SetActive(true);
            }
        }
    }

    public EvolutionVisual GetEvolutionVisual(int buildingLevel) => towerEVs[buildingLevel - 1];
    public void BuildingCompleted(bool toggle) => buildingCompleted = toggle;
}
