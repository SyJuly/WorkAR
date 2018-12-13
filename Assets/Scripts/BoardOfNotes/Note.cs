using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

    [SerializeField]
    Vector3 activeViewPosition;

    [SerializeField]
    int activeViewScale;

    Tagalong tagalong;

    public bool isUsed;

    private void Start()
    {
       tagalong = GetComponent<Tagalong>();
    }

    public void SetToActiveView()
    {
        StartCoroutine(TransformToActiveView());
        tagalong.enabled = true;
    }

    IEnumerator TransformToActiveView()
    {
        float step = 0.1f;
        while(step < 1)
        {
            transform.transform.localPosition = transform.transform.localPosition + activeViewPosition * step;
            transform.localScale = transform.localScale * activeViewScale * step;
            step += step;
            yield return new WaitForSeconds(step);
        }
    }
}
