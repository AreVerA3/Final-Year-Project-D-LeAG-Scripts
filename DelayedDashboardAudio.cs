using UnityEngine;

public class DelayedDashboardAudio : MonoBehaviour
{
    public AudioSource greetingAudio; 
    
    public float delayTime = 2f; 

    void Start()
    {
        // The moment the scene opens, this starts a countdown timer
        Invoke("PlayGreeting", delayTime);
    }

    void PlayGreeting()
    {
        // When the timer hits zero, play the sound!
        greetingAudio.Play();
    }
}