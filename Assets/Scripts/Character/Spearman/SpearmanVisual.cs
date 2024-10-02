using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanVisual : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Spearman character;
    [SerializeField] private List<EvolutionVisual> evolutionVisuals;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimAction(Spearman.State state)
    {
        int stateInt = 0;
        switch (state)
        {
            case Spearman.State.Idle:
                stateInt = 0;
                break;
            case Spearman.State.Walking:
                stateInt = 1;
                break;
            case Spearman.State.Attacking:
                stateInt = 2;
                break;
            case Spearman.State.Dead:
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
