using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutManager : MonoBehaviour
{
    [Header("UI Confirmation Panel (Optional)")]
    public GameObject logoutConfirmationPanel; // Drag a "Are you sure?" popup here if you have one

    [Header("Navigation Target")]
    public string authSceneName = "AuthScene"; // Name of your login/registration scene

    void Start()
    {
        // Safety: Ensure the confirmation panel is hidden when entering the scene
        if (logoutConfirmationPanel != null)
        {
            logoutConfirmationPanel.SetActive(false);
        }
    }
    public void ClickLogoutButton()
    {
        if (logoutConfirmationPanel != null)
        {
            logoutConfirmationPanel.SetActive(true);
        }
        else
        {
            ConfirmAndExecuteLogout();
        }
    }
    public void CancelLogout()
    {
        if (logoutConfirmationPanel != null)
        {
            logoutConfirmationPanel.SetActive(false);
        }
    }
    public void ConfirmAndExecuteLogout()
    {
        Debug.Log("Processing user logout session routine...");

        // 1. Clear session status tracking keys so they don't auto-login next startup
        if (PlayerPrefs.HasKey("IsLoggedIn"))
        {
            PlayerPrefs.SetInt("IsLoggedIn", 0); 
        }
        
        // Explicitly write the changes to local storage memory
        PlayerPrefs.Save();

        // 2. Shut off the confirmation view if it's currently open
        if (logoutConfirmationPanel != null)
        {
            logoutConfirmationPanel.SetActive(false);
        }

        // 3. Kick user back out to the main Authentication screen layout cleanly
        if (!string.IsNullOrEmpty(authSceneName))
        {
            Debug.Log($"Redirecting session to interface view: {authSceneName}");
            SceneManager.LoadScene(authSceneName);
        }
        else
        {
            Debug.LogError("Logout redirect aborted: The targeted Authentication scene name is blank!");
        }
    }
}