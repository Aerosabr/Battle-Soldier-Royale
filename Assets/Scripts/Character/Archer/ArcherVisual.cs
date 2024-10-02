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

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimAction(Archer.State state)
    {
        int stateInt = 0;
        switch (state)
        {
            case Archer.State.Idle:
                stateInt = 0;
                break;
            case Archer.State.Walking:
                stateInt = 1;
                break;
            case Archer.State.Attacking:
                stateInt = 2;
                break;
            case Archer.State.Dead:
                stateInt = 3;
                break;
        }

        anim.SetInteger("State", stateInt);
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
