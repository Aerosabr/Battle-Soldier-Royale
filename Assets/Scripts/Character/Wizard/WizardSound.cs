using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSound : MonoBehaviour
{
    [SerializeField] private Wizard wizard;
    private AudioSource audioSource;

    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip damaged;
    [SerializeField] private AudioClip dead;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Attack(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(attack, pos, GameManager.Instance.GetSoundVolume());
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
