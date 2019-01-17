﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonthCalendar : MonoBehaviour {

    [SerializeField]
    TextMeshPro monthDisplay;

    Bounds bounds;

    [SerializeField]
    GameObject dayPrefab;

    [SerializeField]
    GameObject weekDayPrefab;

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
        bounds = GetComponent<MeshFilter>().mesh.bounds;
        DateTime today = DateTime.Now;
        monthDisplay.text = today.ToString("MMMM");
        DateTime firstOfMonth = new DateTime(today.Year, today.Month, 1);
        indexFirstDayOfWeekOfMonth = (int)firstOfMonth.DayOfWeek - 1;
        daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
        PlaceDayFields();
        
    }

    private void PlaceDayFields()
    {
        float x = bounds.size.x;
        float y = bounds.size.y;

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
            float fieldY = gameObject.transform.localPosition.y - divCounterY + topAlign - topAlign / numberOfColumns + divY / 2;
            divCounterY += divY;
            for (int i = 0; i < numberOfRows; i++)
            {

                GameObject dayField = (n==0) ? GetNewWeekDayField(i) : GetNewDayField();
                float fieldX = gameObject.transform.localPosition.x + divCounterX - leftAlign - divX / 2;
                dayField.transform.localPosition = new Vector3(fieldX, fieldY, - 0.5f);
                dayField.transform.localRotation = Quaternion.identity;
                
                divCounterX += divX;
                
                if (!startIndexing)
                {   
                    if(n == 1 && i == indexFirstDayOfWeekOfMonth)
                    {   //when first week day of current month is drawn, start counting
                        dayIndex = 1;
                        startIndexing = true;
                    } else if (n != 0)
                    {   //if dayField is before first week day of current month
                        dayField.SetActive(false);
                    }
                }
                if(startIndexing){
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

    private GameObject GetNewWeekDayField(int i)
    {
        GameObject weekDayFieldGO = Instantiate(weekDayPrefab, gameObject.transform);
        WeekDayField weekDayField = weekDayFieldGO.GetComponent<WeekDayField>();
        weekDayField.enabled = true;
        weekDayField.dayOfWeek = i;
        weekDayField.transform.localScale = new Vector3(weekDayPrefab.transform.localScale.x, weekDayPrefab.transform.localScale.y, 0.5f);
        return weekDayFieldGO;
    }

    private GameObject GetNewDayField()
    {
        GameObject dayFieldGO = Instantiate(dayPrefab, gameObject.transform);
        dayFieldGO.transform.localScale = new Vector3(dayPrefab.transform.localScale.x, dayPrefab.transform.localScale.y, 0.5f);
        return dayFieldGO;
    }
}
