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

    int day = 0;

    // days in a week
    int numberOfRows = 7;

    // possible weeks in a month plus one for weekdays display
    int numberOfColumns = 6;

    string[] weekDays = new string[] { "MON","TUE", "WED", "THU", "FRI", "SAT", "SUN" };


    private void Start()
    {
        renderer = GetComponent<Renderer>();
        PlaceDayFields();
    }

    void Update()
    {
        int newDay = DateTime.Now.Day;
        if (day != newDay)
        {
            monthDisplay.text = DateTime.Now.ToString("MMMM");
        }
    }

    private void PlaceDayFields()
    {
        float x = renderer.bounds.size.x;
        float y = renderer.bounds.size.y;
        float divX = ((x - borderLeft * 2) / numberOfRows);
        float divY = ((y - borderTop * 2) / numberOfColumns);
        float leftAlign = ((x - borderLeft * 2) / 2);
        float topAlign = ((y - borderTop * 2) / 2);

        float divCounterX = divX;
        
        for (int n = 0; n < numberOfRows; n++)
        {
            float fieldX = gameObject.transform.position.x + divCounterX - leftAlign - divX / 2;
            divCounterX += divX;
            float divCounterY = divY;
            for (int i = 0; i < numberOfColumns; i++)
            {
                GameObject dayField = Instantiate(dayPrefab);
                dayField.transform.parent = transform;
                if (i == 0)
                {
                    makeWeekDayField(dayField, n);
                }
                float fieldY = gameObject.transform.position.y - divCounterY + topAlign - topAlign / numberOfColumns + divY / 2;
                dayField.transform.position = new Vector3(fieldX, fieldY, gameObject.transform.position.z - 0.02f);
                divCounterY += divY;
            }
        }


        /*float divCounterY = divY;
        for (int n = 0; n < numberOfColumns; n++)
        {
            float divCounterX = divX;
            float topAlign = ((y - borderTop * 2)/ 2);
            float fieldY = gameObject.transform.position.y - divCounterY + topAlign - topAlign / numberOfColumns + divY / 2;
            divCounterY += divY;
            for (int i = 0; i < numberOfRows; i++)
            {
                GameObject dayField = Instantiate(dayPrefab);
                dayField.transform.parent = transform;
                if (n == 0)
                {
                    makeWeekDayField(dayField, i);
                }
                float leftAlign = ((x - borderLeft * 2) / 2);
                dayField.transform.position = new Vector3(gameObject.transform.position.x + divCounterX - leftAlign - divX / 2, fieldY, gameObject.transform.position.z - 0.02f);
                divCounterX += divX;
            }
        }*/

    }

    private void makeWeekDayField(GameObject dayField, int i)
    {
        TextMeshPro tm = dayField.GetComponentInChildren<TextMeshPro>();
        tm.text = weekDays[i];
        Color defaultColor = dayField.GetComponent<MeshRenderer>().material.color;
        Color transparentColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);
        dayField.GetComponent<MeshRenderer>().material.color = transparentColor;
    }
}
