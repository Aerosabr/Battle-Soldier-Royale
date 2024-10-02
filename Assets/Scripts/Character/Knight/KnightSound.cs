using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSound : MonoBehaviour
{
    [SerializeField] private Knight knight;
    private AudioSource audioSource;

    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip dead;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        knight.OnSoundPlay += Knight_OnSoundPlay;
    }

    private void Knight_OnSoundPlay(object sender, Knight.OnSoundPlayEventArgs e)
    {
        switch (e.state)
        {
            case Knight.State.Attacking:
                AudioSource.PlayClipAtPoint(attack, transform.position, GameManager.Instance.GetSoundVolume());
                break;
            case Knight.State.Dead:
                AudioSource.PlayClipAtPoint(dead, transform.position, GameManager.Instance.GetSoundVolume());
                break;
        }
    }
}
