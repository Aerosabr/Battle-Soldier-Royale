using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanAnimator : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Swordsman character;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimAction(string action, bool toggle)
    {
        anim.SetBool(action, toggle);
    }

    public void Attack01()
    {
        character.Attack01();
        
    }
}
