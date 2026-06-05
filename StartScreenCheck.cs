using UnityEngine;

public class StartScreenCheck : MonoBehaviour
{
    void Start()
    {
        // Check the memory vault as soon as the game opens
        if (PlayerPrefs.GetInt("NameFinished", 0) == 1)
        {
            // If the note exists, instantly turn this entire panel off!
            gameObject.SetActive(false);
        }
    }
}