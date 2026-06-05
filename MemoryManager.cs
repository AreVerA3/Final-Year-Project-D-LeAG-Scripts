using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;

public class MemoryManager : MonoBehaviour
{
    public InputField nameInputField; 
    public TextMeshProUGUI slimeChatBubble; 
    public GameObject friendNamePanel; 

    public void ConfirmNameEntered()
    {
        string typedName = nameInputField.text;
        
        PlayerPrefs.SetString("SlimeName", typedName);
        PlayerPrefs.SetInt("NameFinished", 1);
        PlayerPrefs.Save();
        StartCoroutine(ClosePanelAfterDelay());
    }

    IEnumerator ClosePanelAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Main Dashboard");
    }

    // --- CANCEL BUTTON FEATURE ---
    public void ClearTypingBox()
    {
        // This replaces whatever they typed with absolutely nothing!
        nameInputField.text = ""; 
    }
}