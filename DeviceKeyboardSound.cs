using UnityEngine;
using UnityEngine.UI; // Required for the Legacy InputField

[RequireComponent(typeof(InputField))]
[RequireComponent(typeof(AudioSource))]
public class DeviceKeyboardSound : MonoBehaviour
{
    private InputField inputField;
    private AudioSource audioSource;

    [Header("Sound Settings")]
    public AudioClip clickSound; // Drag your asset file here
    [Range(0.1f, 0.5f)] public float volume = 0.25f;

    void Awake()
    {
        inputField = GetComponent<InputField>();
        audioSource = GetComponent<AudioSource>();

        // Ensure the Audio Source doesn't play automatically on start
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        // Tell the InputField to run our function every time the text changes
        inputField.onValueChanged.AddListener(OnTextChanged);
    }

    // This runs automatically whenever the kid types or deletes a letter
    void OnTextChanged(string currentText)
    {
        // Only play the sound if they actually typed something (ignores empty resets)
        if (clickSound != null)
        {
            // Subtle pitch variation so it sounds like a real device keyboard
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(clickSound, volume);
        }
    }

    void OnDestroy()
    {
        // Clean up the listener when the scene closes
        if (inputField != null)
        {
            inputField.onValueChanged.RemoveListener(OnTextChanged);
        }
    }
}