using UnityEngine;
using UnityEngine.UI;

public class SpellingMapManager : MonoBehaviour
{
    public RectTransform slimeIcon;
    public Button[] spellingButtons; // Assign your 20 Spelling bubble buttons here
    
    public GameObject panelLevel1; 
    public GameObject panelLevel2; 

    void Start()
    {
        // 1. Tell the system we are explicitly tracking spelling progress
        int levelReached = PlayerPrefs.GetInt("SpellingLevelReached", 1);

        // 2. Handle Panel Swapping (Level 11+ logic)
        if (levelReached >= 11 && panelLevel2 != null)
        {
            panelLevel1.SetActive(false);
            panelLevel2.SetActive(true);
        }
        else
        {
            panelLevel1.SetActive(true);
            if (panelLevel2 != null) panelLevel2.SetActive(false);
        }

        // 3. Automatically program each of your 20 spelling buttons
        for (int i = 0; i < spellingButtons.Length; i++)
        {
            if (spellingButtons[i] != null)
            {
                // Lock or unlock based on progress
                spellingButtons[i].interactable = (i + 1 <= levelReached);

                // Find the MapLevelButton script sitting on the button prefab
                MapLevelButton levelButtonScript = spellingButtons[i].GetComponent<MapLevelButton>();
                
                if (levelButtonScript != null)
                {
                    levelButtonScript.levelNumber = (i + 1);
                    levelButtonScript.gameplaySceneName = "Spelling Gameplay"; // Matches your spelling room name!

                    // Setup click listener clean
                    spellingButtons[i].onClick.RemoveAllListeners();
                    spellingButtons[i].onClick.AddListener(() => levelButtonScript.LoadThisLevel());
                }
            }
        }

        // 4. Position the little slime avatar on the map path
        int currentSlot = levelReached - 1;
        if (currentSlot >= 0 && currentSlot < spellingButtons.Length && spellingButtons[currentSlot] != null)
        {
            slimeIcon.position = spellingButtons[currentSlot].transform.position;
        }
    }
}