using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanSound : MonoBehaviour
{
    [SerializeField] private Spearman Spearman;
    private AudioSource audioSource;

    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip damaged;
    [SerializeField] private AudioClip dead;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Attack()
    {
        AudioSource.PlayClipAtPoint(attack, transform.position, GameManager.Instance.GetSoundVolume());
    }

    public void Damaged()
    {
        AudioSource.PlayClipAtPoint(damaged, transform.position, GameManager.Instance.GetSoundVolume());
    }

    public void Died()
    {
        AudioSource.PlayClipAtPoint(dead, transform.position, GameManager.Instance.GetSoundVolume());
    }
}
