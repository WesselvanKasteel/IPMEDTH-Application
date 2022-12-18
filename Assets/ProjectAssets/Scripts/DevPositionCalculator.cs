using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DevPositionCalculator : MonoBehaviour
{
    // Array of strings containing the names of gameObjects whose position need to be tracked
    private string[] objectsToBeTracked = { "DevMarker1", "DevMarker2", "DevMarker3" };

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
    //private Transform anchorpointInteractions;

    // Last point positions
    private Vector3 lastPositionMobile;
    private Vector3 lastPositionImageTrack;

    // Reference DevLogger script
    private DevLogger devLogger;

    // TEST prefab
    public GameObject TestPrefab1;
    public GameObject TestPrefab2;

    private Transform emptyAnchor;


    // Start is called before the first frame update
    void Start()
    {
        // Get component 'DevLogger'
        devLogger = GameObject.FindGameObjectWithTag("Logs").GetComponent<DevLogger>();

        // Update lastPositionMobile
        lastPositionMobile = ArCamera.transform.position;

        // Update lastPositionImageTrack
        lastPositionImageTrack = new Vector3(0, 0, 0);

        devLogger.printLogMessage("item: World Origin - pos: (0.00, 0.00, 0.00)");

        // anchorpointInteractions = new GameObject().transform;

        emptyAnchor = new GameObject().transform;
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
                devLogger.printLogMessage("item: Mobile - pos: " + transformMobile.transform.position + " - rot: " + transformMobile.transform.rotation);
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
                devLogger.printLogMessage("item: ImageTrack - pos: " + transformImageTrack.transform.position + " - rot: " + transformImageTrack.transform.rotation);
            }
        }
    }

    private void updatePositionMobile(Transform transform)
    {
        transformMobile = transform;
    }

    public void updatePositionImageTrack(Transform trackedImageTransform, string trackedImageName)
    {
        //nameImageTrack = name;

        Instantiate(TestPrefab1, trackedImageTransform);

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
        // devLogger.printLogMessage("name: " + trackedImageName + " transform: " + trackedImageTransform.position);

        // bool anchorPlaced = false;

        // image(1, 1, 1)
        Vector3 trackedImagePosition = trackedImageTransform.position;

        // // new pos (1, 1, 2)
        // Vector3 newPosition = new Vector3(trackedImageTransform.position.x, trackedImageTransform.position.y, trackedImageTransform.localPosition.z);

        emptyAnchor.rotation = trackedImageTransform.rotation;

        emptyAnchor.position = trackedImageTransform.position + 0 * trackedImageTransform.right;
        emptyAnchor.position = trackedImageTransform.position + 0 * trackedImageTransform.up;
        emptyAnchor.position = trackedImageTransform.position + (float)0.5 * trackedImageTransform.forward; 

        Instantiate(TestPrefab2, emptyAnchor);

        devLogger.printLogMessage("ImagePos: " + trackedImagePosition + " NewPos: " + emptyAnchor.transform.position); 

        // switch (trackedImageName)
        // {
        //     case "DevMarker1":
        //         //devLogger.printLogMessage("name: DevMarker1");
        //         break;

        //     case "DevMarker2":
        //         //devLogger.printLogMessage("name: DevMarker2");
        //         break;

        //     case "DevMarker3":
        //         //devLogger.printLogMessage("name: DevMarker3");
        //         break;
        // }

        // if (anchorPlaced)
        // {

        //     foreach (GameObject interaction in Interactions)
        //     {
        //         switch (interaction.name)
        //         {
        //             case "DevMarker1":
        //                 break;
        //             case "DevMarker2":
        //                 break;
        //             case "DevMarker3":
        //                 break;
        //         }
        //     }
        // }
    }
}
