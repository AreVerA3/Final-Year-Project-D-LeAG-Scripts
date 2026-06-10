using UnityEngine;
using UnityEngine.UI;

public class ReadingMapManager : MonoBehaviour
{
    public RectTransform slimeIcon;
    public Button[] readingButtons; // Assign your 20 Reading bubble buttons here
    
    public GameObject panelLevel1; 
    public GameObject panelLevel2; 

    void Start()
    {
        // 1. Tell the system we are explicitly tracking reading progress
        int readingLevel = PlayerPrefs.GetInt("ReadingLevelReached", 1);

        // 2. Handle Panel Swapping (Level 11+ logic)
        if (readingLevel >= 11 && panelLevel2 != null)
        {
            panelLevel1.SetActive(false);
            panelLevel2.SetActive(true);
        }
        else
        {
            panelLevel1.SetActive(true);
            if (panelLevel2 != null) panelLevel2.SetActive(false);
        }

        // 3. Automatically program each of your 20 reading buttons
        for (int i = 0; i < readingButtons.Length; i++)
        {
            if (readingButtons[i] != null)
            {
                // Lock or unlock based on progress
                readingButtons[i].interactable = (i + 1 <= readingLevel);

                // Find the MapLevelButton script sitting on the button prefab
                MapLevelButton levelButtonScript = readingButtons[i].GetComponent<MapLevelButton>();
                
                if (levelButtonScript != null)
                {
                    levelButtonScript.levelNumber = (i + 1);
                    levelButtonScript.gameplaySceneName = "Reading Gameplay"; // Matches your reading room name!

                    // Setup click listener clean
                    readingButtons[i].onClick.RemoveAllListeners();
                    readingButtons[i].onClick.AddListener(() => levelButtonScript.LoadThisLevel());
                }
            }
        }

        // 4. Position the little slime avatar on the map path
        int currentSlot = readingLevel - 1;
        if (currentSlot >= 0 && currentSlot < readingButtons.Length && readingButtons[currentSlot] != null)
        {
            slimeIcon.position = readingButtons[currentSlot].transform.position;
        }
    }
}