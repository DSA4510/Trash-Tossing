using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class scoreUpdater : MonoBehaviour {
    public TextMeshProUGUI myText;
    public TextMeshProUGUI logText;
    public TextMeshProUGUI distanceText;
    public Image wind;
    private int Count = 0;
    private float distance = 0;
    private GameObject bin;
    private int windAngle = 0;
    private AudioSource audioSource;
    // public AudioSource mySource;
    // Use this for initialization
    void Start () {
        Application.logMessageReceived += HandleMessage;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame

    void OnDestroy()
    {
        // Unsubscribe from log message events
        Application.logMessageReceived -= HandleMessage;
    }
    void Update()
    {
        if (bin == null)
        {
            bin = GameObject.FindGameObjectWithTag("Bin");
            return;
        }
        Vector2 distanceVector = new Vector2(bin.transform.position.x, bin.transform.position.z) - new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z);
        distance = Vector2.SqrMagnitude(distanceVector)/2f;
        distanceText.SetText(string.Format("Distance: {0:00.00}m", distance));
    }

    void HandleMessage(string logMessage, string stackTrace, LogType logType)
    {
        if (logType == LogType.Log)
        {
            // Append log message to the UI Text component
            if (logMessage == "Score")
            {
                Count++;
                myText.SetText(string.Format("Score: {0}", Count.ToString()));
                audioSource.time = 0f;
                audioSource.Play();
                StartCoroutine(StopSound());
            }
            if (logMessage == "Throw")
            {
                windAngle = UnityEngine.Random.Range(-24, 25) * 5;
                wind.transform.rotation = Quaternion.Euler(0f, 0f, windAngle);
            }
            if (logMessage[0] == 'C')
            {
                logText.SetText(logMessage + "\n");
            }
        }
    }

    IEnumerator StopSound()
    {
        yield return new WaitForSeconds(0.5f);
        audioSource.Stop();
    }
}
