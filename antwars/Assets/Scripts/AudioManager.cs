using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    public AudioMixer audioMixer;
    public List<AudioClip> audioClips;
    public AudioSource audioSource;
    int currentclip;
    private void Start()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();
        currentclip++;
        Invoke("Increment", audioSource.clip.length);
    }
    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }
    public void Increment()
    {
        if(currentclip > audioClips.Count-1)
        {
            currentclip = 0;
        }
        audioSource.clip = audioClips[currentclip];
        audioSource.Play();
        currentclip++;
    }
}
