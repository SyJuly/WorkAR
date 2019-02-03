using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeekDayField : MonoBehaviour
{
    public int dayOfWeek;

    string[] weekDays = new string[] { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };

    TextMeshProUGUI textField;

    void Start()
    {
        textField = GetComponentInChildren<TextMeshProUGUI>();
        textField.text = weekDays[dayOfWeek];
    }
}
