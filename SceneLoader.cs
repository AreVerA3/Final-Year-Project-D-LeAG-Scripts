using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneLoader : MonoBehaviour
{

    public void LoadProfile()
    {
        SceneManager.LoadScene("Profile");
        Debug.Log("Jumping to Profile scene!");
    }
    public void LoadSpelling()
    {
        SceneManager.LoadScene("Spelling");
        Debug.Log("Jumping to Spelling scene!");
    }

    public void LoadWardrobe()
    {
        SceneManager.LoadScene("Wardrobe");
        Debug.Log("Jumping to Wardrobe scene!");
    }

    public void LoadReading()
    {
        SceneManager.LoadScene("Reading");
        Debug.Log("Jumping to Reading scene!");
    }

    public void LoadMainDashboard()
    {
        SceneManager.LoadScene("Main Dashboard");
        Debug.Log("Jumping to Main Dashboard scene!");
    }
}