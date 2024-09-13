using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerVisual : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Worker worker;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimAction(int state)
    {
        anim.SetInteger("State", state);
    }
}
