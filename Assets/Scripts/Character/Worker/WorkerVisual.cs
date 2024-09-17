using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerVisual : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Worker worker;
    [SerializeField] private List<EvolutionVisual> evolutionVisuals;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimAction(int state)
    {
        anim.SetInteger("State", state);
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
