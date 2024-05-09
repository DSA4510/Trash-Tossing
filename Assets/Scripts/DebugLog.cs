using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugLog : MonoBehaviour
{
    public TextMeshProUGUI logText;

    void Start()
    {
        // Clear existing text

        // Subscribe to log message events
        Application.logMessageReceived += HandleLogMessage;
    }

    void OnDestroy()
    {
        // Unsubscribe from log message events
        Application.logMessageReceived -= HandleLogMessage;
    }

    void HandleLogMessage(string logMessage, string stackTrace, LogType logType)
    {
        if (logType == LogType.Log)
        {
            // Append log message to the UI Text component
            logText.SetText(logMessage + "\n");
        }
    }
}
