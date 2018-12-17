using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputNote : MonoBehaviour {

    TextMeshProUGUI textField;

    [SerializeField]
    int timeToLerp = 1;
    
    public ConfirmButton confirmButton;
    
    public CancelButton cancelButton;

    private void Awake()
    {
        confirmButton = GetComponentInChildren<ConfirmButton>();
        cancelButton = GetComponentInChildren<CancelButton>();
        textField = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
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

    public void CompleteInput()
    {
        StartCoroutine(Disappear());
    }

    IEnumerator TransformToActiveView()
    {
        float distance = GetComponent<Tagalong>().TagalongDistance;
        float counter = 0;
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance;
        Vector3 destinationScale = transform.localScale;
        Vector3 originalScale = Vector3.zero;

        while (counter < timeToLerp)
        {
            counter += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, destinationScale, counter / timeToLerp);
            yield return null;
        }
    }

    IEnumerator Disappear()
    {
        float counter = 0;
        Vector3 originalScale = transform.localScale;
        Vector3 destinationScale = Vector3.zero;

        while (counter < timeToLerp)
        {
            counter += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, destinationScale, counter / timeToLerp);
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
