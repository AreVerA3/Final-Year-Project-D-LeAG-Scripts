using UnityEngine;

public class PanelSoundPlayer : MonoBehaviour
{
    public static PanelSoundPlayer instance;

    [Header("Audio Source component")]
    public AudioSource audioSource;

    [Header("Your 3 Panel Sound Effects")]
    public AudioClip successClip;       // Audio 1
    public AudioClip failClip;          // Audio 2
    public AudioClip levelCompleteClip;  // Audio 3

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void PlaySuccessSound()
    {
        if (instance != null && instance.audioSource != null && instance.successClip != null)
            instance.audioSource.PlayOneShot(instance.successClip);
    }

    public static void PlayFailSound()
    {
        if (instance != null && instance.audioSource != null && instance.failClip != null)
            instance.audioSource.PlayOneShot(instance.failClip);
    }

    public static void PlayLevelCompleteSound()
    {
        if (instance != null && instance.audioSource != null && instance.levelCompleteClip != null)
            instance.audioSource.PlayOneShot(instance.levelCompleteClip);
    }
}