using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class DevCreateAnchors : MonoBehaviour
{
    // Reference to AR tracked image manager component
    private ARTrackedImageManager _trackedImagesManager;

    // Keep dictionary array of created prefabs
    private readonly Dictionary<string, GameObject> _instantiatedPrefabs = new Dictionary<string, GameObject>();

    // Transform variable of active prefab
    private Transform prefabLocation;

    // Reference DevPositionCalculator script
    private DevPositionCalculator positionCalculator;

    // List of prefabs to instantiate - these should be named the same
    // as their corresponding 2D images in the reference image library 
    public GameObject[] ArPrefabs;

    // Reference DevLogger script
    private DevLogger devLogger;


    void Awake()
    {
        // Cache a reference to the Tracked Image Manager component
        _trackedImagesManager = GetComponent<ARTrackedImageManager>();
    }

    private void Start()
    {
        // Get component DevPositionCalculator
        positionCalculator = GameObject.FindGameObjectWithTag("PositionCalculator").GetComponent<DevPositionCalculator>();
        // Get component 'DevLogger'
        devLogger = GameObject.FindGameObjectWithTag("Logs").GetComponent<DevLogger>();
    }

    void OnEnable()
    {
        // Attach event handler when tracked images change
        _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        // Remove event handler
        _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    IEnumerator TrackedImageCoroutine(ARTrackedImage trackedImage)
    {
        // Get the name of the reference image
        var imageName = trackedImage.referenceImage.name;

        yield return null;

        // Now loop over the array of prefabs
        foreach (var curPrefab in ArPrefabs)
        {
            // Check whether this prefab matches the tracked image name, and that
            if (string.Compare(curPrefab.name, imageName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Call updatePositionImageTrack on DevPositionCalculator
                positionCalculator.updatePositionImageTrack(trackedImage.transform, trackedImage.referenceImage.name);
            }
        }
    }

    // Event Handler
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                //trackedImage is tracked
                StartCoroutine(TrackedImageCoroutine(trackedImage));
            }
            else
            {
                //trackedImage is lost
                devLogger.printLogMessage("Tracking disabled");
            }
        }
    }
}