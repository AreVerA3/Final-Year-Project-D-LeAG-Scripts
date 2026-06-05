using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class PetNamingManager : MonoBehaviour
{
    [Header("UI Elements")]
    public InputField nameInputField; 
    
    [Header("Navigation")]
    public string nextSceneName; 

    public void SavePetName()
    {
        // 1. Grab the text the player typed
        string chosenName = nameInputField.text;

        // 2. Safety check: If they haven't typed anything, stop everything!
        if (string.IsNullOrWhiteSpace(chosenName))
        {
            Debug.LogWarning("No name entered! Waiting for the player to type something...");
            return; // This immediately cancels the save and scene load!
        }

        // 3. Save it to memory
        PlayerPrefs.SetString("SlimeName", chosenName);
        PlayerPrefs.Save();

        Debug.Log("Pet name successfully saved as: " + chosenName);

        // 4. Move to the next scene 
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName); 
        }
    }

    public void CancelNaming()
    {
        nameInputField.text = "";
        Debug.Log("Naming cancelled. Box cleared.");
    }
}