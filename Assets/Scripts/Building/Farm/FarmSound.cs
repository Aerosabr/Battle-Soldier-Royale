using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmSound : MonoBehaviour
{
    [SerializeField] private AudioClip finishBuilding;
    private AudioSource building;

    private void Awake()
    {
        building = GetComponent<AudioSource>();
    }

    public void Building(bool toggle)
    {
        if (toggle)
            building.Play();
        else
            building.Stop();
    }

    public void FinishBuilding() => AudioSource.PlayClipAtPoint(finishBuilding, transform.position, GameManager.Instance.GetSoundVolume());
}
