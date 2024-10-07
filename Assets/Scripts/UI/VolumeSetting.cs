using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
	[SerializeField] private Slider MusicSlider;
	[SerializeField] private Slider SoundSlider;


	private void Start()
	{
		MusicSlider.value = GameManager.Instance.GetMusicVolume();
		SoundSlider.value = GameManager.Instance.GetSoundVolume();
	}
	public void MusicVolumeChange(float volume)
    {
        GameManager.Instance.SetMusicVolume(volume);
    }
	public void SoundVolumeChange(float volume)
	{
		GameManager.Instance.SetSoundVolume(volume);
	}
}
