using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using UnityEngine.UI;
using TMPro;

public class ImageTracker : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject[] ArPrefabs;

    // We only care about the CURRENTLY displayed AR object
    private GameObject currentActiveArObject = null;
    private string currentActiveMarkerName = ""; // To keep track of which marker's model is active

    public TMP_Text infoBox;
    public InfoScreenManager infoScreenManager; // Assigned in Inspector

    void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
        if (infoScreenManager == null)
        {
            infoScreenManager = FindObjectOfType<InfoScreenManager>();
            if (infoScreenManager == null)
            {
                Debug.LogError("ImageTracker: No InfoScreenManager found in the scene! Please assign it or ensure one exists.");
            }
        }
    }

    void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        infoBox.text = ""; // Clear the infoBox for fresh updates

        // --- Handle newly added or updated images for model display ---
        // Prioritize "added" images for showing models immediately
        foreach (var trackedImage in eventArgs.added)
        {
            HandleImageDetected(trackedImage);
            infoBox.text += "Added: " + trackedImage.referenceImage.name + " " + trackedImage.trackingState + "\n";
        }

        // Then handle "updated" images, but only if they are being tracked
        foreach (var trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                HandleImageDetected(trackedImage);
            }
            else // If updated but tracking is limited or none, hide the model and info
            {
                HandleImageLostOrLimited(trackedImage);
            }
            infoBox.text += "Updated: " + trackedImage.referenceImage.name + " " + trackedImage.trackingState + "\n";
        }

        // --- Handle removed images ---
        foreach (var trackedImage in eventArgs.removed)
        {
            HandleImageLostOrLimited(trackedImage);
            infoBox.text += "Removed: " + trackedImage.referenceImage.name + "\n";
        }
    }

    // --- Simplified Helper Functions ---

    // This function is called when an image is actively being tracked
    private void HandleImageDetected(ARTrackedImage trackedImage)
    {
        // Only change if a DIFFERENT image is now being tracked
        if (currentActiveMarkerName != trackedImage.referenceImage.name)
        {
            // Destroy the previous active model if it exists
            if (currentActiveArObject != null)
            {
                Destroy(currentActiveArObject);
                currentActiveArObject = null;
                Debug.Log($"ImageTracker: Destroyed previous AR model for {currentActiveMarkerName}.");
            }

            // Find the correct prefab and instantiate it
            GameObject prefabToInstantiate = null;
            foreach (var arPrefab in ArPrefabs)
            {
                if (trackedImage.referenceImage.name == arPrefab.name)
                {
                    prefabToInstantiate = arPrefab;
                    break; // Found it, stop searching
                }
            }

            if (prefabToInstantiate != null)
            {
                // Instantiate the new model as a child of the tracked image
                currentActiveArObject = Instantiate(prefabToInstantiate, trackedImage.transform);
                currentActiveArObject.name = prefabToInstantiate.name + "_Instance"; // For debugging
                currentActiveMarkerName = trackedImage.referenceImage.name; // Remember which model is active
                Debug.Log($"ImageTracker: Instantiated new AR model for {currentActiveMarkerName}.");
            }
            else
            {
                Debug.LogWarning($"ImageTracker: No AR Prefab found for reference image: {trackedImage.referenceImage.name}");
            }
        }
        else // If the same image is still being tracked, just ensure the model is active
        {
            if (currentActiveArObject != null && !currentActiveArObject.activeSelf)
            {
                currentActiveArObject.SetActive(true);
                Debug.Log($"ImageTracker: Re-activated AR model for {currentActiveMarkerName}.");
            }
        }

        // Always update the InfoScreenManager for the currently tracked image
        if (infoScreenManager != null)
        {
            infoScreenManager.DisplayInfoForMarker(trackedImage.referenceImage.name);
            infoScreenManager.ShowInfoButton();
        }
    }

    // This function is called when an image loses tracking or is removed
    private void HandleImageLostOrLimited(ARTrackedImage trackedImage)
    {
        // If the lost/limited image is the one currently displaying a model
        if (currentActiveMarkerName == trackedImage.referenceImage.name)
        {
            if (currentActiveArObject != null)
            {
                // Deactivate the model instead of destroying, in case it regains tracking quickly
                currentActiveArObject.SetActive(false);
                Debug.Log($"ImageTracker: Deactivated AR model for {currentActiveMarkerName}.");
            }

            // Hide the info button and panel
            if (infoScreenManager != null)
            {
                infoScreenManager.HideInfoButton();
            }
        }
    }
}