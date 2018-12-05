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

    void OnEnable()
    {
        //TODO: Uncomment for logging debugs
        //Application.logMessageReceived += handleUnityLog;
    }

    private void handleUnityLog(string logString, string stackTrace, LogType type)
    {
        ErrorField.Instance.textMesh.text = "Trace: " + logString + "\n" + ErrorField.Instance.textMesh.text;
        ErrorField.Instance.textMesh.text = "StackTrace: " + stackTrace.ToString() + "\n" + ErrorField.Instance.textMesh.text;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= handleUnityLog;
    }

    private void Start()
    {
        textMesh = GetComponentInChildren<TextMeshPro>();
    }
    /*<<------------------Singleton-----------------------*/
}
