using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SpellingManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image wordImage;
    public TextMeshProUGUI wordTextDisplay;
    public Button[] letterButtons;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText; 
    public GameObject correctPanel, wrongPanel, levelCompletePanel;
    public List<WordData> allLevelBanks;
    public AudioSource audioSource; 

    private SingleWord currentWord; 
    private List<SingleWord> availableWords; 
    private int currentLevelNumber;
    private int score = 0;
    private int sessionCoinsEarned = 0;
    private int totalQuestionsAsked = 0;
    private List<char> currentChoices = new List<char>();
    private bool hasMistakeOnCurrentWord = false; 

    void Start()
{
    // Get the level number passed from the button click
    currentLevelNumber = PlayerPrefs.GetInt("SelectedLevel", 1);
    
    // Calculate the array slot index safely
    int bankIndex = Mathf.Clamp(currentLevelNumber - 1, 0, allLevelBanks.Count - 1);

    availableWords = new List<SingleWord>(allLevelBanks[bankIndex].words);
    LoadNextQuestion();
}

    void UpdateCoinDisplay()
    {
        if (coinText != null)
        {
            coinText.text = "+" + sessionCoinsEarned.ToString();
        }
    }

    void AddCoins(int amount)
    {
        // 1. Grab the central pool balance
        int currentTotalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
    
        // 2. Add the 5 coins
        currentTotalCoins += amount;

        // 3. Save it to the disk
        PlayerPrefs.SetInt("TotalCoins", currentTotalCoins);
        PlayerPrefs.Save();

        // 4. Update spelling coin UI text if you have one set up
        // if (coinText != null) coinText.text = currentTotalCoins.ToString();
        
        Debug.Log($"Spelling Correct! Earned {amount} coins. Total balance: {currentTotalCoins}");
    }

    public void LoadNextQuestion()
    {
        // Update the display text to show how many total questions they've completed
        if (scoreText) scoreText.text = score + "/5";

        // FIX: End the game when they have been asked 5 questions total, NOT when score hits 5!
        if (totalQuestionsAsked >= 5 || availableWords.Count == 0)
        {
            ShowLevelComplete();
            return;
        }

        hasMistakeOnCurrentWord = false; 

        int randomIndex = Random.Range(0, availableWords.Count);
        currentWord = availableWords[randomIndex];
        availableWords.RemoveAt(randomIndex);

        wordImage.sprite = currentWord.wordPicture;
        wordTextDisplay.text = currentWord.displayWord.ToUpper();

        SetupLetterButtons();
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

    void SetupLetterButtons()
    {
        char correctLetter = currentWord.correctLetter.ToUpper()[0];
        
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        char wrong1 = alphabet[Random.Range(0, alphabet.Length)];
        while (wrong1 == correctLetter) wrong1 = alphabet[Random.Range(0, alphabet.Length)];

        char wrong2 = alphabet[Random.Range(0, alphabet.Length)];
        while (wrong2 == correctLetter || wrong2 == wrong1) wrong2 = alphabet[Random.Range(0, alphabet.Length)];

        currentChoices = new List<char> { correctLetter, wrong1, wrong2 };

        // Shuffle
        for (int i = 0; i < currentChoices.Count; i++)
        {
            char temp = currentChoices[i];
            int rand = Random.Range(i, currentChoices.Count);
            currentChoices[i] = currentChoices[rand];
            currentChoices[rand] = temp;
        }

        // Apply to UI
        for (int i = 0; i < letterButtons.Length; i++)
        {
            if (i < currentChoices.Count)
            {
                letterButtons[i].gameObject.SetActive(true);
                letterButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentChoices[i].ToString();
                letterButtons[i].onClick.RemoveAllListeners();
                int buttonIndex = i;
                letterButtons[i].onClick.AddListener(() => OnChoiceButtonClicked(buttonIndex));
            }
            else
            {
                letterButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // New intermediate method to safely process the click isolated from loops
    void OnChoiceButtonClicked(int index)
    {
        char chosen = currentChoices[index];
        char correct = currentWord.correctLetter.ToUpper()[0];
        CheckAnswer(chosen, correct);
    }

    void CheckAnswer(char chosen, char correct)
    {
        if (chosen == correct)
        {
            totalQuestionsAsked++;

            if (!hasMistakeOnCurrentWord)
            {
                score++;
                sessionCoinsEarned += 5; 
                int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);
                PlayerPrefs.SetInt("TotalCoins", currentCoins + 5);
                
                // NEW: Secretly track a flawless correct answer!
                PlayerPrefs.SetInt("TotalSpellingCorrect", PlayerPrefs.GetInt("TotalSpellingCorrect", 0) + 1);
                
                PlayerPrefs.Save();
                UpdateCoinDisplay();
            }

            if (scoreText) scoreText.text = score + "/5";
            wordTextDisplay.text = currentWord.displayWord.Replace("_", correct.ToString()).ToUpper();
            correctPanel.SetActive(true);
        }
        else
        {
            // NEW: Secretly track a mistake!
            PlayerPrefs.SetInt("TotalSpellingMistakes", PlayerPrefs.GetInt("TotalSpellingMistakes", 0) + 1);
            PlayerPrefs.Save();
            
            hasMistakeOnCurrentWord = true; 
            wrongPanel.SetActive(true);
        }
    }

    void ShowLevelComplete()
    {
        int highestLevelUnlocked = PlayerPrefs.GetInt("SpellingLevelReached", 1);
        if (currentLevelNumber >= highestLevelUnlocked)
        {
            PlayerPrefs.SetInt("SpellingLevelReached", currentLevelNumber + 1);
            PlayerPrefs.Save();
        }
        levelCompletePanel.SetActive(true);
    }

    public void ClickContinue() 
    { 
        correctPanel.SetActive(false); 
        LoadNextQuestion(); 
    }

    public void ClickTryAgain() 
    { 
        wrongPanel.SetActive(false); 
    }
    
    public void ClickFinish() 
    { 
        SceneManager.LoadScene("Spelling");
    }
}