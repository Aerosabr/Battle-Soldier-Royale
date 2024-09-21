using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmVisual : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Farm farm;
    [SerializeField] private List<EvolutionVisual> farmEVs;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimAction(string action)
    {
        anim.SetTrigger(action);
    }

    public void ChangeBuildingVisual(int buildingLevel, int buildingPhase)
    {
        foreach (EvolutionVisual visual in farmEVs)
        {
            foreach (GameObject buildingPart in visual.bodyParts)
                buildingPart.SetActive(false);
        }

        farmEVs[buildingLevel - 1].bodyParts[buildingPhase - 1].SetActive(true);
    }

    public EvolutionVisual GetEvolutionVisual(int buildingLevel) => farmEVs[buildingLevel - 1];
}
