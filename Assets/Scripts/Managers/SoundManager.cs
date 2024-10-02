using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private SoundSO soundRefs;

    private void Awake()
    {
        Instance = this;
    }

    //Reworked "AudioSource.PlayClipAtPoint" to attach GO to SoundManager
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

    public void MoneySpent()
    {
        CreateAudioObject(soundRefs.moneySpent, transform.position, GameManager.Instance.GetSoundVolume());
    }

    public void CardUpgraded()
    {
        CreateAudioObject(soundRefs.moneySpent, transform.position, GameManager.Instance.GetSoundVolume());
        CreateAudioObject(soundRefs.cardUpgraded, transform.position, GameManager.Instance.GetSoundVolume());
    }
}
