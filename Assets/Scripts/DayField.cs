using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayField : MonoBehaviour {
    
    public DateTime representedDay;

    public bool isDayOfWeekField;

    public int dayOfWeek;

    string[] weekDays = new string[] { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };

    GoogleCalendar googleCalendar;

    TextMeshPro textField;

    MeshRenderer renderer;
    
	void Start () {
        googleCalendar = GetComponentInParent<GoogleCalendar>();
        textField = GetComponentInChildren<TextMeshPro>();
        renderer = GetComponent<MeshRenderer>();

    }
	
	void Update () {
        if (isDayOfWeekField)
        {
            textField.text = weekDays[dayOfWeek];
            Color defaultColor = renderer.material.color;
            Color transparentColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);
            renderer.material.color = transparentColor;
        }
        else
        {
            textField.text = representedDay.Day.ToString();
        }
	}
}
