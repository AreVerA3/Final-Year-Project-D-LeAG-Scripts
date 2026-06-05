using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalendarManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI monthYearText;  
    public GameObject dayPrefab;           
    public GameObject emptyPrefab;         
    public Transform calendarGrid;         

    [Header("Colors")]
    public Color playedColor = new Color(0f, 0.7f, 1f); 
    public Color missedColor = new Color(0.4f, 0.4f, 0.4f); 

    //Keeps track of the month we are currently viewing
    private DateTime currentViewDate;

    void Start()
    {
        // Start by looking at today's real date
        currentViewDate = DateTime.Now;
        GenerateLiveCalendar();
    }

    //go back one month
    public void PreviousMonth()
    {
        currentViewDate = currentViewDate.AddMonths(-1);
        GenerateLiveCalendar(); // Redraw the calendar
    }

    //go forward one month
    public void NextMonth()
    {
        currentViewDate = currentViewDate.AddMonths(1);
        GenerateLiveCalendar(); // Redraw the calendar
    }

    public void GenerateLiveCalendar()
    {
        // 1. Clear out any old days in the grid
        foreach (Transform child in calendarGrid)
        {
            Destroy(child.gameObject);
        }

        // 2. Use our VIEW date instead of the real-time clock
        DateTime firstDayOfMonth = new DateTime(currentViewDate.Year, currentViewDate.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(currentViewDate.Year, currentViewDate.Month);
        
        // 3. Update the Title Text
        if (monthYearText != null)
        {
            monthYearText.text = currentViewDate.ToString("MMMM yyyy");
        }

        // 4. Find out what day of the week the 1st falls on
        int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

        // 5. Spawn the invisible padding blocks
        for (int i = 0; i < startDayOfWeek; i++)
        {
            Instantiate(emptyPrefab, calendarGrid);
        }

        // 6. Spawn the actual functioning days
        for (int i = 1; i <= daysInMonth; i++)
        {
            GameObject newDay = Instantiate(dayPrefab, calendarGrid);
            newDay.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            
            // Check memory using the exact year and month we are currently viewing
            string dateKey = $"Played_{currentViewDate.Year}_{currentViewDate.Month:D2}_{i:D2}";
            
            Image circleImage = newDay.GetComponent<Image>();
            
            if (PlayerPrefs.GetInt(dateKey, 0) == 1) 
            {
                circleImage.color = playedColor;
            }
            else 
            {
                circleImage.color = missedColor;
            }
        }
    }
}