using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Log : MonoBehaviour
{
    public TextMeshProUGUI logText;
    private Queue<string> logQueue = new Queue<string>();
    public int maxLines = 20;

    public void handleLog(string logString)
    {
        string logEntry = logString;
        logQueue.Enqueue(logEntry);

        if (logQueue.Count > maxLines)
        {
            logQueue.Dequeue();

        }
        logText.text = string.Join("\n", logQueue.ToArray());

    }
}
