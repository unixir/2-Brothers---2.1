using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public float musicVol, sfxVol;
    public Slider musicSlider, sfxSlider;
    public AudioSource musicAudioSource;
    public AudioSource[] sfxAudioSources;
    private void Start()
    {
        musicVol=musicSlider.value;
        sfxVol = sfxSlider.value;
    }

    public void ChangeMusicVol()
    {
        musicVol= musicSlider.value/100;
        musicAudioSource.volume = musicVol;
    }

    public void ChangeSFXVol()
    {
        sfxVol = sfxSlider.value / 100;
        foreach(AudioSource audioSource in sfxAudioSources)
            audioSource.volume = sfxVol;
    }
}
