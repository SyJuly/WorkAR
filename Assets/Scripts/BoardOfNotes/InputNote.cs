using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputNote : MonoBehaviour {

    TextMeshProUGUI textField;

    private void Start()
    {
        textField = GetComponentInChildren<TextMeshProUGUI>();
        SetToActiveView();
    }

    public void SendHypothesis(string message)
    {
        textField.text = message;
    }

    public void SetToActiveView()
    {
        StartCoroutine(TransformToActiveView());
    }

    IEnumerator TransformToActiveView()
    {
        Vector3 goalScale = transform.localScale;
        float step = 0.1f;
        while(step < 1)
        {
            transform.localScale = goalScale * step;
            step += step;
            yield return new WaitForSeconds(step);
        }
    }
}
