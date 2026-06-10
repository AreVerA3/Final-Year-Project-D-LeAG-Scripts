using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public ScreenFader fader;
    
    [Header("Scene Configuration")]
    public string mainDashboardSceneName = "Main Dashboard"; 
    public string registerSceneName = "Register"; 

    void Start()
    {
        // 🚀 Automatically kick off the game intro loop on startup!
        StartCoroutine(AutomatedIntroSequence());
    }

    IEnumerator AutomatedIntroSequence()
    {
        // 1. Wait for your Logo's ScreenFader to finish fading in (Logo takes 2s to appear)
        // We add a tiny buffer so the logo stays fully visible on screen for a moment
        yield return new WaitForSeconds(2.5f); 

        // 2. Smoothly fade the logo back out before loading the next scene
        if (fader != null)
        {
            yield return StartCoroutine(fader.FadeOut(1.0f));
        }

        // 3. 🔍 Check local session data
        int loginStatus = PlayerPrefs.GetInt("IsLoggedIn", 0);

        if (loginStatus == 1)
        {
            Debug.Log("Automated Router: Active session found. Loading Dashboard.");
            SceneManager.LoadScene(mainDashboardSceneName);
        }
        else
        {
            Debug.Log("Automated Router: No session found. Loading Registration.");
            SceneManager.LoadScene(registerSceneName);
        }
    }
}