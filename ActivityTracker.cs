using UnityEngine;
using System; // Needed for DateTime

public class ActivityTracker : MonoBehaviour
{
    public void LogDailyActivity()
    {
        string todayKey = "Played_" + DateTime.Now.ToString("yyyy_MM_dd");
        PlayerPrefs.SetInt(todayKey, 1);
        PlayerPrefs.Save();
        Debug.Log("Activity stamped for: " + todayKey);
    }
}