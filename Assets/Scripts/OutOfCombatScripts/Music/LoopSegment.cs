using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopSegment : MonoBehaviour
{
    [SerializeField]
    public AudioClip audioClip; // The audio clip to play
    private AudioSource audioSource; // AudioSource component

    public float loopStart = 14f; // Loop start time in seconds
    public float loopEnd = 63f;  // Loop end time in seconds
    public float fadeInDuration = 2f; // Duration of the fade-in effect in seconds

    private static bool hasPlayedIntro = false; // Tracks whether the intro has been played

    void Start()
    {
        // Ensure there is an AudioSource on the GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.loop = false; // Disable automatic looping
        audioSource.volume = 0f; // Start at 0 volume for the fade-in

        // Check if the intro has been played
        if (!hasPlayedIntro)
        {
            // Play the intro from the start
            audioSource.time = 0f; // Start from the beginning
            audioSource.Play();

            // Schedule to start looping after the intro finishes
            Invoke(nameof(StartLooping), audioClip.length);

            // Mark the intro as played
            hasPlayedIntro = true;
        }
        else
        {
            // Skip directly to looping part
            StartLooping();
        }

        // Start the fade-in effect
        StartCoroutine(FadeIn());
    }

    void StartLooping()
    {
        // Start looping between loopStart and loopEnd
        audioSource.time = loopStart;
        audioSource.Play();
    }

    void Update()
    {
        // Check if the current playback time exceeds the loop end time
        if (audioSource.isPlaying && audioSource.time >= loopEnd)
        {
            // Reset to the loop start time and play again
            audioSource.time = loopStart;
            audioSource.Play();
        }
    }

    // Coroutine to handle fade-in
    private System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            // Gradually increase the volume over time
            audioSource.volume = Mathf.Lerp(0f, 0.5f, elapsedTime / fadeInDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the volume is set to 1 at the end of the fade
        audioSource.volume = 0.5f;
    }
}
