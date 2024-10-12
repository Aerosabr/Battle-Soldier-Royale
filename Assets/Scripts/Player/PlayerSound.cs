using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioClip unitSpawned;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void UnitSpawned(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(unitSpawned, pos, GameManager.Instance.GetSoundVolume());
    }
}
