using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Clock : MonoBehaviour {

    TextMeshPro timeDisplay;
    
	void Start () {
        timeDisplay = GetComponentInChildren<TextMeshPro>();
	}
	
	void Update () {
        String time = DateTime.Now.ToString("HH:mm");
        timeDisplay.text = time;

    }
}
