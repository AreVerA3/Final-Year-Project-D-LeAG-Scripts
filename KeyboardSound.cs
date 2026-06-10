using UnityEngine;
using UnityEngine.UI; // Crucial for using classic/legacy UI components like InputField

public class KeyboardSound : MonoBehaviour
{
    public static KeyboardSound Instance;

    [Header("UI Elements")]
    public InputField wordInputField; // This will now accept your classic Input Field!

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    [Range(0.1f, 0.5f)]
    public float volume = 0.25f; 

    [Header("Pitch Randomization")]
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Call this when an alphabet button is clicked
    public void TypeLetter(string letter)
    {
        if (wordInputField != null)
        {
            // Append the letter to the legacy input field text
            wordInputField.text += letter;
            
            // Play the keyboard click noise
            PlayKeyPressSound();
        }
    }

    private void PlayKeyPressSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(clickSound, volume);
        }
    }
}