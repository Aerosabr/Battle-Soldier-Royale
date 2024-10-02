using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanSound : MonoBehaviour
{
    [SerializeField] private Swordsman swordsman;
    private AudioSource audioSource;

    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip dead;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        swordsman.OnSoundPlay += Swordsman_OnSoundPlay;
    }

    private void Swordsman_OnSoundPlay(object sender, Swordsman.OnSoundPlayEventArgs e)
    {
        switch (e.state)
        {
            case Swordsman.State.Attacking:
                AudioSource.PlayClipAtPoint(attack, transform.position, GameManager.Instance.GetSoundVolume());
                break;
            case Swordsman.State.Dead:
                AudioSource.PlayClipAtPoint(dead, transform.position, GameManager.Instance.GetSoundVolume());
                break;
        }
    }
}
