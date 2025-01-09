using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFootsteps : StateMachineBehaviour
{
    [SerializeField] private string soundName;
    [SerializeField, Range(0, 1)] private float volume = 1;
    private AudioSource audioSource;
    private AudioClip footstepClip;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audioSource = animator.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            footstepClip = SoundManager.GetAudioClip(soundName);
            if (footstepClip != null)
            {
                audioSource.loop = true;
                audioSource.clip = footstepClip;
                audioSource.volume = volume;
                audioSource.Play();
                Debug.Log("Footstep sound started");
            }
            else
            {
                Debug.LogError("Footstep audio clip not found");
            }
        }
        else
        {
            Debug.LogError("AudioSource component not found on the animator's GameObject");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.loop = false;
            Debug.Log("Footstep sound stopped");
        }
    }
}