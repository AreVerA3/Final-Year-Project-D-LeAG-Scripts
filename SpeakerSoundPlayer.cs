using UnityEngine;
using UnityEngine.UI; // Required to talk to the Button component

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class SpeakerSoundPlayer : MonoBehaviour
{
    [Header("Sound Setup")]
    public AudioClip clickSound; // Drag your sound file here

    [Range(0.1f, 1.0f)]
    public float volume = 0.4f;

    private Button button;
    private AudioSource audioSource;

    void Awake()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();

        // Set up the speaker rules automatically so you don't have to
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f; // Forces it to be 2D so it always plays clearly

        // Automatically tell the button to play the sound when clicked
        button.onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        if (clickSound != null)
        {
            // Subtle pitch randomization so it feels satisfying and natural
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(clickSound, volume);
        }
        else
        {
            Debug.LogWarning("Hey! You forgot to drag the sound file onto this button: " + gameObject.name);
        }
    }

    void OnDestroy()
    {
        // Clean up the listener when switching scenes
        if (button != null)
        {
            button.onClick.RemoveListener(PlaySound);
        }
    }
}