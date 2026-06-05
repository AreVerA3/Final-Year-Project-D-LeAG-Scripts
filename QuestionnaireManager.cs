using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestionnaireManager : MonoBehaviour
{
    [Header("Question Panels (Drag Q1 to Q9 here in order)")]
    public GameObject[] questionPanels; 

    [Header("Routing")]
    public string dashboardSceneName; // Type your main scene name here

    private int totalScore = 0;
    private int currentQuestionIndex = 0;

    // We will call this single function from EVERY button, 
    // and pass the specific point value (3, 1, or 0) from the Inspector!
    public void AnswerQuestion(int points)
    {
        // 1. Add the points to the running total
        totalScore += points;
        Debug.Log("Points added: " + points + " | Total Score is now: " + totalScore);

        // 2. Hide the current question panel
        questionPanels[currentQuestionIndex].SetActive(false);

        // 3. Move to the next index
        currentQuestionIndex++;

        // 4. Check if there are more questions left
        if (currentQuestionIndex < questionPanels.Length)
        {
            // Turn on the next question
            questionPanels[currentQuestionIndex].SetActive(true);
        }
        else
        {
            // We ran out of questions! Calculate the final result.
            FinishQuestionnaire();
        }
    }

    private void FinishQuestionnaire()
    {
        string recommendedDifficulty = "Beginner"; // Default starting point

        // Calculate difficulty based on the max possible score (9 questions * 3 pts = 27 max)
        // You can change these numbers later once you decide the exact scoring tiers!
        if (totalScore >= 20)
        {
            recommendedDifficulty = "Advanced";
        }
        else if (totalScore >= 10)
        {
            recommendedDifficulty = "Intermediate";
        }

        Debug.Log("Final Questionnaire Score: " + totalScore + " | Assigned Difficulty: " + recommendedDifficulty);

        // Save the difficulty level in the memory bank so the game knows how hard to make the levels
        PlayerPrefs.SetString("ChildDifficulty", recommendedDifficulty);
        PlayerPrefs.Save();

        // Jump straight to the main dashboard!
        SceneManager.LoadScene(dashboardSceneName);
    }
}