using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardVisual : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Wizard character;

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
        character.Attack01();

    }
}
