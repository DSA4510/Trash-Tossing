using UnityEngine;
using TMPro;

public class scoreUpdater : MonoBehaviour {
    public TextMeshProUGUI myText;
    public TextMeshProUGUI logText;
    private int Count = 0;
    // public AudioSource mySource;
	// Use this for initialization
	void Start () {
        Application.logMessageReceived += HandleScoreMessage;
        Application.logMessageReceived += HandleLogMessage;
    }

    // Update is called once per frame

    void OnDestroy()
    {
        // Unsubscribe from log message events
        Application.logMessageReceived -= HandleScoreMessage;
        Application.logMessageReceived -= HandleLogMessage;
    }

    void HandleScoreMessage(string logMessage, string stackTrace, LogType logType)
    {
        if (logType == LogType.Log)
        {
            // Append log message to the UI Text component
            if (logMessage == "Score")
            {
                Count++;
                myText.SetText(string.Format("Score : {0}", Count.ToString()));
            }
        }
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
