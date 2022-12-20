using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DevPositionCalculator : MonoBehaviour
{
    // Array of strings containing the names of gameObjects whose position need to be tracked
    public string[] objectsToBeTracked;

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
    public GameObject ArtPiecePrefab3;

    private Transform AnchorPoint;

    private GameObject AnchorInstance;
    private GameObject ArtPiece1;
    private GameObject ArtPiece2;
    private GameObject ArtPiece3;


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
        ArtPiece1 = Instantiate(ArtPiecePrefab1, new Vector3(0, -10, 0), new Quaternion(0, 0, 0, 0));
        ArtPiece2 = Instantiate(ArtPiecePrefab2, new Vector3(0, -10, 0), new Quaternion(0, 0, 0, 0));
        ArtPiece3 = Instantiate(ArtPiecePrefab3, new Vector3(0, -10, 0), new Quaternion(0, 0, 0, 0));      

        // Make interactions inactive
        ArtPiece1.SetActive(false);
        ArtPiece2.SetActive(false);
        ArtPiece3.SetActive(false);

        //devLogger.printLogMessage("item: World Origin - pos: (0.00, 0.00, 0.00)");
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

                // Call printLogMessage from 'DevLogger'
                //devLogger.printLogMessage("item: Mobile - pos: " + transformMobile.transform.position + " - rot: " + transformMobile.transform.rotation);
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

                // Call printLogMessage from 'DevLogger'
                //devLogger.printLogMessage("item: ImageTrack - pos: " + transformImageTrack.transform.position + " - rot: " + transformImageTrack.transform.rotation);
            }
        }
    }

    private void updatePositionMobile(Transform transform)
    {
        transformMobile = transform;
    }

    public void updatePositionImageTrack(Transform trackedImageTransform, string trackedImageName)
    {
        devUiController.ImageTracked = true;

        if (trackedImageTransform.position != new Vector3(0, 0, 0))
        {
            // REMOVE when logs are no longer needed
            transformImageTrack = transform;

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
                        // REMOVE when logs are no longer needed
                        transformImageTrack = temp.transform;

                        calculatePositions(trackedImageTransform, trackedImageName);
                    }
                }
            }
        }
    }

    private void calculatePositions(Transform trackedImageTransform, string trackedImageName)
    {
        bool anchorPlaced = false;

        // devLogger.printLogMessage("ImagePos: " + trackedImageTransform.position + " NewPos: " + emptyAnchorTransform.position); 

        AnchorPoint.rotation = trackedImageTransform.rotation;

        if (anchorPlaced == false)   
        {
            switch (trackedImageName)
            {
                case "DevArtPiece1":
                    AnchorPoint.position = trackedImageTransform.position + 0 * trackedImageTransform.right + 0 * trackedImageTransform.up + (float)0.5 * trackedImageTransform.forward;

                    // Update transform of AnchorPoint
                    AnchorPoint.SetPositionAndRotation(AnchorPoint.position, AnchorPoint.rotation);

                    //AnchorInstance.transform.SetPositionAndRotation(emptyAnchorTransform.position, emptyAnchorTransform.rotation);
                    //AnchorInstance.SetActive(true);     

                    anchorPlaced = true;
                    break;

                case "DevArtPiece2":
                    AnchorPoint.position = trackedImageTransform.position - (float)0.5 * trackedImageTransform.right + 0 * trackedImageTransform.up + (float)0.5 * trackedImageTransform.forward;

                    // Update transform of AnchorPoint
                    AnchorPoint.SetPositionAndRotation(AnchorPoint.position, AnchorPoint.rotation);     

                    anchorPlaced = true;
                    break;

                case "DevArtPiece3":
                    AnchorPoint.position = trackedImageTransform.position - (float)0.5 * trackedImageTransform.right + 0 * trackedImageTransform.up + 0 * trackedImageTransform.forward;

                    // Update transform of AnchorPoint
                    AnchorPoint.SetPositionAndRotation(AnchorPoint.position, AnchorPoint.rotation);
    
                    anchorPlaced = true;
                    break;
            }            
        }

        if (anchorPlaced)
        {
            Transform ArtPiecePrefabTransform1 = new GameObject().transform;
            Transform ArtPiecePrefabTransform2 = new GameObject().transform;
            Transform ArtPiecePrefabTransform3 = new GameObject().transform;

            ArtPiecePrefabTransform1.rotation = AnchorPoint.rotation;
            ArtPiecePrefabTransform2.rotation = AnchorPoint.rotation;
            ArtPiecePrefabTransform3.rotation = AnchorPoint.rotation;

            ArtPiecePrefabTransform1.position = AnchorPoint.position + 0 * trackedImageTransform.right + 0 * trackedImageTransform.up - (float)0.5 * trackedImageTransform.forward; 
            ArtPiecePrefabTransform2.position = AnchorPoint.position + (float)0.5 * trackedImageTransform.right + 0 * trackedImageTransform.up - (float)0.5 * trackedImageTransform.forward; 
            ArtPiecePrefabTransform3.position = AnchorPoint.position + (float)0.5 * trackedImageTransform.right + 0 * trackedImageTransform.up + 0 * trackedImageTransform.forward; 

            ArtPiece1.transform.SetPositionAndRotation(ArtPiecePrefabTransform1.position, ArtPiecePrefabTransform1.rotation);
            ArtPiece2.transform.SetPositionAndRotation(ArtPiecePrefabTransform2.position, ArtPiecePrefabTransform2.rotation);
            ArtPiece3.transform.SetPositionAndRotation(ArtPiecePrefabTransform3.position, ArtPiecePrefabTransform3.rotation);

            ArtPiece1.SetActive(true);
            ArtPiece2.SetActive(true);
            ArtPiece3.SetActive(true);

            anchorPlaced = false;
        }
    }
}
