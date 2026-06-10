using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class ProfileDisplay : MonoBehaviour
{
    [Header("Profile Text Displays")]
    public TextMeshProUGUI[] childNameDisplayTexts; 
    public TextMeshProUGUI childAgeDisplayText;

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
        RefreshProfileUI(); 
    }

    void OnEnable()
    {
        Debug.Log("[PROFILE SYSTEM], Panel popped open! Refreshing level and percentage data...");
        RefreshProfileUI();
    }

    public void ManualTriggerRefresh()
    {
        RefreshProfileUI();
    }

   public void RefreshProfileUI()
    {
        // 1. Retrieve child strings from memory
        string retrievedName = PlayerPrefs.GetString("SavedChildName", "Unknown");
        string retrievedAge = PlayerPrefs.GetString("SavedChildAge", "?");

        foreach (TextMeshProUGUI nameBox in childNameDisplayTexts)
        {
            if (nameBox != null) nameBox.text = retrievedName;
        }

        if (childAgeDisplayText != null) childAgeDisplayText.text = retrievedAge;

        // 2. HIGHEST LEVELS (Still displays correctly on the text!)
        int spellingLevel = PlayerPrefs.GetInt("SpellingLevelReached", 1);
        int readingLevel = PlayerPrefs.GetInt("ReadingLevelReached", 1);

        if (SpellingLevelText != null) SpellingLevelText.text = "Highest Level: " + spellingLevel;
        if (ReadingLevelText != null) ReadingLevelText.text = "Highest Level: " + readingLevel;

        // 3. THE NEW ACCURACY MATH!
        int spellingCorrect = PlayerPrefs.GetInt("TotalSpellingCorrect", 0);
        int spellingMistakes = PlayerPrefs.GetInt("TotalSpellingMistakes", 0);
        int totalSpellingAttempts = spellingCorrect + spellingMistakes;
        float spellingAccuracy = 0f; // Defaults to 0% if they haven't played yet

        if (totalSpellingAttempts > 0)
        {
            spellingAccuracy = (float)spellingCorrect / totalSpellingAttempts; 
        }

        int readingCorrect = PlayerPrefs.GetInt("TotalReadingCorrect", 0);
        int readingMistakes = PlayerPrefs.GetInt("TotalReadingMistakes", 0);
        int totalReadingAttempts = readingCorrect + readingMistakes;
        float readingAccuracy = 0f; 

        if (totalReadingAttempts > 0)
        {
            readingAccuracy = (float)readingCorrect / totalReadingAttempts;
        }

        // 4. Update the Fill Sprites dynamically
        if (SpellingProgressBarImage != null) SpellingProgressBarImage.fillAmount = spellingAccuracy;
        if (ReadingProgressBarImage != null) ReadingProgressBarImage.fillAmount = readingAccuracy;

        // 5. Turn accuracy decimals into clean percentages
        if (SpellingPercentageText != null) 
        {
            SpellingPercentageText.text = Mathf.RoundToInt(spellingAccuracy * 100f) + "%";
        }
        if (ReadingPercentageText != null) 
        {
            ReadingPercentageText.text = Mathf.RoundToInt(readingAccuracy * 100f) + "%";
        }
    }

    public void ClickBackButton()
    {
        SceneManager.LoadScene(sceneToLoadOnBack);
    }
}