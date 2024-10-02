using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArcherSound : MonoBehaviour
{
    [SerializeField] private Archer archer;
    private AudioSource audioSource;

    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip dead;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        archer.OnSoundPlay += Archer_OnSoundPlay;
    }

    private void Archer_OnSoundPlay(object sender, Archer.OnSoundPlayEventArgs e)
    {
        switch (e.state)
        {
            case Archer.State.Attacking:
                AudioSource.PlayClipAtPoint(attack, transform.position, GameManager.Instance.GetSoundVolume());
                break;
            case Archer.State.Dead:
                AudioSource.PlayClipAtPoint(dead, transform.position, GameManager.Instance.GetSoundVolume());
                break;
        }
    }
}
