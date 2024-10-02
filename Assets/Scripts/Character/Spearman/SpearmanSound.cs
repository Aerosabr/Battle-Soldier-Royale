using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanSound : MonoBehaviour
{
    [SerializeField] private Spearman Spearman;
    private AudioSource audioSource;

    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip dead;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Spearman.OnSoundPlay += Spearman_OnSoundPlay;
    }

    private void Spearman_OnSoundPlay(object sender, Spearman.OnSoundPlayEventArgs e)
    {
        switch (e.state)
        {
            case Spearman.State.Attacking:
                AudioSource.PlayClipAtPoint(attack, transform.position, GameManager.Instance.GetSoundVolume());
                break;
            case Spearman.State.Dead:
                AudioSource.PlayClipAtPoint(dead, transform.position, GameManager.Instance.GetSoundVolume());
                break;
        }
    }
}
