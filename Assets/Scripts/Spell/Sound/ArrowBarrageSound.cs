using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBarrageSound : MonoBehaviour
{
    [SerializeField] private AudioClip barrage;

    private void CreateAudioObject(AudioClip clip, Vector3 position, float volume)
    {
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        gameObject.transform.SetParent(transform);
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.Play();
        Object.Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    public void Attack()
    {
        CreateAudioObject(barrage, transform.position, GameManager.Instance.GetSoundVolume());
    }
}
