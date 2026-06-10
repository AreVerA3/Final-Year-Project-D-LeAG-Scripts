using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [Header("Login Input Fields")]
    public TMP_InputField loginParentNameInput;
    public TMP_InputField loginPasswordInput;

    [Header("Error Pop-up UI (Optional)")]
    public GameObject failLoginPanel; // A popup panel saying "Wrong Name or Password!"

    [Header("Navigation Targets")]
    public string dashboardSceneName; 


    void Start()
    {
        // Automatically hide the error panel when the scene starts
        if (failLoginPanel != null) failLoginPanel.SetActive(false);
    }

    public void AttemptLogin()
    {
        // 1. Safety Check: Make sure input variables are linked
        if (loginParentNameInput == null || loginPasswordInput == null)
        {
            Debug.LogError("Missing Login Input Field references in the Inspector!");
            return;
        }

        // 2. Grab what the user just typed and clean up accidental extra spaces
        string enteredName = loginParentNameInput.text.Trim();
        string enteredPassword = loginPasswordInput.text; // Don't trim passwords!

        // 3. Retrieve the real registered data saved in PlayerPrefs
        // (Assuming your registration script saves keys like "ParentName" and "ParentPassword")
        string registeredName = PlayerPrefs.GetString("SavedParentName", "").Trim();
        string registeredPassword = PlayerPrefs.GetString("SavedPassword", "");

        // 4. Check if an account even exists yet
        if (string.IsNullOrEmpty(registeredName) || string.IsNullOrEmpty(registeredPassword))
        {
            Debug.LogWarning("No account found in PlayerPrefs! Please register first.");
            if (failLoginPanel != null) failLoginPanel.SetActive(true);
            return;
        }

        Debug.Log("Typed Name: [" + enteredName + "] | Saved Name: [" + registeredName + "]");
        Debug.Log("Typed Pass: [" + enteredPassword + "] | Saved Pass: [" + registeredPassword + "]");

        // 5. THE DATA VALIDATION MATCH CHECK
        if (enteredName == registeredName && enteredPassword == registeredPassword)
        {
            Debug.Log("Login Successful! Credentials match perfectly.");

            // Mark the session as active so your auto-login code works next boot
            PlayerPrefs.SetInt("IsLoggedIn", 1);
            PlayerPrefs.Save();

            // Hide error popup if it was open
            if (failLoginPanel != null) failLoginPanel.SetActive(false);

            // Move the user into the main gameplay hub/map scene
            SceneManager.LoadScene(dashboardSceneName);
        }
        else
        {
            Debug.LogWarning("Login Failed: Parent Name or Password does not match registered data!");
            
            // Pop up your error panel so the user knows they messed up
            if (failLoginPanel != null) failLoginPanel.SetActive(true);
        }
    }

    // Hook this up to a close button on your "Wrong Password" error panel
    public void CloseFailLoginPanel()
    {
        if (failLoginPanel != null) failLoginPanel.SetActive(false);
    }
}