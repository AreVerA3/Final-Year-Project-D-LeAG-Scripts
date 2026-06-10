using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 
using TMPro; // Added for TextMeshPro if your main UI text uses it

public class PetNamingManager : MonoBehaviour
{
    [Header("UI Elements")]
    public InputField nameInputField; 
    public TextMeshProUGUI slimeNameTextMain; // Drag your wardrobe screen's main name text here
    public GameObject editPanel;              // Drag your new Edit Panel here
    
    [Header("Navigation (Leave empty for Wardrobe scene)")]
    public string nextSceneName; 

    public void SavePetName()
    {
        // 1. Grab the text the player typed
        string chosenName = nameInputField.text.Trim();

        // 2. Safety check: If they haven't typed anything, stop everything!
        if (string.IsNullOrWhiteSpace(chosenName))
        {
            Debug.LogWarning("No name entered! Waiting for the player to type something...");
            return; 
        }

        // 3. Save it to memory using your exact key
        PlayerPrefs.SetString("SlimeName", chosenName);
        PlayerPrefs.Save();

        Debug.Log("Pet name successfully saved as: " + chosenName);

        // 4. Update the main screen text instantly if it's assigned
        if (slimeNameTextMain != null)
        {
            slimeNameTextMain.text = chosenName;
        }

        // 5. If we are editing inside a panel, close it automatically
        if (editPanel != null)
        {
            editPanel.SetActive(false);
        }

        // 6. Move to the next scene (Only runs if a scene name is actually typed in the Inspector)
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName); 
        }
    }

    public void CancelNaming()
    {
        nameInputField.text = "";
        
        // If editing in wardrobe, close the panel when they cancel
        if (editPanel != null)
        {
            editPanel.SetActive(false);
        }
        
        Debug.Log("Naming cancelled. Box cleared.");
    }
}