using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [Header("Reading Progress UI")]
    public Image readingFillBar;
    public TextMeshProUGUI readingLevelText;
    public TextMeshProUGUI readingPercentText;

    [Header("Spelling Progress UI")]
    public Image spellingFillBar;
    public TextMeshProUGUI spellingLevelText;
    public TextMeshProUGUI spellingPercentText;

    void Start()
    {
        UpdateProgressBars();
    }

    public void UpdateProgressBars()
    {
        // Grab the saved data from PlayerPrefs
        int highestReadingLevel = PlayerPrefs.GetInt("HighestSpellingLevel", 1);
        float readingScore = PlayerPrefs.GetInt("ReadingScore", 0); // e.g., 3, 4, or 5

        // Math: Divide score by 5 to get a number between 0.0 and 1.0 for the bar
        float readingFillAmount = readingScore / 5f;
        // Math: Multiply by 100 to get the percentage for the text
        int readingPercent = Mathf.RoundToInt(readingFillAmount * 100f);

        // Update Reading UI
        readingLevelText.text = "Highest Level: " + highestReadingLevel;
        readingPercentText.text = readingPercent + "%";
        readingFillBar.fillAmount = readingFillAmount;


        // --- SPELLING DATA CALCULATION ---
        int highestSpellingLevel = PlayerPrefs.GetInt("HighestSpellingLevel", 1);
        float spellingScore = PlayerPrefs.GetInt("SpellingScore", 0); 

        float spellingFillAmount = spellingScore / 5f;
        int spellingPercent = Mathf.RoundToInt(spellingFillAmount * 100f);

        // Update Spelling UI
        spellingLevelText.text = "Highest Level: " + highestSpellingLevel;
        spellingPercentText.text = spellingPercent + "%";
        spellingFillBar.fillAmount = spellingFillAmount;
    }
}