using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanVisual : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Swordsman character;

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
