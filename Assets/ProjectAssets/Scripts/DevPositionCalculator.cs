using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DevPositionCalculator : MonoBehaviour
{
    // Array of strings containing the names of gameObjects whose position need to be tracked
    private string[] objectsToBeTracked = { "DevMarker" };

    // GameObjects
    public GameObject ArCamera;
    // Minimum Mobile displacement to recognize 
    public float thresholdMobile = 0.5f;
    // Minimum ImageTrack displacement to recognize 
    public float thresholdImageTrack = 0.25f;

    // Points
    private Vector3 positionWorldOrigin = new Vector3(0, 0, 0);
    private Transform transformMobile;
    private Transform transformImageTrack;

    // Last point positions
    private Vector3 lastPositionMobile;
    private Vector3 lastPositionImageTrack;

    // Reference DevLogger script
    private DevLogger devLogger;


    // Start is called before the first frame update
    void Start()
    {
        //Invoke("FindAll", 2);

        // Get component 'DevLogger'
        devLogger = GameObject.FindGameObjectWithTag("Logs").GetComponent<DevLogger>();
        // Call printLogMessage from 'DevLogger'
        devLogger.printLogMessage("item: WorldOrigin - position: " + positionWorldOrigin);

        // Update lastPositionMobile
        lastPositionMobile = ArCamera.transform.position;
        // Update lastPositionImageTrack
        lastPositionImageTrack = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        // Call updatePositionMobile 
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
                devLogger.printLogMessage("item: Mobile - position: " + transformMobile.transform.position + " - rotation: " + transformMobile.transform.rotation);
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
                devLogger.printLogMessage("item: ImageTrack - position: " + transformImageTrack.transform.position + " - rotation: " + transformImageTrack.transform.rotation);
            }
        }
    }

    private void FindAll()
    {
        Object[] tempList = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        List<GameObject> realList = new List<GameObject>();
        GameObject temp;

        foreach (Object obj in tempList)
        {
            if (obj is GameObject)
            {
                temp = (GameObject)obj;
                if (temp.hideFlags == HideFlags.None && objectsToBeTracked.Contains(temp.name) && temp.transform.position != new Vector3(0,0,0)) 
                {
                    realList.Add((GameObject)obj);

                    // Call printLogMessage from 'DevLogger'
                    devLogger.printLogMessage("item: " + temp.name + " - position: " + temp.transform.position);
                }
            }
        }

        Selection.objects = realList.ToArray();
    }

    private void updatePositionMobile(Transform transform)
    {
        transformMobile = transform;;
    }

    public void updatePositionImageTrack(Transform transform)
    {
        transformImageTrack = transform;
    }
}