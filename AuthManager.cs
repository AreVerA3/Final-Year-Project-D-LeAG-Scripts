using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class AuthManager : MonoBehaviour
{
    [Header("Registration UI")]
    public TMP_InputField registerParentNameInput; 
    public TMP_InputField registerPasswordInput;
    public TMP_InputField childNameInput; 
    public TMP_InputField childAgeInput;  

    [Header("Login UI")]
    public TMP_InputField loginParentNameInput;    
    public TMP_InputField loginPasswordInput;

    [Header("Universal Pop-ups")]
    public GameObject successPopUp;
    public GameObject errorPopUp;          
    public TextMeshProUGUI errorPopUpText; 

    [Header("Screen Routing Panels")]
    public GameObject loginPanel;       
    public GameObject q1Panel;          
    public string dashboardSceneName;   

    public void ShowRegisterPanel()
    {
        if (loginPanel != null && q1Panel != null)
        {
            loginPanel.SetActive(false); 
            q1Panel.SetActive(true);     
        }
    }

    public void ShowLoginPanel()
    {
        if (loginPanel != null && q1Panel != null)
        {
            q1Panel.SetActive(false);    
            loginPanel.SetActive(true);  
        }
    }

    public void RegisterUser()
    {
        if (registerParentNameInput == null) return;

        string username = registerParentNameInput.text.Trim();
        string password = registerPasswordInput.text.Trim();
        string childName = childNameInput.text.Trim();
        string childAge = childAgeInput.text.Trim();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            if (errorPopUp != null && errorPopUpText != null)
            {
                errorPopUpText.text = "Parent's name and password cannot be empty!";
                errorPopUp.SetActive(true);
            }
            return;
        }

        if (PlayerPrefs.HasKey("Password_" + username))
        {
            if (errorPopUp != null && errorPopUpText != null)
            {
                errorPopUpText.text = "This Parent Name is already registered! Please log in instead.";
                errorPopUp.SetActive(true);
            }
            return;
        }

        PlayerPrefs.SetString("Password_" + username, password);
        PlayerPrefs.SetString("ChildName_" + username, childName);
        PlayerPrefs.SetString("ChildAge_" + username, childAge);
        
        PlayerPrefs.SetInt(username + "_TotalCoins", 50); 
        PlayerPrefs.SetString(username + "_SlimeName", string.IsNullOrWhiteSpace(childName) ? "Rimuru" : childName);
        PlayerPrefs.SetInt(username + "_SpellingLevelReached", 1);
        PlayerPrefs.SetInt(username + "_ReadingLevelReached", 1);

        PlayerPrefs.SetString("CurrentActiveUser", username);
        PlayerPrefs.SetInt("IsLoggedIn", 1); 
        PlayerPrefs.Save(); 

        if (successPopUp != null) successPopUp.SetActive(true);
        
        registerParentNameInput.text = "";
        registerPasswordInput.text = "";
        childNameInput.text = "";
        childAgeInput.text = "";
    }

    public void LoginUser()
    {
        string inputParentName = loginParentNameInput.text.Trim();
        string inputPassword = loginPasswordInput.text.Trim();
        string savedPassword = PlayerPrefs.GetString("Password_" + inputParentName, "");

        if (inputPassword == savedPassword && !string.IsNullOrEmpty(savedPassword))
        {
            PlayerPrefs.SetString("CurrentActiveUser", inputParentName);
            PlayerPrefs.SetInt("IsLoggedIn", 1); 
            PlayerPrefs.Save();

            SceneManager.LoadScene(dashboardSceneName);
        }
        else
        {
            if (errorPopUp != null && errorPopUpText != null)
            {
                errorPopUpText.text = "Invalid Parent's Name or password.";
                errorPopUp.SetActive(true);
            }
            loginPasswordInput.text = ""; 
        }
    }

    public void ClosePopUp()
    {
        if (successPopUp != null) successPopUp.SetActive(false);
    }

    public void CloseErrorPopUp()
    {
        if (errorPopUp != null) errorPopUp.SetActive(false);
    }

    public void FinishQ1AndEnterGame()
    {
        SceneManager.LoadScene(dashboardSceneName);
    }
}