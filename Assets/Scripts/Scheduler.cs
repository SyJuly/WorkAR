using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scheduler : MonoBehaviour {

    [SerializeField]
    TextMeshPro monthDisplay;

    int day = 0;

    void Update()
    {
        int newDay = DateTime.Now.Day;
        if (day != newDay)
        {
            monthDisplay.text = DateTime.Now.ToString("MMMM");
        }

    }
}
