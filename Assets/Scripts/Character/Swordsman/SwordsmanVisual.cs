using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanVisual : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Swordsman character;
    [SerializeField] private List<EvolutionVisual> evolutionVisuals;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimAction(Swordsman.State state)
    {
        int stateInt = 0;
        switch (state)
        {
            case Swordsman.State.Idle:
                stateInt = 0;
                break;
            case Swordsman.State.Walking:
                stateInt = 1;
                break;
            case Swordsman.State.Attacking:
                stateInt = 2;
                break;
            case Swordsman.State.Dead:
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
