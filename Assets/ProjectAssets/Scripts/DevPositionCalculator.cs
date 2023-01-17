using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DevPositionCalculator : MonoBehaviour
{
    // Array of strings containing the names of gameObjects whose position need to be tracked
    public string[] objectsToBeTracked;

    // Public variables
    public bool ScanningDone = false;

    // GameObjects
    public GameObject ArCamera;
    public GameObject Anchor;
    public GameObject[] Interactions;

    // Thresholds
    public float thresholdMobile;
    public float thresholdImageTrack;

    // Points
    private Vector3 positionWorldOrigin = new Vector3(0, 0, 0);
    private Transform transformMobile;

    // REMOVE when logs are no longer needed
    private Transform transformImageTrack;
    // -------------------------------------

    // Last point positions
    private Vector3 lastPositionMobile;
    private Vector3 lastPositionImageTrack;

    // Reference to scripts
    private DevLogger devLogger;
    private DevUiController devUiController;

    // TEST prefab
    public GameObject ArtPiecePrefab1;
    public GameObject ArtPiecePrefab2;

    private Transform AnchorPoint;

    private GameObject AnchorInstance;
    private GameObject ArtPiece1;
    private GameObject ArtPiece2;

    public GameObject DestinationArrow;
    private GameObject Arrow;


    private void Awake()
    {
        // Get script-component from GameObject
        devLogger = GameObject.FindGameObjectWithTag("Logs").GetComponent<DevLogger>();
        devUiController = GameObject.FindGameObjectWithTag("UiController").GetComponent<DevUiController>();
    }

    void Start()
    {
        // Update lastPositionMobile
        lastPositionMobile = ArCamera.transform.position;

        // Update lastPositionImageTrack
        lastPositionImageTrack = new Vector3(0, 0, 0);

        AnchorPoint = new GameObject().transform;

        // Place interactions at in scene
        ArtPiece1 = Instantiate(ArtPiecePrefab1, new Vector3(0, -5, 0), new Quaternion(0, 0, 0, 0));
        ArtPiece2 = Instantiate(ArtPiecePrefab2, new Vector3(0, -5, 1), new Quaternion(0, 0, 0, 0));    

        // Make interactions inactive
        ArtPiece1.SetActive(true);
        ArtPiece2.SetActive(false);

        // DEMO -----
        Arrow = Instantiate(DestinationArrow, new Vector3(0, (float)-1.5, 0), new Quaternion(0, 0, 0, 0));
        Arrow.SetActive(false);
        // DEMO -----
    }

    void Update()
    {
        updatePositionMobile(ArCamera.transform);

        if (transformMobile != null)
        {
            // Check if Mobile position changed 
            Vector3 offsetMobile = transformMobile.transform.position - lastPositionMobile;

            if (offsetMobile.x > thresholdMobile || offsetMobile.x < -thresholdMobile ||
                offsetMobile.y > thresholdMobile || offsetMobile.y < -thresholdMobile ||
                offsetMobile.z > thresholdMobile || offsetMobile.z < -thresholdMobile)
            {
                // Update lastPositionMobile
                lastPositionMobile = transformMobile.transform.position;
            }
        }

        if (transformImageTrack != null)
        {
            // Check if ImageTrack position changed 
            Vector3 offsetImageTrack = transformImageTrack.transform.position - lastPositionImageTrack;

            if (offsetImageTrack.x > thresholdImageTrack || offsetImageTrack.x < -thresholdImageTrack ||
                offsetImageTrack.y > thresholdImageTrack || offsetImageTrack.y < -thresholdImageTrack ||
                offsetImageTrack.z > thresholdImageTrack || offsetImageTrack.z < -thresholdImageTrack)
            {
                // Update lastPositionMobile
                lastPositionImageTrack = transformImageTrack.transform.position;
            }
        }
    }

    private void updatePositionMobile(Transform transform)
    {
        transformMobile = transform;
    }

    public void updatePositionImageTrack(Transform trackedImageTransform, string trackedImageName)
    {
        if (!ScanningDone && trackedImageTransform.position != new Vector3(0, 0, 0))
        {
            //devLogger.printLogMessage("Scanned");

            // update UI here
            devUiController.HideScanning();
            devUiController.DisplayBasicInformation(trackedImageName);
            devUiController.ShowBasicInformation();

            if (trackedImageTransform.position != new Vector3(0, 0, 0))
            {
                calculatePositions(trackedImageTransform, trackedImageName);
            }
            else
            {
                Object[] tempList = Resources.FindObjectsOfTypeAll(typeof(GameObject));
                GameObject temp;

                foreach (Object obj in tempList)
                {
                    if (obj is GameObject)
                    {
                        temp = (GameObject)obj;

                        if (temp.hideFlags == HideFlags.None && objectsToBeTracked.Contains(temp.name) && temp.transform.position != new Vector3(0, 0, 0))
                        {
                            calculatePositions(trackedImageTransform, trackedImageName);
                        }
                    }
                }
            }
            ScanningDone = true;
        }

    }

    private void calculatePositions(Transform trackedImageTransform, string trackedImageName)
    {
        bool anchorPlaced = false;

        AnchorPoint.rotation = trackedImageTransform.rotation;

        //devLogger.printLogMessage("calculatePositions | name: " + trackedImageName + " pos: " + trackedImageTransform.position);

        if (anchorPlaced == false)   
        {
            switch (trackedImageName)
            {
                case "DevArtPiece1":
                    AnchorPoint.position = trackedImageTransform.position + 0 * trackedImageTransform.right + 2 * trackedImageTransform.up + 0 * trackedImageTransform.forward;

                    // Update transform of AnchorPoint
                    AnchorPoint.SetPositionAndRotation(AnchorPoint.position, AnchorPoint.rotation);

                    // DEMO -----
                    Vector3 arrowTransform = new Vector3(AnchorPoint.position.x, AnchorPoint.position.y - 1, AnchorPoint.position.z);
                    Arrow.transform.SetPositionAndRotation(arrowTransform, AnchorPoint.rotation);
                    Arrow.SetActive(true);
                    // DEMO -----

                    anchorPlaced = true;
                    break;

                case "DevArtPiece2":
                    AnchorPoint.position = trackedImageTransform.position + (float)1.5 * trackedImageTransform.right + 0 * trackedImageTransform.up + 0 * trackedImageTransform.forward;

                    // Update transform of AnchorPoint
                    AnchorPoint.SetPositionAndRotation(AnchorPoint.position, AnchorPoint.rotation);

                    Arrow.SetActive(false);

                    // DEMO -----
                    devUiController.EnableEndUI();
                    // DEMO -----

                    anchorPlaced = true;
                    break;
            }            
        }

        if (anchorPlaced)
        {
            Transform ArtPiecePrefabTransform1 = new GameObject().transform;
            Transform ArtPiecePrefabTransform2 = new GameObject().transform;

            ArtPiecePrefabTransform1.rotation = AnchorPoint.rotation;
            ArtPiecePrefabTransform2.rotation = AnchorPoint.rotation;

            //ArtPiecePrefabTransform1.rotation = Quaternion.Euler(AnchorPoint.rotation.x, AnchorPoint.rotation.y, AnchorPoint.rotation.z);
            //ArtPiecePrefabTransform2.rotation = Quaternion.Euler(AnchorPoint.rotation.x, AnchorPoint.rotation.y, AnchorPoint.rotation.z);

            ArtPiecePrefabTransform1.position = AnchorPoint.position + 0 * trackedImageTransform.right - 2 * trackedImageTransform.up + 0 * trackedImageTransform.forward; 
            ArtPiecePrefabTransform2.position = AnchorPoint.position - (float)1.5 * trackedImageTransform.right + 0 * trackedImageTransform.up - 0 * trackedImageTransform.forward; 

            //devLogger.printLogMessage("Art1 | pos: " + ArtPiecePrefabTransform1.position + " rot: " + ArtPiecePrefabTransform1.rotation);
            //devLogger.printLogMessage("Art2 | pos: " + ArtPiecePrefabTransform2.position + " rot: " + ArtPiecePrefabTransform2.rotation);

            ArtPiece1.transform.SetPositionAndRotation(ArtPiecePrefabTransform1.position, ArtPiecePrefabTransform1.rotation);
            ArtPiece2.transform.SetPositionAndRotation(ArtPiecePrefabTransform2.position, ArtPiecePrefabTransform2.rotation);

            ArtPiece1.SetActive(true);
            ArtPiece2.SetActive(true);

            anchorPlaced = false;
        }
    }
}
