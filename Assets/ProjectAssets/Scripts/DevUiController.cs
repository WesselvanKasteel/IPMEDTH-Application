using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DevUiController : MonoBehaviour
{
    private string ActiveInteractionName;

    // Public variables
    public bool ScanningDone = false;
    public bool SpecificInformationActive = false;

    // UI elements
    [SerializeField] GameObject fase1ScanIcon;

    // UI groups
    [SerializeField] GameObject fase1Scanning;
    [SerializeField] GameObject fase2BasicInformation;
    [SerializeField] GameObject fase3SpecificInformation;
    [SerializeField] GameObject fase4End;

    // Basic information
    private TextMeshProUGUI basicInformationTitleText;
    private TextMeshProUGUI basicInformationLocationText;
    private TextMeshProUGUI basicInformationTimePeriodText;

    public GameObject basicInformationTitle;
    public GameObject basicInformationLocation;
    public GameObject basicInformationTimePeriod;

    // Specific information
    [SerializeField] GameObject[] specificInteractions;

    [SerializeField] GameObject[] nextButtons;
    [SerializeField] GameObject[] backButtons;

    //private List<int> _historyArray = new List<int>();

    public int lastInteraction;


    // Reference DevLogger script
    private DevLogger devLogger;
    private DevCreateAnchors devCreateAnchors;
    private DevShowInformation devShowInformation;

    private void Awake()
    {
        // Get script-component from GameObject
        devLogger = GameObject.FindGameObjectWithTag("Logs").GetComponent<DevLogger>();
        devCreateAnchors = GameObject.FindGameObjectWithTag("CreateAnchor").GetComponent<DevCreateAnchors>();

        // Basic information
        basicInformationTitleText = basicInformationTitle.GetComponent<TextMeshProUGUI>();
        basicInformationLocationText = basicInformationLocation.GetComponent<TextMeshProUGUI>();
        basicInformationTimePeriodText = basicInformationTimePeriod.GetComponent<TextMeshProUGUI>();
    }


    // Start is called before the first frame update
    void Start()
    {
        fase1ScanIcon.SetActive(true);
        fase1Scanning.SetActive(true); 

        // Basic information
        fase2BasicInformation.SetActive(false);

        // Specific informatuin
        fase3SpecificInformation.SetActive(false);

        foreach(GameObject interaction in specificInteractions)
        {
            interaction.SetActive(false);
        }

        foreach (GameObject button in backButtons)
        {
            button.SetActive(false);
        }
        foreach (GameObject button in nextButtons)
        {
            button.SetActive(false);
        }

        fase4End.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Fase 1 Scanning
    public void ShowScanning()
    {
        fase1ScanIcon.SetActive(true);
        fase1Scanning.GetComponent<DevUiFade>().ShowUI();
    }

    public void HideScanning()
    {
        fase1ScanIcon.SetActive(false);
        fase1Scanning.GetComponent<DevUiFade>().HideUI();
    }

    // Fase 2 Basic Information

    public void ShowBasicInformation()
    {
        //devLogger.printLogMessage("ShowBasicInformation called" + ScanningDone);

        if (!ScanningDone)
        {
            fase2BasicInformation.SetActive(true);
            fase2BasicInformation.GetComponent<DevUiFade>().ShowUI();

            //devCreateAnchors.EnableTrackedImageManager();

            ScanningDone = true;
        }

    }

    public void HideBasicInformation()
    {
        //devLogger.printLogMessage("HideBasicInformation called");

        fase2BasicInformation.GetComponent<DevUiFade>().HideUI();
        ScanningDone = false;
    }

    public void DisplayBasicInformation(string trackedImageName)
    {
        try
        {
            switch (trackedImageName)
            {
                case "DevArtPiece1":
                    basicInformationTitleText.text = "Stèle van Sehetepibreanch";
                    basicInformationLocationText.text = "Egypte, Abydos";
                    basicInformationTimePeriodText.text = "12e dynastie; Amenemhat III 1853-1806 v.Chr.";
                    break;
                case "DevArtPiece2":
                    basicInformationTitleText.text = "2";
                    basicInformationLocationText.text = "2";
                    basicInformationTimePeriodText.text = "2";
                    break;
            }
        }
        catch (Exception e)
        {
            devLogger.printLogMessage("EnableBasicInformation ERROR: " + e);
        }

    }

    //// Fase 3 Specific Information (parent)

    public void ShowSpecificInformation()
    {
        //devLogger.printLogMessage("ShowSpecificInformation called");

        if (!SpecificInformationActive)
        {
            fase3SpecificInformation.SetActive(true);
            fase3SpecificInformation.GetComponent<DevUiFade>().ShowUI();

            devShowInformation = GameObject.FindGameObjectWithTag("Button").GetComponent<DevShowInformation>();

            SpecificInformationActive = true;
        }
    }

    public void HideSpecificInformation()
    {
        //devLogger.printLogMessage("HideSpecificInformation called");

        fase3SpecificInformation.GetComponent<DevUiFade>().HideUI();
        SpecificInformationActive = false;
    }

    // Fase 3 Specific Information (child)

    public void ShowSpecificInteraction(int number)
    {
        //devLogger.printLogMessage("ShowSpecificInteraction called | bool: " + SpecificInformationActive);

        lastInteraction = number - 1;

        EnableOrDisableButtons(number);

        if (SpecificInformationActive)
        {
            foreach (GameObject interaction in specificInteractions)
            {
                int interactionNumber = (int)Variables.Object(interaction).Get("InteractionNumber");

                if (interactionNumber == number)
                {
                    interaction.SetActive(true);

                    devShowInformation.ShowGlow(interactionNumber);
                }
                else
                {
                    interaction.SetActive(false);
                }
            }
        }
    }

    public void goToLastInteraction()
    {
        ShowSpecificInteraction(lastInteraction);
    }

    private void EnableOrDisableButtons(int number)
    {

        // Disable or enable back buttons
        if (number == 1)
        {
            foreach (GameObject button in backButtons)
            {
                button.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject button in backButtons)
            {
                button.SetActive(true);
            }
        }

        // Disable or enable next buttons
        if (number == 4)
        {
            foreach (GameObject button in nextButtons)
            {
                button.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject button in nextButtons)
            {
                button.SetActive(true);
            }
        }
    }

    // Fase 4 End
    public void EnableEndUI()
    {
        fase4End.SetActive(true);
    }

    public void CloseApplication() {
        Application.Quit();
    }


}
