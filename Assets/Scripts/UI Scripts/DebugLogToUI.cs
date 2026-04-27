using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugLogToUI : MonoBehaviour
{
    public TextMeshProUGUI logText; 
    private Queue<string> logQueue = new Queue<string>();
    public int maxLines = 20;
    
    //From the original project, heavy chatGPT help in the original. Modified to not take debug messages (at least later it will be)
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        //skips yellow warnings and errors
        if (type == LogType.Warning)
            return;
        if (type == LogType.Error)
            return;

        // Format log entry
        string logEntry = logString;
        
        if (type == LogType.Error || type == LogType.Exception)
            logEntry = $"<color=red>{logString}</color>";
        else if (type == LogType.Warning)
            logEntry = $"<color=yellow>{logString}</color>";
        
        logQueue.Enqueue(logEntry);

        if (logQueue.Count > maxLines)
            logQueue.Dequeue();

        logText.text = string.Join("\n", logQueue.ToArray());
    }
}
