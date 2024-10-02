using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArcherVisual : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Archer character;
    [SerializeField] private List<EvolutionVisual> evolutionVisuals;
    public bool active = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimAction(int state)
    {
        if (!active)
            return;

        anim.SetInteger("State", state);
    }

    public void Attack01()
    {
        character.Attack01();
    }

    public void ActivateEvolutionVisual(int level)
    {
        foreach (EvolutionVisual visual in evolutionVisuals)
        {
            foreach (GameObject bodyPart in visual.bodyParts)
                bodyPart.SetActive(false);
        }

        foreach (GameObject bodyPart in evolutionVisuals[level - 1].bodyParts)
        {
            bodyPart.SetActive(true);
        }
    }
}
