using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene-checking logic!

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Automatically grab the AudioSource component attached to this GameObject
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // Tell Unity to watch for scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks when the object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This function automatically triggers whenever a new scene loads
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (audioSource == null) return;

        if (scene.name == "Register" || scene.name == "Profile")
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                Debug.Log("Background music paused for game scene focus.");
            }
        }
        else
        {
            // ▶️ Resume the music automatically when going back to menus/wardrobe
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log("Background music resumed.");
            }
        }
    }
}