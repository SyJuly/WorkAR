using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridArranger : MonoBehaviour
{
    Bounds bounds;

    [SerializeField]
    GameObject fieldPrefab;

    [SerializeField]
    float borderLeft = 0.05f;

    [SerializeField]
    float borderTop = 0.25f;

    [SerializeField]
    int numberOfColumns = 6;

    private void Start()
    {
        bounds = GetComponent<MeshFilter>().mesh.bounds;
        PlaceDayFields();

    }

    private void PlaceDayFields()
    {
        float x = bounds.size.x;
        float y = bounds.size.y;

        float divX = ((x - borderLeft * 2) / numberOfColumns);
        float divY = ((y - borderTop * 2));
        float leftAlign = ((x - borderLeft * 2) / 2);

        float divCounterX = divX;
        for (int n = 0; n < numberOfColumns; n++)
        {

            float fieldY = gameObject.transform.localPosition.y - borderTop;
            
            GameObject field = Instantiate(fieldPrefab, transform);
            float fieldX = gameObject.transform.localPosition.x + divCounterX - leftAlign - divX / 2;
            field.transform.localPosition = new Vector3(fieldX, fieldY, -0.5f);
            field.transform.localRotation = Quaternion.identity;
            field.transform.localScale = new Vector3(fieldPrefab.transform.localScale.x, fieldPrefab.transform.localScale.y, 0.5f);

            divCounterX += divX;

        }
    }
}
