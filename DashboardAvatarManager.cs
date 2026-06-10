using UnityEngine;
using UnityEngine.UI;

public class DashboardAvatarLoader : MonoBehaviour
{
    [Header("Dashboard Display UI Layers")]
    public Image headLayer;
    public Image eyesLayer;

    [Header("Wardrobe Inventories (Must Match AdvancedWardrobeManager Lists)")]
    public WardrobeItem[] headItems;
    public WardrobeItem[] eyesItems;

    void Start()
    {
        // Load up Rimuru's saved look the exact second the dashboard scene opens!
        LoadEquippedOutfit();
    }

    void LoadEquippedOutfit()
    {
        // 1. Fetch the exact active string IDs saved from your wardrobe scene
        string equippedHeadID = PlayerPrefs.GetString("Equipped_Head", "");
        string equippedEyesID = PlayerPrefs.GetString("Equipped_Eyes", "");

        // 2. Find and apply the corresponding sprites from your collections
        ApplySprite(headLayer, FindSpriteByID(headItems, equippedHeadID));
        ApplySprite(eyesLayer, FindSpriteByID(eyesItems, equippedEyesID));
        
        Debug.Log($"Dashboard Slime Synced! Head Item ID: '{equippedHeadID}' | Eyes Item ID: '{equippedEyesID}'");
    }

    void ApplySprite(Image layer, Sprite sprite)
    {
        if (layer == null) return;

        if (sprite != null)
        {
            layer.sprite = sprite;
            layer.color = new Color(1, 1, 1, 1); // Make fully visible
        }
        else
        {
            layer.sprite = null;
            layer.color = new Color(1, 1, 1, 0); // Hide the layer completely if nothing is worn
        }
    }

    Sprite FindSpriteByID(WardrobeItem[] list, string id)
    {
        if (list == null || string.IsNullOrEmpty(id)) return null;

        foreach (var item in list)
        {
            if (item.itemID == id) return item.itemSprite;
        }
        return null;
    }
}