using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSound : MonoBehaviour
{
    [SerializeField] private Wizard wizard;
    private AudioSource audioSource;

    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip dead;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        wizard.OnSoundPlay += Wizard_OnSoundPlay;
    }

    private void Wizard_OnSoundPlay(object sender, Wizard.OnSoundPlayEventArgs e)
    {
        switch (e.state)
        {
            case Wizard.State.Attacking:
                AudioSource.PlayClipAtPoint(attack, e.pos, GameManager.Instance.GetSoundVolume());
                break;
            case Wizard.State.Dead:
                AudioSource.PlayClipAtPoint(dead, e.pos, GameManager.Instance.GetSoundVolume());
                break;
        }
    }
}
