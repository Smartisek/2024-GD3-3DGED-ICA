using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* https://www.youtube.com/watch?v=g5WT91Sn3hg */

//public enum SoundType
//{
//    PICKUP,
//    FOOTSTEPS,
//    CORRECT,
//    DIALOGUE,
//    PLANT,
//    BACKGROUND_MUSIC
//}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    [SerializeField] private AudioClip[] soundList;
    private AudioSource audioSource;
    private Dictionary<string, AudioClip> soundDictionary;

    private void Awake()
    {
        instance = this;
        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in soundList)
        {
            soundDictionary[clip.name] = clip;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(string soundName, float volume = 1)
    {
        if (instance.soundDictionary.TryGetValue(soundName, out var clip))
        {
            instance.audioSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    public static void PlayLoopingSound(string soundName, float volume = 1)
    {
        if (instance.soundDictionary.TryGetValue(soundName, out var clip))
        {
            instance.audioSource.loop = true;
            instance.audioSource.clip = clip;
            instance.audioSource.volume = volume;
            instance.audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    public static void StopLoopingSound()
    {
        instance.audioSource.Stop();
        instance.audioSource.loop = false;
    }

    public static AudioClip GetAudioClip(string soundName)
    {
        if (instance.soundDictionary.TryGetValue(soundName, out var clip))
        {
            return clip;
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
            return null;
        }
    }
}
