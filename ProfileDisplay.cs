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

    [Header("Highest Level Text Displays")] // <-- NEW: Added slots for the level counters!
    public TextMeshProUGUI SpellingHighestLevelText; // Drag your Spelling "Highest Level: X" text here
    public TextMeshProUGUI ReadingHighestLevelText;  // Drag your Reading "Highest Level: X" text here

    [Header("Configuration")]
    public int totalLevelsInGame = 20; // Matches your 20 levels map layout

    void Start()
    {
        childNameText.text = PlayerPrefs.GetString("ChildName", "Unknown");
        childAgeText.text = "Age: " + PlayerPrefs.GetString("ChildAge", "?");
        RefreshProfileUI(); 
    }

    void OnEnable()
    {
        Debug.Log("[PROFILE SYSTEM], Panel popped open! Refreshing level and percentage data...");
        RefreshProfileUI();
    }

    // This allows buttons to manually force a UI update
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

        // (Name and age text layout loop remains here...)

        // 2. THE MEMORY LINK: Tapping directly into what the map managers save!
        int spellingLevelReached = PlayerPrefs.GetInt("SpellingLevelReached", 1);
        int readingLevelReached = PlayerPrefs.GetInt("ReadingLevelReached", 1);

        // 3. Calculate completed levels based on that memory
        // If they are on Level 4, they have fully completed 3 levels!
        int spellingCompleted = spellingLevelReached - 1;
        int readingCompleted = readingLevelReached - 1;

        // Clamp values safely between 0 and 20
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

        // 6. Link it directly to the text displays!
        if (SpellingHighestLevelText != null)
        {
            SpellingHighestLevelText.text = "Highest Level: " + spellingLevelReached;
        }
        if (ReadingHighestLevelText != null)
        {
            ReadingHighestLevelText.text = "Highest Level: " + readingLevelReached;
        }
    }

    public void ClickBackButton()
    {
        SceneManager.LoadScene(sceneToLoadOnBack);
    }
}