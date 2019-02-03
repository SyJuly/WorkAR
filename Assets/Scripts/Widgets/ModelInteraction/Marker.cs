using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public Vector3 point;

    OpenInputNote opener;

    private void Awake()
    {
        opener = GetComponent<OpenInputNote>();
    }

    private void Start()
    {
        opener.OnInputClicked(new InputClickedEventData(UnityEngine.EventSystems.EventSystem.current));
    }
}
