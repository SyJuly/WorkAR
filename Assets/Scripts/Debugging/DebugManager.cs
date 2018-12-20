using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The DebugManager delivers the debug output to a 3D TextMesh.
/// </summary>
public class DebugManager : MonoBehaviour {

    public Text canvasText; 
    public TextMesh textMesh;
    public TextMeshPro proText;
    public string ignoreIfContains;
    public int outputSize = 10;
    public bool IgnoreDetails;

    private List<string> outputArray;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void Awake()
    {
        outputArray = new List<string>();
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
            switch (type)
            {
                case LogType.Warning:
                    logString = ColoredLogString("ffff00ff", logString, stackTrace, IgnoreDetails);
                    break;
                case LogType.Exception:
                    logString = ColoredLogString("ff0000ff", logString, stackTrace, IgnoreDetails);
                    break;
                case LogType.Error:
                    logString = ColoredLogString("ff0000ff", logString, stackTrace, IgnoreDetails);
                    break;
                case LogType.Log:
                    logString = ColoredLogString("ffffffff", logString, stackTrace, IgnoreDetails);
                    break;
            }
            if(canvasText != null)
                canvasText.text = UpdateOutput(logString);

            if (textMesh != null)
                textMesh.text = UpdateOutput(logString);

            if (proText != null)
                proText.text = UpdateOutput(logString);
       
    }

    void Update()
    {
        //TODO remove
        transform.LookAt(2 * transform.position - Camera.main.transform.position);

        //Test keys
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("log");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.LogWarning("warning");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.LogError("error/exception");
        }
    }

    private string UpdateOutput(string str)
    {
        var finalOutput = "";

        outputArray.Add(str);

        if(outputArray.Count > outputSize)
        {
            outputArray.Remove(outputArray[0]);
        }

        for(int i = 0; i < outputArray.Count; i++)
        {
            finalOutput += outputArray[i] + "\n";
        }

        return finalOutput;
    }

    private string ColoredLogString(string hexColor, string logString, string stackTrace, bool details)
    {
        var result = $"<color=#{hexColor}>{logString}</color>";
        if (!IgnoreDetails)
            result += $"\n<color=#{hexColor}>{stackTrace}</color>";

        return result;
    }
}
