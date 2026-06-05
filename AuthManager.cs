using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class AuthManager : MonoBehaviour
{
    [Header("Registration UI")]
    public TMP_InputField registerUsernameInput;
    public TMP_InputField registerPasswordInput;
    public TMP_InputField childNameInput; 
    public TMP_InputField childAgeInput;  

    [Header("Login UI")]
    public TMP_InputField loginUsernameInput;
    public TMP_InputField loginPasswordInput;

    [Header("Universal Pop-ups")]
    public GameObject successPopUp;
    public GameObject errorPopUp;          // NEW: Just ONE error panel for everything!
    public TextMeshProUGUI errorPopUpText; // NEW: The text box INSIDE that error panel

    [Header("Screen Routing")]
    public GameObject loginPanel;       
    public GameObject q1Panel;          
    public string dashboardSceneName;   

    public void RegisterUser()
    {
        string username = registerUsernameInput.text;
        string password = registerPasswordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            // 1. Tell the pop-up what to say, then turn it on!
            if (errorPopUp != null && errorPopUpText != null)
            {
                errorPopUpText.text = "Username and password cannot be empty!";
                errorPopUp.SetActive(true);
            }
            return;
        }

        // Save data
        PlayerPrefs.SetString("SavedUsername", username);
        PlayerPrefs.SetString("SavedPassword", password);
        PlayerPrefs.SetString("SavedChildName", childNameInput.text);
        PlayerPrefs.SetString("SavedChildAge", childAgeInput.text);
        PlayerPrefs.SetInt("FirstTimeUser", 1); 
        PlayerPrefs.Save(); 

        if (successPopUp != null)
        {
            successPopUp.SetActive(true);
        }

        // Clear fields
        registerUsernameInput.text = "";
        registerPasswordInput.text = "";
        childNameInput.text = "";
        childAgeInput.text = "";
    }

    public void LoginUser()
    {
        string inputUsername = loginUsernameInput.text;
        string inputPassword = loginPasswordInput.text;

        string savedUsername = PlayerPrefs.GetString("SavedUsername", "");
        string savedPassword = PlayerPrefs.GetString("SavedPassword", "");

        if (inputUsername == savedUsername && inputPassword == savedPassword && !string.IsNullOrEmpty(savedUsername))
        {
            int isFirstTime = PlayerPrefs.GetInt("FirstTimeUser", 1); 

            if (isFirstTime == 1)
            {
                PlayerPrefs.SetInt("FirstTimeUser", 0);
                PlayerPrefs.Save();

                if (loginPanel != null) loginPanel.SetActive(false);
                if (q1Panel != null) q1Panel.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene(dashboardSceneName);
            }
        }
        else
        {
            // 2. Tell the SAME pop-up to say something else, then turn it on!
            if (errorPopUp != null && errorPopUpText != null)
            {
                errorPopUpText.text = "Invalid username or password.";
                errorPopUp.SetActive(true);
            }

            loginPasswordInput.text = ""; 
        }
    }

    public void ClosePopUp()
    {
        if (successPopUp != null) successPopUp.SetActive(false);
    }

    // Hook this up to the X button on your shared Error Pop-up
    public void CloseErrorPopUp()
    {
        if (errorPopUp != null) errorPopUp.SetActive(false);
    }
}