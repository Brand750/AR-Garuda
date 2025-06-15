using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InfoScreenManager : MonoBehaviour
{
    public GameObject infoPanel;       // Assign in Inspector: The panel that shows the detailed info
    public Button infoButton;          // Assign in Inspector: The button that shows up to open the panel
    public TextMeshProUGUI infoText;   // Assign in Inspector: The TextMeshProUGUI element inside the infoPanel

    [SerializeField]
    private List<MarkerInfo> markerInfos = new List<MarkerInfo>(); // Populate this in Inspector

    [System.Serializable]
    public class MarkerInfo
    {
        public string markerName; // CRITICAL: Must match ReferenceImageLibrary names (e.g., "Marker1")
        [TextArea(5, 10)]
        public string content;
    }

    private Dictionary<string, string> markerContentDictionary = new Dictionary<string, string>();
    private bool isInfoPanelActive = false;
    private string currentMarkerNameForInfo = ""; // Renamed for clarity, stores name for current info content

    void Awake()
    {
        // Build the dictionary from the serialized list for quick lookups
        foreach (MarkerInfo info in markerInfos)
        {
            if (!markerContentDictionary.ContainsKey(info.markerName))
            {
                markerContentDictionary.Add(info.markerName, info.content);
            }
            else
            {
                Debug.LogWarning($"InfoScreenManager: Duplicate marker name '{info.markerName}' found in Marker Infos. Only the first entry will be used.");
            }
        }

        if (infoPanel == null) Debug.LogError("InfoScreenManager: Info Panel is not assigned!");
        if (infoButton == null) Debug.LogError("InfoScreenManager: Info Button is not assigned!");
        if (infoText == null) Debug.LogError("InfoScreenManager: Info Text is not assigned!");
    }

    void Start()
    {
        // Initialize UI state
        if (infoPanel != null)
        {
            infoPanel.SetActive(false); // Start with the panel hidden
        }
        isInfoPanelActive = false;

        if (infoButton != null)
        {
            infoButton.onClick.AddListener(ToggleInfoPanel); // Add listener for the button
            infoButton.gameObject.SetActive(false); // Start with the button hidden
        }
    }

    public void ToggleInfoPanel()
    {
        if (infoPanel == null) return;

        isInfoPanelActive = !isInfoPanelActive;
        infoPanel.SetActive(isInfoPanelActive);
        Debug.Log($"InfoScreenManager: Info panel toggled. Now: {(isInfoPanelActive ? "Active" : "Inactive")}");
    }

    // This method is called by ImageTracker
    public void DisplayInfoForMarker(string detectedMarkerName)
    {
        currentMarkerNameForInfo = detectedMarkerName; // Store the name of the marker we are about to display

        if (markerContentDictionary.TryGetValue(detectedMarkerName, out string content))
        {
            if (infoText != null)
            {
                infoText.text = content; // Set the text on the panel
                Debug.Log($"InfoScreenManager: Displaying info content for: {detectedMarkerName}");
            }
        }
        else
        {
            if (infoText != null)
            {
                // Fallback content if marker name not found in dictionary
                infoText.text = $"Information not found for marker: {detectedMarkerName}.\nPlease add content for this marker in the InfoScreenManager component in the Inspector.";
                Debug.LogWarning($"InfoScreenManager: No information content found for marker: {detectedMarkerName}");
            }
        }

        ShowInfoButton(); // Make the button visible
    }

    public void ShowInfoButton()
    {
        if (infoButton != null && !infoButton.gameObject.activeSelf)
        {
            infoButton.gameObject.SetActive(true);
            Debug.Log($"InfoScreenManager: Info button SHOWN for marker: {currentMarkerNameForInfo}");
        }
    }

    // This method is called by ImageTracker
    public void HideInfoButton()
    {
        if (infoButton != null && infoButton.gameObject.activeSelf)
        {
            infoButton.gameObject.SetActive(false);
            Debug.Log("InfoScreenManager: Info button HIDDEN.");
        }

        // Also hide the info panel itself if the button is hidden
        if (infoPanel != null && infoPanel.activeSelf)
        {
            infoPanel.SetActive(false);
            isInfoPanelActive = false;
            Debug.Log("InfoScreenManager: Info panel hidden because button was hidden.");
        }
        
        currentMarkerNameForInfo = ""; // Clear the marker name when button is hidden
    }

    // Helper method (optional, for debugging or other scripts)
    public string GetCurrentMarkerForInfo()
    {
        return currentMarkerNameForInfo;
    }
}