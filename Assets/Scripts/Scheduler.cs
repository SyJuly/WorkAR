using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scheduler : MonoBehaviour {

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

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        startEditingMode();
    }

    void Update()
    {
        int newDay = DateTime.Now.Day;
        if (day != newDay)
        {
            monthDisplay.text = DateTime.Now.ToString("MMMM");
        }
    }

    private void startEditingMode()
    {
        gameObject.transform.position = new Vector3(-0.8f, gameObject.transform.position.y, gameObject.transform.position.z);
        gameObject.transform.localScale = new Vector3(2.3f, 1.7f, 1f);

        float x = renderer.bounds.size.x;
        float y = renderer.bounds.size.y;
        float divX = ((x - borderLeft * 2) / 7);
        float divY = ((y - borderTop * 2) / 5);

        float divCounterY = divY;
        for (int n = 0; n < 5; n++)
        {
            float divCounterX = divX;
            float topAlign = ((y - borderTop * 2)/ 2);
            float fieldY = gameObject.transform.position.y - divCounterY + topAlign - topAlign / 6 + divY / 2;
            divCounterY += divY;
            for (int i = 0; i < 7; i++)
            {
                GameObject dayField = Instantiate(dayPrefab);
                float leftAlign = ((x - borderLeft * 2) / 2);
                dayField.transform.position = new Vector3(gameObject.transform.position.x + divCounterX - leftAlign - divX / 2, fieldY, gameObject.transform.position.z - 0.02f);
                divCounterX += divX;
            }
        }

    }
}
