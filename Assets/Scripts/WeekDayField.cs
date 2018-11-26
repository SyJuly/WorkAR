using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeekDayField : MonoBehaviour
{
    public int dayOfWeek;

    string[] weekDays = new string[] { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };

    TextMeshPro textField;

    MeshRenderer renderer;

    void Start()
    {
        textField = GetComponentInChildren<TextMeshPro>();
        renderer = GetComponent<MeshRenderer>();
        textField.text = weekDays[dayOfWeek];
        Color defaultColor = renderer.material.color;
        Color transparentColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);
        renderer.material.color = transparentColor;
    }
}
