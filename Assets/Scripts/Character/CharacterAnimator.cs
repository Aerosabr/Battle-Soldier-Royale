using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Character character;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimAction(string action, bool toggle)
    {
        anim.SetBool(action, toggle);
    }
}
