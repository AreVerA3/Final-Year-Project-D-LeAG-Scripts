using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ReadingGameplayManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI wordToRead; 
    public Button[] imageButtons;       
    public TextMeshProUGUI scoreText; // Matches your Spelling naming structure
    public TextMeshProUGUI coinText;  // Drag the text box from inside your win panel here!
    public GameObject correctPanel, wrongPanel, levelCompletePanel;
    public List<WordData> allLevelBanks; 
    public AudioSource audioSource; 

    private SingleWord currentWord; 
    private List<SingleWord> availableQuestions; 
    private int currentLevelNumber; // Matches Spelling naming structure
    private int score = 0;             
    private int sessionCoinsEarned = 0; // Tracks only the coins earned in this session
    private int totalQuestionsAsked = 0;
    private bool hasMistakeOnCurrentWord = false; 
    private int safeBankIndex; 

    void Start() 
    {
        // 1. Get the level number passed from the button click
        currentLevelNumber = PlayerPrefs.GetInt("SelectedLevel", 1);
        
        // 2. Calculate the array slot index safely
        safeBankIndex = Mathf.Clamp(currentLevelNumber - 1, 0, allLevelBanks.Count - 1);
        
        // 3. Clear the session coins text display at start
        UpdateCoinDisplay();

        availableQuestions = new List<SingleWord>(allLevelBanks[safeBankIndex].words);
        LoadNextReadingQuestion();
    }

    void UpdateCoinDisplay()
    {
        if (coinText != null)
        {
            // Adds the requested '+' sign dynamically before the session number!
            coinText.text = "+" + sessionCoinsEarned.ToString();
        }
    }

    public void LoadNextReadingQuestion() 
    {
        if (scoreText) scoreText.text = score + "/5";

        if (totalQuestionsAsked >= 5 || availableQuestions.Count == 0) 
        {
            ShowLevelComplete();
            return;
        }

        hasMistakeOnCurrentWord = false; 

        int r = Random.Range(0, availableQuestions.Count);
        currentWord = availableQuestions[r];
        availableQuestions.RemoveAt(r); 

        wordToRead.text = currentWord.displayWord.Replace("_", currentWord.correctLetter).ToUpper();

        SetupImageChoices();
    }

    void SetupImageChoices() 
    {
        List<SingleWord> bank = new List<SingleWord>(allLevelBanks[safeBankIndex].words);
        List<SingleWord> choices = new List<SingleWord> { currentWord };
        bank.Remove(currentWord);

        while (choices.Count < 3 && bank.Count > 0) 
        {
            int rand = Random.Range(0, bank.Count);
            if (!choices.Contains(bank[rand]))
            {
                choices.Add(bank[rand]);
            }
            bank.RemoveAt(rand);
        }

        // Prevent duplicate images by pulling backup words if current level is tiny
        if (choices.Count < 3)
        {
            List<SingleWord> absoluteAllWords = new List<SingleWord>();
            foreach (var levelBank in allLevelBanks)
            {
                if (levelBank != null && levelBank.words != null)
                {
                    absoluteAllWords.AddRange(levelBank.words);
                }
            }

            int safetyBreak = 0; 
            while (choices.Count < 3 && absoluteAllWords.Count > 0 && safetyBreak < 100)
            {
                safetyBreak++;
                int rand = Random.Range(0, absoluteAllWords.Count);
                SingleWord backupWord = absoluteAllWords[rand];

                if (!choices.Contains(backupWord))
                {
                    choices.Add(backupWord);
                }
                absoluteAllWords.RemoveAt(rand);
            }
        }

        for (int i = 0; i < choices.Count; i++) 
        {
            SingleWord temp = choices[i];
            int rand = Random.Range(i, choices.Count);
            choices[i] = choices[rand];
            choices[rand] = temp;
        }

        for (int i = 0; i < imageButtons.Length; i++) 
        {
            if (i < choices.Count) 
            {
                imageButtons[i].gameObject.SetActive(true);
                imageButtons[i].GetComponent<Image>().sprite = choices[i].wordPicture;
                
                SingleWord choice = choices[i];
                imageButtons[i].onClick.RemoveAllListeners();
                imageButtons[i].onClick.AddListener(() => CheckAnswer(choice));
            } 
            else 
            {
                imageButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void CheckAnswer(SingleWord selected) 
    { 
        if (selected == currentWord) 
        { 
            totalQuestionsAsked++; 

            if (!hasMistakeOnCurrentWord)
            {
                score++;
                sessionCoinsEarned += 5;

                int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);
                PlayerPrefs.SetInt("TotalCoins", currentCoins + 5);
                
                // NEW: Secretly track a flawless correct reading!
                PlayerPrefs.SetInt("TotalReadingCorrect", PlayerPrefs.GetInt("TotalReadingCorrect", 0) + 1);
                
                PlayerPrefs.Save();
                UpdateCoinDisplay();
            }

            if (scoreText) scoreText.text = score + "/5";

            correctPanel.SetActive(true); 
        } 
        else 
        { 
            // NEW: Secretly track a reading mistake!
            PlayerPrefs.SetInt("TotalReadingMistakes", PlayerPrefs.GetInt("TotalReadingMistakes", 0) + 1);
            PlayerPrefs.Save();
            
            hasMistakeOnCurrentWord = true; 
            wrongPanel.SetActive(true); 
        }
    }

    void ShowLevelComplete()
    {
        int highestLevelUnlocked = PlayerPrefs.GetInt("ReadingLevelReached", 1);
        if (currentLevelNumber >= highestLevelUnlocked) 
        {
            PlayerPrefs.SetInt("ReadingLevelReached", currentLevelNumber + 1);
            PlayerPrefs.Save();
        }
        levelCompletePanel.SetActive(true);
    }

    public void PlayWordAudio() 
    {
        if (currentWord != null && currentWord.wordAudio != null) 
        {
            audioSource.PlayOneShot(currentWord.wordAudio);
        } 
        else 
        {
            Debug.LogWarning("No audio clip assigned to " + currentWord.fullWord);
        }
    }

    public void ClickContinue() { correctPanel.SetActive(false); LoadNextReadingQuestion(); }
    public void ClickTryAgain() { wrongPanel.SetActive(false); }
    public void ClickFinish() { SceneManager.LoadScene("Reading"); } 
}