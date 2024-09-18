using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTowerVisual : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private ArcherTower archerTower;
    [SerializeField] private List<EvolutionVisual> archerEVs;
    [SerializeField] private List<EvolutionVisual> towerEVs;

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
    /*
    public void ActivateEvolutionVisual(int level)
    {
        foreach (EvolutionVisual visual in evolutionVisuals)
        {
            foreach (GameObject buildingPart in visual.bodyParts)
                buildingPart.SetActive(false);
        }

        foreach (GameObject buildingPart in evolutionVisuals[level - 1].bodyParts)
        {
            buildingPart.SetActive(true);
        }
    }
    */
}
