using UnityEngine;

public class ButtonSoundPlayer : MonoBehaviour
{
    public static ButtonSoundPlayer instance;

    [Header("Audio Source component")]
    public AudioSource audioSource;

    [Header("Sfx")]
    public AudioClip buttonSound;
    public AudioClip bubbleSound;
    public AudioClip LogoutSound;
    public AudioClip KechakSound;
    public AudioClip MoneySound;
    public AudioClip WearSound;
    public AudioClip WoodSound;
    public AudioClip RockSound;


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

    public static void PlaySoundOne()
    {
        if (instance != null && instance.audioSource != null && instance.buttonSound != null)
            instance.audioSource.PlayOneShot(instance.buttonSound);
    }

    public static void PlaySoundTwo()
    {
        if (instance != null && instance.audioSource != null && instance.bubbleSound != null)
            instance.audioSource.PlayOneShot(instance.bubbleSound);
    }

    public static void PlaySoundThree()
    {
        if (instance != null && instance.audioSource != null && instance.LogoutSound != null)
            instance.audioSource.PlayOneShot(instance.LogoutSound);
    }

    public static void PlaySoundFour()
    {
        if (instance != null && instance.audioSource != null && instance.KechakSound != null)
            instance.audioSource.PlayOneShot(instance.KechakSound);
    }

    public static void PlaySoundFive()
    {
        if (instance != null && instance.audioSource != null && instance.MoneySound != null)
            instance.audioSource.PlayOneShot(instance.MoneySound);
    }

    public static void PlaySoundSix()
    {
        if (instance != null && instance.audioSource != null && instance.WearSound != null)
            instance.audioSource.PlayOneShot(instance.WearSound);
    }

    public static void PlaySoundSeven()
    {
        if (instance != null && instance.audioSource != null && instance.WoodSound != null)
            instance.audioSource.PlayOneShot(instance.WoodSound);
    }

    public static void PlaySoundEight()
    {
        if (instance != null && instance.audioSource != null && instance.RockSound != null)
            instance.audioSource.PlayOneShot(instance.RockSound);
    }

}