using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class DevShowInformation : MonoBehaviour
{
    private static Camera arCamera;

    // 2D
    [SerializeField] private int InteractionNumber;

    // Reference DevLogger script
    private DevLogger devLogger;
    private DevUiController devUiController;

    [SerializeField] GameObject[] interactionGlows;
    [SerializeField] GameObject interactionGlow;

    private void Awake()
    {
        devLogger = GameObject.FindGameObjectWithTag("Logs").GetComponent<DevLogger>();
        devUiController = GameObject.FindGameObjectWithTag("UiController").GetComponent<DevUiController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        var all = FindObjectsOfType<Camera>();

        foreach (Camera item in all)
        {
            if (item.tag.CompareTo("ArCamera") == 0)
            {
                arCamera = item;
            }
        }

        interactionGlow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            Touch touch = Input.touches[0];
            Vector3 pos = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(pos);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Button" && hit.collider.name == gameObject.name)
                    {
                        //devLogger.printLogMessage("Button pressed: " + hit.collider.name);

                        devUiController.ShowSpecificInformation();
                        devUiController.ShowSpecificInteraction(InteractionNumber);
                        devUiController.HideBasicInformation();
                    }
                }
            }
        }
        catch
        {
            return;
        }
    }

    public void ShowGlow(int number)
    {
        foreach (GameObject interactionGlow in interactionGlows)
        {
            int interactionGlowNumber = (int)Variables.Object(interactionGlow).Get("glowNumber");

            if (interactionGlowNumber == number)
            {
                interactionGlow.SetActive(true);
            }
            else
            {
                interactionGlow.SetActive(false);

                GameObject parent = interactionGlow.transform.parent.gameObject;
                Renderer r = parent.GetComponentInParent<Renderer>();
                Color color = r.material.color;
                color.a = (float)0.75;
                r.material.color = color;
            }
        }
    }

    public void hideGlow()
    {
        foreach (GameObject interactionGlow in interactionGlows)
        {
            interactionGlow.SetActive(false);
        }
    }
}
