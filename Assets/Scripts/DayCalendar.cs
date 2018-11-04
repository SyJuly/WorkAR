using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayCalendar : MonoBehaviour {

    [SerializeField]
    TextMeshPro dayDisplay;

    [SerializeField]
    TextMeshPro monthYearDisplay;

    int day = 0;

	void Update () {
        int newDay = DateTime.Now.Day;
        if (day != newDay) {
            dayDisplay.text = DateTime.Now.ToString("dd");
            monthYearDisplay.text = DateTime.Now.ToString("MM/yy");
        }
		
	}
}
