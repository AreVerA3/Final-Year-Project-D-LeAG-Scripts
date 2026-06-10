using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 

[System.Serializable]
public class WardrobeItem
{
    public string itemID;      
    public Sprite itemSprite;  
    public int price;          
}

public class AdvancedWardrobeManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainDisplayPanel;
    public GameObject editShopPanel;
    public GameObject failBuyPanel;          // NEW: The fail pop-up!

    [Header("Slime Display Layers")]
    public Image headLayer;
    public Image eyesLayer;

    [Header("Slime Stats Display")]
    public TextMeshProUGUI slimeStatsText; 

    [Header("Edit Name Panel")]
    public GameObject editNamePanel;         
    public TMP_InputField newSlimeNameInput;  

    [Header("Shop UI Elements")]
    public GameObject lockIcon;              
    public TextMeshProUGUI categoryText;     
    public TextMeshProUGUI coinCountText;    
    public Button actionButton;              
    public TextMeshProUGUI actionButtonText; 
    public Image actionButtonImage;          

    [Header("Button Colors")]
    public Color colorWear = new Color(0.6f, 1f, 0.6f);      
    public Color colorUnwear = new Color(1f, 0.7f, 0.7f);    
    public Color colorBuy = new Color(0.6f, 0.4f, 0.8f);     

    [Header("Wardrobe Inventories")]
    public WardrobeItem[] headItems;
    public WardrobeItem[] eyesItems;

    [Header("Navigation")]
    public string sceneToLoadOnBack = "MapScene"; 

    private int currentCategoryIndex = 0; 
    private int currentItemIndex = 0;
    private int totalCoins = 0;

    void Start()
    {
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 50); 
        UpdateCoinDisplay();
        UpdateStatsDisplay(); 
        LoadEquippedOutfit();
        CloseShop(); 
        
        // Hide both extra panels when the scene starts up cleanly
        if (failBuyPanel != null) failBuyPanel.SetActive(false); 
        if (editNamePanel != null) editNamePanel.SetActive(false);
    }

    public void OpenEditNamePanel()
    {
        if (editNamePanel != null)
        {
            editNamePanel.SetActive(true);
            
            // Pre-fill input field with current name so it's not empty
            if (newSlimeNameInput != null)
            {
                newSlimeNameInput.text = PlayerPrefs.GetString("SlimeName", "Rimuru");
            }
        }
    }

    public void SaveNewSlimeName()
    {
        if (newSlimeNameInput == null || editNamePanel == null)
        {
            Debug.LogError("Hey! You forgot to drag the Edit Panel or Input Field into the Inspector slots!");
            return;
        }

        string newName = newSlimeNameInput.text.Trim();

        if (!string.IsNullOrWhiteSpace(newName))
        {
            // 1. Save permanently
            PlayerPrefs.SetString("SlimeName", newName);
            PlayerPrefs.Save();

            // 2. Refresh the stats block text card
            UpdateStatsDisplay();

            // 3. Close the popup edit panel
            editNamePanel.SetActive(false);
            
            // 4. BRING BACK THE MAIN WARDROBE INTERFACE!
            if (mainDisplayPanel != null)
            {
                mainDisplayPanel.SetActive(true);
            }
            
            Debug.Log("Slime name successfully updated to: " + newName);
        }
    }
    
    public void CloseEditNamePanel()
    {
        if (editNamePanel != null)
        {
            editNamePanel.SetActive(false);
        }

        // BRING BACK THE MAIN WARDROBE INTERFACE IF THEY CANCEL!
        if (mainDisplayPanel != null)
        {
            mainDisplayPanel.SetActive(true);
        }
    }

    public void OpenShop()
    {
        mainDisplayPanel.SetActive(false);
        editShopPanel.SetActive(true);
        currentCategoryIndex = 0;
        currentItemIndex = 0;
        RefreshShopUI();
    }

    public void CloseShop()
    {
        editShopPanel.SetActive(false);
        mainDisplayPanel.SetActive(true);
        LoadEquippedOutfit(); 
        
        if (lockIcon != null) lockIcon.SetActive(false); 
    }

    public void NextCategory()
    {
        currentCategoryIndex = (currentCategoryIndex + 1) % 2; 
        currentItemIndex = 0;
        RefreshShopUI();
    }

    public void PreviousCategory()
    {
        currentCategoryIndex--;
        if (currentCategoryIndex < 0) currentCategoryIndex = 1; 
        currentItemIndex = 0;
        RefreshShopUI();
    }

    public void NextItem()
    {
        WardrobeItem[] currentList = GetCurrentCategoryList();
        if (currentList.Length == 0) return;

        currentItemIndex = (currentItemIndex + 1) % currentList.Length;
        RefreshShopUI();
    }

    public void PreviousItem()
    {
        WardrobeItem[] currentList = GetCurrentCategoryList();
        if (currentList.Length == 0) return;

        currentItemIndex--;
        if (currentItemIndex < 0) currentItemIndex = currentList.Length - 1;
        RefreshShopUI();
    }

    void RefreshShopUI()
    {
        string categoryName = "";
        WardrobeItem[] currentList = GetCurrentCategoryList();
        Image targetLayer = null;
        string equippedPrefKey = "";

        switch (currentCategoryIndex)
        {
            case 0: categoryName = "Head"; targetLayer = headLayer; equippedPrefKey = "Equipped_Head"; break;
            case 1: categoryName = "Eyes"; targetLayer = eyesLayer; equippedPrefKey = "Equipped_Eyes"; break;
        }

        categoryText.text = categoryName;

        if (currentList.Length == 0) return;

        WardrobeItem viewedItem = currentList[currentItemIndex];
        
        LoadEquippedOutfit(); 
        ApplySprite(targetLayer, viewedItem.itemSprite);

        bool isOwned = (viewedItem.price == 0) || (PlayerPrefs.GetInt("Owned_" + viewedItem.itemID, 0) == 1);
        string currentlyEquippedID = PlayerPrefs.GetString(equippedPrefKey, "");

        if (lockIcon != null)
        {
            lockIcon.SetActive(!isOwned);
        }

        actionButton.onClick.RemoveAllListeners();

        if (!isOwned)
        {
            actionButtonText.text = "GET \u25CF " + viewedItem.price; 
            actionButtonImage.color = colorBuy;
            actionButton.onClick.AddListener(() => BuyItem(viewedItem));
        }
        else if (currentlyEquippedID == viewedItem.itemID)
        {
            actionButtonText.text = "UN-WEAR";
            actionButtonImage.color = colorUnwear;
            actionButton.onClick.AddListener(() => UnwearItem(equippedPrefKey));
        }
        else
        {
            actionButtonText.text = "WEAR";
            actionButtonImage.color = colorWear;
            actionButton.onClick.AddListener(() => WearItem(equippedPrefKey, viewedItem.itemID));
        }
    }

    void BuyItem(WardrobeItem item)
    {
        if (totalCoins >= item.price)
        {
            totalCoins -= item.price;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            PlayerPrefs.SetInt("Owned_" + item.itemID, 1); 
            PlayerPrefs.Save();
            ButtonSoundPlayer.PlaySoundFive();

            UpdateCoinDisplay();
            RefreshShopUI(); 
        }
        else
        {
            Debug.Log("Not enough coins!");
            PanelSoundPlayer.PlayFailSound();

            if (failBuyPanel != null) failBuyPanel.SetActive(true); 
        }
    }

    void WearItem(string prefKey, string itemID)
    {
        PlayerPrefs.SetString(prefKey, itemID);
        PlayerPrefs.Save();
        ButtonSoundPlayer.PlaySoundSix();

        RefreshShopUI();
    }

    void UnwearItem(string prefKey)
    {
        PlayerPrefs.SetString(prefKey, ""); 
        PlayerPrefs.Save();
        ButtonSoundPlayer.PlaySoundSix();

        RefreshShopUI();
    }

    void LoadEquippedOutfit()
    {
        string equippedHead = PlayerPrefs.GetString("Equipped_Head", "");
        ApplySprite(headLayer, FindSpriteByID(headItems, equippedHead));

        string equippedEyes = PlayerPrefs.GetString("Equipped_Eyes", "");
        ApplySprite(eyesLayer, FindSpriteByID(eyesItems, equippedEyes));
    }

    void ApplySprite(Image layer, Sprite sprite)
    {
        if (sprite != null)
        {
            layer.sprite = sprite;
            layer.color = new Color(1, 1, 1, 1);
        }
        else
        {
            layer.sprite = null;
            layer.color = new Color(1, 1, 1, 0); 
        }
    }

    WardrobeItem[] GetCurrentCategoryList()
    {
        if (currentCategoryIndex == 0) return headItems;
        return eyesItems;
    }

    Sprite FindSpriteByID(WardrobeItem[] list, string id)
    {
        foreach (var item in list)
        {
            if (item.itemID == id) return item.itemSprite;
        }
        return null;
    }

    void UpdateCoinDisplay()
    {
        if(coinCountText != null) coinCountText.text = totalCoins.ToString();
    }

    void UpdateStatsDisplay()
    {
        if (slimeStatsText != null)
        {
            string slimeName = PlayerPrefs.GetString("SlimeName", "Rimuru"); 
            int spellingLevel = PlayerPrefs.GetInt("SpellingLevelReached", 1);
            int readingLevel = PlayerPrefs.GetInt("ReadingLevelReached", 1);
            slimeStatsText.text = $"Slime Name: {slimeName}\nSpelling Level: {spellingLevel}\nReading Level: {readingLevel}";
        }
    }

    public void ClickBackButton()
    {
        SceneManager.LoadScene(sceneToLoadOnBack);
    }

    public void CloseFailBuyPanel()
    {
        if (failBuyPanel != null) failBuyPanel.SetActive(false);
    }
}