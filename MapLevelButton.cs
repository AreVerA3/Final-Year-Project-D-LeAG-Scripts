using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLevelButton : MonoBehaviour
{
    [Header("Which level is this button for?")]
    public int levelNumber;
    
    [Header("Exact name of your game scene")]
    public string gameplaySceneName = "Reading Gameplay"; // Default changed for convenience on Reading assets

    public void LoadThisLevel()
    {
        // 1. Leave the sticky note for the game scene to read
        PlayerPrefs.SetInt("SelectedLevel", levelNumber);
        
        // 2. BACKUP LINE: Double check module tagging based on the target gameplay room name!
        if (gameplaySceneName.Contains("Reading"))
        {
            PlayerPrefs.SetString("LastModulePlayed", "Reading");
        }
        else if (gameplaySceneName.Contains("Spelling"))
        {
            PlayerPrefs.SetString("LastModulePlayed", "Spelling");
        }
        
        PlayerPrefs.Save();
        
        // 3. Load the actual gameplay room
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnClickSpellingModule() 
    {
        // 1. Tell the game: "We are doing Spelling now!"
        PlayerPrefs.SetString("LastModulePlayed", "Spelling");
        PlayerPrefs.Save();
        
        // 2. Load your Spelling Map scene
        SceneManager.LoadScene("Spelling"); 
    }

    public void OnClickReadingModule() 
    {
        // 1. Tell the game: "We are doing Reading now!"
        PlayerPrefs.SetString("LastModulePlayed", "Reading");
        PlayerPrefs.Save();
        
        // 2. Load your Reading Map scene
        SceneManager.LoadScene("Reading"); 
    }

    // Kept for backward compatibility if old buttons reference it
    public void ClickLevelButton(int levelNum)
    {
        PlayerPrefs.SetInt("SelectedLevel", levelNum);
        PlayerPrefs.SetString("LastModulePlayed", "Spelling");
        PlayerPrefs.Save();
        SceneManager.LoadScene("Spelling Gameplay");
    }
}