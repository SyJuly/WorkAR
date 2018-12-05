using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorField : MonoBehaviour {
    /*------------------Singleton---------------------->>*/
    public TextMeshPro textMesh;
    
    private static ErrorField _instance;

    public static ErrorField Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        textMesh = GetComponentInChildren<TextMeshPro>();
    }
    /*<<------------------Singleton-----------------------*/
}
