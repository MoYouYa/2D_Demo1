using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{
    //[Header("Audio Name")]
    public string name;

    //[Header("Audio Source")]
    public AudioClip audioClip;

    //[Header("Audio Mixer Group")]
    public AudioMixerGroup mixerGroup;

}

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static AudioManager instance;
    public AudioMixer audioMixer;
    public List<Sound> sounds;
    private Dictionary<string,AudioSource> audioSources;

    public static AudioManager Instance
    {
        get { 
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }
            return instance; 
        }
    }

    private void Awake()
    {
        audioSources= new Dictionary<string,AudioSource>();

        foreach (Sound sound in sounds)
        {
            AudioSource audioSource = this.AddComponent<AudioSource>();
            audioSource.clip=sound.audioClip;
            audioSource.outputAudioMixerGroup=sound.mixerGroup;
            audioSource.loop = sound.mixerGroup.name.Equals("BGM") ? true : false;
            audioSource.playOnAwake = false;

            audioSources.Add(sound.name,audioSource);
        }
    }

    private void Start()
    {

    }

    public void PlayAudio(string name)
    {
        if (audioSources.ContainsKey(name))
        {
            if (audioSources[name].outputAudioMixerGroup.name.Equals("BGM"))
            {
                if (!audioSources[name].isPlaying)
                {
                    audioSources[name].Play();
                }
            }
            else
            {
                audioSources[name].Play();
            }
        }
    }

    public void StopAudio(string name)
    {
        if(audioSources.ContainsKey(name) && audioSources[name].isPlaying)
        {
            audioSources[name].Stop();
        }
    }

    public void SetBGMVolum(Slider slider)
    {
        audioMixer.SetFloat("BGM", slider.value);
    }

    public void SetSFXVolum(Slider slider)
    {
        audioMixer.SetFloat("SFX", slider.value);
    }
}
