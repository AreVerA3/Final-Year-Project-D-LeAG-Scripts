using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class ProfileDisplay : MonoBehaviour
{
    [Header("Profile Text Displays")]
    public TextMeshProUGUI[] childNameDisplayTexts; 
    public TextMeshProUGUI childAgeDisplayText;
    public TextMeshProUGUI childNameText;
    public TextMeshProUGUI childAgeText;

    [Header("Navigation")]
    public string sceneToLoadOnBack = "MapScene"; 

    [Header("Progress Bar Filled Sprites")] 
    public Image SpellingProgressBarImage;      
    public Image ReadingProgressBarImage;       

    [Header("Progress Percentage Displays")] 
    public TextMeshProUGUI SpellingPercentageText; 
    public TextMeshProUGUI ReadingPercentageText;  

    [Header("Highest Level Text Displays")] 
    public TextMeshProUGUI SpellingLevelText; 
    public TextMeshProUGUI ReadingLevelText;  

    [Header("Configuration")]
    public int totalLevelsInGame = 20; 

    void Start()
    {
        childNameText.text = PlayerPrefs.GetString("SavedChildName", "Unknown");
        childAgeText.text = "Age: " + PlayerPrefs.GetString("SavedChildAge", "?");
        RefreshProfileUI(); 
    }

    void OnEnable()
    {
        Debug.Log("[PROFILE SYSTEM], Panel popped open! Refreshing level and percentage data...");
        RefreshProfileUI();
    }

    public void ManualTriggerRefresh()
    {
        Debug.Log("[PROFILE SYSTEM] Manual Trigger Refresh called via button click!");
        RefreshProfileUI();
    }

    public void RefreshProfileUI()
    {
        // 1. Retrieve child strings from memory
        string retrievedName = PlayerPrefs.GetString("SavedChildName", "No Name Saved");
        string retrievedAge = PlayerPrefs.GetString("SavedChildAge", "0");

        // 2. THE MEMORY LINK: Tapping directly into what the map managers save!
        // Matching your screenshot exactly!
        int spellingLevel = PlayerPrefs.GetInt("SpellingLevelReached", 1);
        int readingLevel = PlayerPrefs.GetInt("ReadingLevelReached", 1);

        // 3. Calculate completed levels based on that memory
        int spellingCompleted = spellingLevel - 1;
        int readingCompleted = readingLevel - 1;

        // Clamp values safely between 0 and totalLevelsInGame
        spellingCompleted = Mathf.Clamp(spellingCompleted, 0, totalLevelsInGame);
        readingCompleted = Mathf.Clamp(readingCompleted, 0, totalLevelsInGame);

        // 4. Update the Fill Sprites dynamically
        if (SpellingProgressBarImage != null)
        {
            SpellingProgressBarImage.fillAmount = (float)spellingCompleted / totalLevelsInGame;
        }
        if (ReadingProgressBarImage != null)
        {
            ReadingProgressBarImage.fillAmount = (float)readingCompleted / totalLevelsInGame;
        }

        // 5. Turn completed levels into clean percentages
        float spellingPercent = ((float)spellingCompleted / totalLevelsInGame) * 100f;
        float readingPercent = ((float)readingCompleted / totalLevelsInGame) * 100f;

        if (SpellingPercentageText != null) 
        {
            SpellingPercentageText.text = Mathf.RoundToInt(spellingPercent) + "%";
        }
        if (ReadingPercentageText != null) 
        {
            ReadingPercentageText.text = Mathf.RoundToInt(readingPercent) + "%";
        }

        // 6. Link it directly to the text displays! (TYPO FIXED HERE)
        if (SpellingLevelText != null)
        {
            SpellingLevelText.text = "Highest Level: " + spellingLevel;
        }
        if (ReadingLevelText != null)
        {
            ReadingLevelText.text = "Highest Level: " + readingLevel;
        }

        Debug.Log($"[PROFILE TEST] Unity thinks the spelling level is: {spellingLevel} and reading level is: {readingLevel}");
    }

    public void ClickBackButton()
    {
        SceneManager.LoadScene(sceneToLoadOnBack);
    }
}