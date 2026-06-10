using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required to talk to the Image component

public class ScreenFader : MonoBehaviour
{
    private Image logoImage;

    void Awake()
    {
        // Grab the Image component on this logo object
        logoImage = GetComponent<Image>();
        
        if (logoImage != null)
        {
            // Start completely invisible (Alpha = 0)
            Color c = logoImage.color;
            c.a = 0f;
            logoImage.color = c;
        }
    }

    void Start()
    {
        // Start the cinematic intro as soon as the scene boots!
        StartCoroutine(CinematicIntroSequence());
    }

    IEnumerator CinematicIntroSequence()
    {
        yield return new WaitForSeconds(0.5f); // Wait a split second in the dark

        float timer = 0f;
        float duration = 2.0f; // Takes 2 seconds to fully fade in

        // 💫 FADE IN: Gradually make the logo visible
        while (timer < duration)
        {
            timer += Time.deltaTime;
            if (logoImage != null)
            {
                Color c = logoImage.color;
                c.a = Mathf.Clamp01(timer / duration);
                logoImage.color = c;
            }
            yield return null;
        }

        Debug.Log("Logo fade-in complete!");
    }

    // Keep this here so your SplashScreen script doesn't break when loading the Main Menu!
    public IEnumerator FadeOut(float duration)
    {
        // Optional: Fade the logo back out when entering the game, or just let it cut
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            if (logoImage != null)
            {
                Color c = logoImage.color;
                c.a = Mathf.Clamp01(1f - (timer / duration));
                logoImage.color = c;
            }
            yield return null;
        }
    }
}