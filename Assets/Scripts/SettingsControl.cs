using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.Events;

public class SettingsControl : MonoBehaviour
{
    [SerializeField] AudioMixer Mixer;

    public UnityEvent<float> OnSetSound;
    public UnityEvent<float> OnSetMusic; 

    public void SetSound(float value)
    {
        Mixer.SetFloat("Sound", value); 
    }

    public void SetMusic(float value)
    {
        Mixer.SetFloat("Music", value); 
    } 
 
    // Use this for initialization
    void Start()
    {
        float Sound;
        float Music;
        Mixer.GetFloat("Sound",out Sound);
        Mixer.GetFloat("Music", out Music);
        OnSetSound.Invoke(Sound);
        OnSetMusic.Invoke(Music);
    }
}