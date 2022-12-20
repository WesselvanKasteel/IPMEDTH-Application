using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevLogger : MonoBehaviour
{
    private TextMeshProUGUI logger;

    private int messageCounter = 0;

    private void Awake()
    {
        logger = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void printLogMessage(string message)
    {
        logger.text += "<br>[" + messageCounter + "] | " + message;
        messageCounter++;
    }
}
