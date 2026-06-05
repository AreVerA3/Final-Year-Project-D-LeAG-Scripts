using UnityEngine;

public class DelayedAudio : MonoBehaviour
{
    public float initialDelay = 2f;  // Waits 2 seconds the very first time
    public float repeatTimer = 10f;  // Waits 10 seconds between repeats
    private AudioSource myAudio;

    void OnEnable()
    {
        myAudio = GetComponent<AudioSource>();
        
        // This starts a looping timer right when the page opens
        InvokeRepeating("CheckAndPlay", initialDelay, repeatTimer);
    }

    void CheckAndPlay()
    {
        // If they already clicked confirm, kill the repeating timer permanently!
        if (PlayerPrefs.GetInt("NameFinished", 0) == 1)
        {
            CancelInvoke("CheckAndPlay");
        }
        else
        {
            // If they haven't finished, play the reminder
            myAudio.Play();
        }
    }
}