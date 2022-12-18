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

    // Last point positions
    private Vector3 lastPositionMobile;
    private Vector3 lastPositionImageTrack;

    // Reference DevLogger script
    private DevLogger devLogger;

    // TEST prefab
    public GameObject TestPrefab1;
    public GameObject TestPrefab2;
    public GameObject TestPrefab3;

    private Transform emptyAnchorTransform;

    private GameObject AnchorInstance;
    private GameObject TestPrefab1Instance;
    private GameObject TestPrefab2Instance;
    private GameObject TestPrefab3Instance;

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


        emptyAnchorTransform = new GameObject().transform;

        // place anchor and interactions at world-origin 
        AnchorInstance = Instantiate(Anchor, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)); 
        TestPrefab1Instance = Instantiate(TestPrefab1, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        TestPrefab2Instance = Instantiate(TestPrefab2, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        TestPrefab3Instance = Instantiate(TestPrefab3, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));      

        AnchorInstance.SetActive(false);     
        TestPrefab1Instance.SetActive(false);
        TestPrefab2Instance.SetActive(false);
        TestPrefab3Instance.SetActive(false);   
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

        emptyAnchorTransform.rotation = trackedImageTransform.rotation;

        if (anchorPlaced == false)   
        {
            switch (trackedImageName)
            {
                case "DevMarker1":
                    emptyAnchorTransform.position = trackedImageTransform.position + 0 * trackedImageTransform.right + 0 * trackedImageTransform.up + (float)0.5 * trackedImageTransform.forward;    

                    // update transform of AnchorInstance
                    AnchorInstance.transform.SetPositionAndRotation(emptyAnchorTransform.position, emptyAnchorTransform.rotation);
                    AnchorInstance.SetActive(true);     

                    devLogger.printLogMessage("DevMarker1"); 

                    anchorPlaced = true;
                    break;

                case "DevMarker2":
                    emptyAnchorTransform.position = trackedImageTransform.position - (float)0.5 * trackedImageTransform.right + 0 * trackedImageTransform.up + (float)0.5 * trackedImageTransform.forward;

                    // update transform of AnchorInstance
                    AnchorInstance.transform.SetPositionAndRotation(emptyAnchorTransform.position, emptyAnchorTransform.rotation);
                    AnchorInstance.SetActive(true);      

                    devLogger.printLogMessage("DevMarker2");  

                    anchorPlaced = true;
                    break;

                case "DevMarker3":
                    emptyAnchorTransform.position = trackedImageTransform.position - (float)0.5 * trackedImageTransform.right + 0 * trackedImageTransform.up + 0 * trackedImageTransform.forward;
                    
                    // update transform of AnchorInstance
                    AnchorInstance.transform.SetPositionAndRotation(emptyAnchorTransform.position, emptyAnchorTransform.rotation);
                    AnchorInstance.SetActive(true);   

                    devLogger.printLogMessage("DevMarker3");        

                    anchorPlaced = true;
                    break;
            }            
        }


        if (anchorPlaced)
        {
            Transform TestPrefab1Transform = new GameObject().transform;
            Transform TestPrefab2Transform = new GameObject().transform;
            Transform TestPrefab3Transform = new GameObject().transform;

            TestPrefab1Transform.rotation = emptyAnchorTransform.rotation;
            TestPrefab2Transform.rotation = emptyAnchorTransform.rotation;
            TestPrefab3Transform.rotation = emptyAnchorTransform.rotation;

            TestPrefab1Transform.position = emptyAnchorTransform.position + 0 * trackedImageTransform.right + 0 * trackedImageTransform.up - (float)0.5 * trackedImageTransform.forward; 
            TestPrefab2Transform.position = emptyAnchorTransform.position + (float)0.5 * trackedImageTransform.right + 0 * trackedImageTransform.up - (float)0.5 * trackedImageTransform.forward; 
            TestPrefab3Transform.position = emptyAnchorTransform.position + (float)0.5 * trackedImageTransform.right + 0 * trackedImageTransform.up + 0 * trackedImageTransform.forward; 

            TestPrefab1Instance.transform.SetPositionAndRotation(TestPrefab1Transform.position, TestPrefab1Transform.rotation);
            TestPrefab2Instance.transform.SetPositionAndRotation(TestPrefab2Transform.position, TestPrefab2Transform.rotation);
            TestPrefab3Instance.transform.SetPositionAndRotation(TestPrefab3Transform.position, TestPrefab3Transform.rotation);

            TestPrefab1Instance.SetActive(true);
            TestPrefab2Instance.SetActive(true);
            TestPrefab3Instance.SetActive(true);

            anchorPlaced = false;
        }
    }
}
