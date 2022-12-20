using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class DevArtPiece1Controller : MonoBehaviour
{
    private static Camera arCamera;

    public GameObject text1;

    // Reference to scripts
    private DevLogger devLogger;

    private void Awake()
    {
        // Get script-component from GameObject
        devLogger = GameObject.FindGameObjectWithTag("Logs").GetComponent<DevLogger>();
    }

    // Start is called before the first frame update
    void Start()
    {
        text1.SetActive(false);
            
        var all = FindObjectsOfType<Camera>();

        foreach (Camera item in all)
        {
            if (item.tag.CompareTo("ArCamera") == 0)
            {
                arCamera = item;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Touch touch = Input.touches[0];
        Vector3 pos = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = arCamera.ScreenPointToRay(pos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                devLogger.printLogMessage("Hit: " + hit.collider.name);

                if (hit.collider.tag == "Button")
                {
                    devLogger.printLogMessage("Button pressed");

                    if (text1.activeSelf == false)
                    {
                        text1.SetActive(true);
                    }
                    else
                    {
                        text1.SetActive(false);
                    }

                }
            }
        }
    }
}
