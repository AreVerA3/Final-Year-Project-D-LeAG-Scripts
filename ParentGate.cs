using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Required for switching scenes!

public class ParentGate : MonoBehaviour
{
    [Header("UI Fields")]
    public TMP_InputField answerInput; 
    public TextMeshProUGUI questionText; 
    
    [Header("Gate Navigation")]
    public GameObject parentGatePanel; 
    public GameObject gateFailurePanel; // Drag your "Try Again" error pop-up panel here!
    public string profileSceneName = "ProfileScene"; // NEW: Type your exact Profile scene name here!

    private int correctAnswer; // The secret hidden math answer

    // OnEnable runs every single time this panel is turned on
    void OnEnable()
    {
        GenerateNewQuestion();
        // Make sure the error panel is hidden when starting fresh
        if (gateFailurePanel != null) gateFailurePanel.SetActive(false);
    }

    public void OpenTheGatePanel()
    {
        Debug.Log(">>> MANAGE PROFILE BUTTON WAS SUCCESSFULLY CLICKED! <<<");

        if (parentGatePanel != null)
        {
            parentGatePanel.SetActive(true);
            Debug.Log("ParentGatePanel has been set to ACTIVE.");
        }
        else
        {
            Debug.LogError("ParentGate error: You forgot to drag the ParentGatePanel into the slot in the Inspector!");
        }
    }

    public void GenerateNewQuestion()
    {
        // Pick a random question type: 0 = Add & Multiply, 1 = Multiply & Subtract, 2 = Multiply & Add, 3 = Divide & Add
        int questionType = Random.Range(0, 4); 

        int num1, num2, num3;

        switch (questionType)
        {
            case 0: // (A + B) x C
                num1 = Random.Range(10, 30);
                num2 = Random.Range(10, 30);
                num3 = Random.Range(2, 6);
                correctAnswer = (num1 + num2) * num3;
                questionText.text = $"What is ({num1} + {num2}) x {num3} ?";
                break;

            case 1: // (A x B) - C
                num1 = Random.Range(4, 12);
                num2 = Random.Range(4, 12);
                num3 = Random.Range(1, 15); // Kept small to avoid negative answers
                correctAnswer = (num1 * num2) - num3;
                questionText.text = $"What is ({num1} x {num2}) - {num3} ?";
                break;

            case 2: // (A x B) + C
                num1 = Random.Range(3, 12);
                num2 = Random.Range(3, 12);
                num3 = Random.Range(10, 50);
                correctAnswer = (num1 * num2) + num3;
                questionText.text = $"What is ({num1} x {num2}) + {num3} ?";
                break;

            case 3: // (A / B) + C
                num2 = Random.Range(2, 10); // The number we divide by
                int multiplier = Random.Range(3, 12);
                num1 = num2 * multiplier; // Guarantees the division leaves no remainder!
                num3 = Random.Range(10, 50);
                correctAnswer = (num1 / num2) + num3;
                questionText.text = $"What is ({num1} / {num2}) + {num3} ?";
                break;
        }

        // Clear out input box for the new question
        answerInput.text = ""; 
    }

    public void CheckMathAnswer()
    {
        // Check if their typed answer matches the secret answer
        if (answerInput.text == correctAnswer.ToString()) 
        {
            Debug.Log("Gate Passed! Loading Profile Scene...");
            
            // NEW: Hide the gate panel and load your dedicated profile scene!
            if (parentGatePanel != null) parentGatePanel.SetActive(false); 
            SceneManager.LoadScene(profileSceneName);     
        }
        else
        {
            // Wrong answer! Open the "Try Again" panel overlay!
            if (gateFailurePanel != null)
            {
                gateFailurePanel.SetActive(true);
            }
        }
    }

    // Hook this up to the "Try Again" button inside your error panel!
    public void ClickTryAgain()
    {
        if (gateFailurePanel != null) gateFailurePanel.SetActive(false);
        GenerateNewQuestion();
    }

    // Hook this up to a "Cancel" or "X" button on the main gate panel
    public void CloseGate()
    {
        parentGatePanel.SetActive(false);
    }
}