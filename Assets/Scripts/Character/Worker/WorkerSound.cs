using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerSound : MonoBehaviour
{
    [SerializeField] private AudioClip deposit;
    [SerializeField] private AudioClip dead;

    public void Deposit()
    {
        AudioSource.PlayClipAtPoint(deposit, transform.position, GameManager.Instance.GetSoundVolume());
    }

    public void Died()
    {
        AudioSource.PlayClipAtPoint(dead, transform.position, GameManager.Instance.GetSoundVolume());
    }
}
