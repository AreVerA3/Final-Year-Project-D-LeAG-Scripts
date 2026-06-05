using UnityEngine;
using TMPro;

public class EditProfileManager : MonoBehaviour
{
    [Header("Edit UI Elements")]
    public GameObject editPanel; // The actual pop-up panel
    public TMP_InputField editNameInput;
    public TMP_InputField editAgeInput;

    [Header("Reference")]
    public ProfileDisplay profileDisplayScript; // Links back to our display script

    // 1. Put this on your "Edit Profile" button to OPEN the panel
    public void OpenEditPanel()
    {
        editPanel.SetActive(true);
        
        // Pre-fill the input fields with the currently saved data
        editNameInput.text = PlayerPrefs.GetString("SavedChildName", "");
        editAgeInput.text = PlayerPrefs.GetString("SavedChildAge", "");
    }

    // 2. Put this on your green "Save" button inside the edit panel
    public void SaveEditedData()
    {
        // Save the new typed text
        PlayerPrefs.SetString("SavedChildName", editNameInput.text);
        PlayerPrefs.SetString("SavedChildAge", editAgeInput.text);
        PlayerPrefs.Save();

        // Tell the main dashboard to instantly update the text
        if (profileDisplayScript != null)
        {
            profileDisplayScript.RefreshProfileUI();
        }

        // Close the panel
        editPanel.SetActive(false);
    }
    
    // 3. Put this on your "Cancel/X" button
    public void CloseEditPanel()
    {
        editPanel.SetActive(false);
    }
}