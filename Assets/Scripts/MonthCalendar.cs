using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonthCalendar : MonoBehaviour {

    [SerializeField]
    TextMeshPro monthDisplay;

    Renderer renderer;

    [SerializeField]
    GameObject dayPrefab;

    [SerializeField]
    float borderLeft = 0.05f;

    [SerializeField]
    float borderTop = 0.25f;

    int indexFirstDayOfWeekOfMonth = 0;

    int daysInMonth = 30;

    // days in a week
    int numberOfRows = 7;

    // possible weeks in a month plus one for weekdays display
    int numberOfColumns = 6;



    private void Start()
    {
        renderer = GetComponent<Renderer>();
        DateTime today = DateTime.Now;
        monthDisplay.text = today.ToString("MMMM");
        DateTime firstOfMonth = new DateTime(today.Year, today.Month, 1);
        indexFirstDayOfWeekOfMonth = (int)firstOfMonth.DayOfWeek;
        daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
        PlaceDayFields();
        
    }

    private void PlaceDayFields()
    {
        float x = renderer.bounds.size.x;
        float y = renderer.bounds.size.y;
        float divX = ((x - borderLeft * 2) / numberOfRows);
        float divY = ((y - borderTop * 2) / numberOfColumns);
        float leftAlign = ((x - borderLeft * 2) / 2);
        float topAlign = ((y - borderTop * 2) / 2);
        
        int dayIndex = 0;
        bool startIndexing = false;

        float divCounterY = divY;
        for (int n = 0; n < numberOfColumns; n++)
        {
            float divCounterX = divX;
            float fieldY = gameObject.transform.position.y - divCounterY + topAlign - topAlign / numberOfColumns + divY / 2;
            divCounterY += divY;
            for (int i = 0; i < numberOfRows; i++)
            {

                GameObject dayField = Instantiate(dayPrefab);
                dayField.transform.parent = transform;
                float fieldX = gameObject.transform.position.x + divCounterX - leftAlign - divX / 2;
                dayField.transform.position = new Vector3(fieldX, fieldY, gameObject.transform.position.z - 0.02f);
                divCounterX += divX;

                if (n == 0)
                {   //first row shows days of week
                    makeWeekDayField(dayField, i);
                } else if (!startIndexing)
                {   
                    if(n == 1 && i == indexFirstDayOfWeekOfMonth)
                    {   //when first week day of current month is drawn, start counting
                        dayIndex = 1;
                        startIndexing = true;
                    } else
                    {   //if dayField is before first week day of current month
                        dayField.SetActive(false);
                    }
                } else
                {
                    dayField.GetComponent<DayField>().representedDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, dayIndex);
                    dayIndex++;
                    if (dayIndex > daysInMonth)
                    {
                        startIndexing = false;
                    }
                }
            }
        }
    }

    private void makeWeekDayField(GameObject dayFieldGO, int i)
    {
        dayFieldGO.GetComponent<DayField>().enabled = false;
        WeekDayField weekDayField = dayFieldGO.GetComponent<WeekDayField>();
        weekDayField.enabled = true;
        weekDayField.dayOfWeek = i;
    }
}
