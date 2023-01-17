using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevDistanceController : MonoBehaviour
{

    private float minDist = 1.25f;

    private float fadeSpeed = 2f;
    public float timeToStartFading = 0.5f;

    private static GameObject arCamera;

    public GameObject[] interactions;


    // Reference to scripts
    private DevUiController devUiController;
    private DevPositionCalculator positionCalculator;
    private DevCreateAnchors devCreateAnchors;
    private DevShowInformation devShowInformation;

    private void Awake()
    {
        devUiController = GameObject.FindGameObjectWithTag("UiController").GetComponent<DevUiController>();
        positionCalculator = GameObject.FindGameObjectWithTag("PositionCalculator").GetComponent<DevPositionCalculator>();
        devCreateAnchors = GameObject.FindGameObjectWithTag("CreateAnchor").GetComponent<DevCreateAnchors>();
        devShowInformation = GameObject.FindGameObjectWithTag("Button").GetComponent<DevShowInformation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        var all = FindObjectsOfType<GameObject>();

        foreach (GameObject interaction in interactions)
        {
            interaction.SetActive(false);
        }

        foreach (GameObject item in all)
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
        float dist = Vector3.Distance(gameObject.transform.position, arCamera.transform.position);

        if (dist < minDist)
        {
            foreach (GameObject interaction in interactions)
            {
                interaction.SetActive(true);
            }
        }
        else if (dist > minDist)
        {
            foreach (GameObject interaction in interactions)
            {
                interaction.SetActive(false);
            }

            devUiController.ShowScanning();

            //devUiController.ScanningDone = false;
            devUiController.HideBasicInformation();
            devUiController.HideSpecificInformation();

            positionCalculator.ScanningDone = false;

            devCreateAnchors.EnableTrackedImageManager();

            devShowInformation.hideGlow();
        }
    }
}
