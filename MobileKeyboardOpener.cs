using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; // Use this if you are using TextMeshPro Input Fields
// using UnityEngine.UI; // Uncomment this instead if you are using Legacy Input Fields

public class MobileKeyboardOpener : MonoBehaviour, IPointerClickHandler
{
    // This interface automatically detects when this specific object is tapped
    public void OnPointerClick(PointerEventData eventData)
    {
        // Forces the native mobile keyboard to slide up instantly on touch
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        Debug.Log("Mobile keyboard forced open for: " + gameObject.name);
    }
}