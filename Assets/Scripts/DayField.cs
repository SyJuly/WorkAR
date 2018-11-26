using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayField : MonoBehaviour {
    
    public DateTime representedDay;

    GoogleCalendar googleCalendar;

    TextMeshPro textField;
    
	void Start () {
        googleCalendar = GetComponentInParent<GoogleCalendar>();
        textField = GetComponentInChildren<TextMeshPro>();

    }
	
	void Update () {
        textField.text = representedDay.Day.ToString();
	}
}
