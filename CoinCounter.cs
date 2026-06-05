using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI coinText; 

    // Changed to OnEnable so it updates the absolute second the scene opens
    void OnEnable()
    {
        // Safety net: if you forget to drag the text in the Inspector, this finds it automatically!
        if (coinText == null)
        {
            coinText = GetComponent<TextMeshProUGUI>();
        }
        
        UpdateCoinDisplay();
    }

    public void AddCoins(int amountToAdd)
    {
        int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0); 
        currentCoins += amountToAdd; 
        
        PlayerPrefs.SetInt("TotalCoins", currentCoins);
        PlayerPrefs.Save();

        UpdateCoinDisplay();         
    }

    // Keeping your perfect Wardrobe logic untouched!
    public bool SpendCoins(int amountToSpend)
    {
        int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        
        if (currentCoins >= amountToSpend)
        {
            currentCoins -= amountToSpend;
            PlayerPrefs.SetInt("TotalCoins", currentCoins);
            PlayerPrefs.Save();
            UpdateCoinDisplay();
            return true; // Purchase successful!
        }
        
        return false; // Not enough money!
    }

    private void UpdateCoinDisplay()
    {
        if (coinText != null)
        {
            int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);
            coinText.text = currentCoins.ToString();
        }
    }
}