using UnityEngine;
using TMPro;
using System.Reflection;

/// <summary>
/// Handles VR UI interactions for Quest - bridges Meta XR Interaction SDK to existing AIManager
/// </summary>
public class QuestUI : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag in your existing AIManager to connect VR interactions")]
    public AIManager aiManager;
    
    [Header("Button UI References")]
    [Tooltip("Drag the PokeButton prefab/GameObject here")]
    public GameObject pokeButton;
    
    [Header("Button States")]
    [Tooltip("Text to show when ready to record")]
    public string recordText = "Record";
    
    [Tooltip("Text to show when recording")]
    public string stopText = "Stop";
    
    [Tooltip("Recording button color (darker red for better text contrast)")]
    public Color recordingColor = new Color(1.0f, 0f, 0f, 0.14f); // Darker red for better white text visibility
    
    [Header("Debug")]
    [Tooltip("Enable detailed logging of VR interactions")]
    public bool enableDebugLogging = true;

    private bool isRecording = false;
    private Color defaultColor; // Auto-discovered from button
    private bool defaultColorDiscovered = false;

    void Start()
    {
        // Auto-find AIManager if not assigned
        if (aiManager == null)
        {
            aiManager = FindObjectOfType<AIManager>();
            if (aiManager == null)
            {
                Debug.LogError("QuestUI: No AIManager found! Please assign one in the inspector.");
            }
            else
            {
                Debug.Log("QuestUI: Auto-found AIManager: " + aiManager.gameObject.name);
            }
        }
    }

    /// <summary>
    /// Called when the record button is pressed via VR poke interaction
    /// </summary>
    /// <param name="sender">The GameObject that triggered the interaction (usually the button)</param>
    public void RecordButtonPressed(GameObject sender)
    {
        if (enableDebugLogging)
        {
            Debug.Log($"[QuestUI] Record button pressed!");
            Debug.Log($"  - Sender: {sender?.name ?? "null"}");
            Debug.Log($"  - Time: {Time.time:F2}s");
            Debug.Log($"  - Current state: {(isRecording ? "Recording" : "Ready")}");
            Debug.Log($"  - AIManager connected: {aiManager != null}");
        }

        // Toggle recording state
        isRecording = !isRecording;
        
        // Update button visual state
        UpdateButtonVisuals();

        // TODO Phase 3: Connect to existing AIManager functionality
        if (aiManager != null)
        {
            Debug.Log($"[QuestUI] Would call aiManager.ToggleRecording() here (new state: {(isRecording ? "Recording" : "Stopped")})");
            // aiManager.ToggleRecording();
        }
        else
        {
            Debug.LogWarning("[QuestUI] Cannot trigger recording - AIManager is null!");
        }
    }

    /// <summary>
    /// Updates the button text and color based on current recording state
    /// </summary>
    private void UpdateButtonVisuals()
    {
        if (pokeButton == null)
        {
            if (enableDebugLogging)
            {
                Debug.LogWarning("[QuestUI] pokeButton is null - please assign the PokeButton GameObject in inspector!");
            }
            return;
        }

        // Auto-discover default color on first run
        if (!defaultColorDiscovered)
        {
            DiscoverDefaultColor();
        }

        // Find and update text component
        if (enableDebugLogging)
        {
            Debug.Log($"[QuestUI] Searching for TextMeshPro components in {pokeButton.name} children...");
            var allTMPsUI = pokeButton.GetComponentsInChildren<TextMeshProUGUI>();
            var allTMPs3D = pokeButton.GetComponentsInChildren<TMPro.TextMeshPro>();
            Debug.Log($"[QuestUI] Found {allTMPsUI.Length} TextMeshProUGUI (UI) components");
            Debug.Log($"[QuestUI] Found {allTMPs3D.Length} TextMeshPro (3D) components:");
            for (int i = 0; i < allTMPs3D.Length; i++)
            {
                Debug.Log($"[QuestUI]   TMP3D {i}: {allTMPs3D[i].gameObject.name} - Current text: '{allTMPs3D[i].text}'");
            }
        }

        // Try TextMeshProUGUI first (UI), then TextMeshPro (3D)
        var buttonTextUI = pokeButton.GetComponentInChildren<TextMeshProUGUI>();
        var buttonText3D = pokeButton.GetComponentInChildren<TMPro.TextMeshPro>();
        
        if (buttonTextUI != null)
        {
            string oldText = buttonTextUI.text;
            buttonTextUI.text = isRecording ? stopText : recordText;
            if (enableDebugLogging)
            {
                Debug.Log($"[QuestUI] Updated UI button text from '{oldText}' to '{buttonTextUI.text}' on GameObject: {buttonTextUI.gameObject.name}");
            }
        }
        else if (buttonText3D != null)
        {
            string oldText = buttonText3D.text;
            buttonText3D.text = isRecording ? stopText : recordText;
            if (enableDebugLogging)
            {
                Debug.Log($"[QuestUI] Updated 3D button text from '{oldText}' to '{buttonText3D.text}' on GameObject: {buttonText3D.gameObject.name}");
            }
        }
        else if (enableDebugLogging)
        {
            Debug.LogWarning("[QuestUI] Could not find any TextMeshPro component (UI or 3D) in PokeButton children!");
        }

        // Update button color using helper method
        SetButtonPanelColor(isRecording ? recordingColor : defaultColor);
    }

    /// <summary>
    /// Auto-discovers the default color from the InteractableColorVisual component
    /// </summary>
    private void DiscoverDefaultColor()
    {
        var interactableColorVisual = FindInteractableColorVisual();
        if (interactableColorVisual != null)
        {
            var normalColorStateField = interactableColorVisual.GetType().GetField("_normalColorState", BindingFlags.NonPublic | BindingFlags.Instance);
            if (normalColorStateField != null)
            {
                var normalColorState = normalColorStateField.GetValue(interactableColorVisual);
                var colorField = normalColorState.GetType().GetField("Color");
                if (colorField != null)
                {
                    defaultColor = (Color)colorField.GetValue(normalColorState);
                    defaultColorDiscovered = true;
                    if (enableDebugLogging)
                    {
                        Debug.Log($"[QuestUI] Auto-discovered default color from InteractableColorVisual: {defaultColor}");
                    }
                    return;
                }
            }
        }
        
        // Fallback to white if we can't find any color
        defaultColor = Color.white;
        defaultColorDiscovered = true;
        if (enableDebugLogging)
        {
            Debug.LogWarning("[QuestUI] Could not auto-discover default color from InteractableColorVisual, using white as fallback");
        }
    }



    /// <summary>
    /// Finds the InteractableColorVisual component on the button
    /// </summary>
    private MonoBehaviour FindInteractableColorVisual()
    {
        // Check both the PokeButton itself AND its children
        var allComponents = pokeButton.GetComponentsInChildren<MonoBehaviour>(true); // Include inactive
        var buttonComponents = pokeButton.GetComponents<MonoBehaviour>(); // Check button itself too
        
        // Combine both arrays
        var combinedComponents = new MonoBehaviour[allComponents.Length + buttonComponents.Length];
        buttonComponents.CopyTo(combinedComponents, 0);
        allComponents.CopyTo(combinedComponents, buttonComponents.Length);
        
        // Search for InteractableColorVisual component
        foreach (var component in combinedComponents)
        {
            if (component != null && component.GetType().Name == "InteractableColorVisual")
            {
                return component;
            }
        }
        return null;
    }

    /// <summary>
    /// Sets the color using the proper Meta XR InteractableColorVisual system
    /// </summary>
    private void SetButtonPanelColor(Color color)
    {
        var interactableColorVisual = FindInteractableColorVisual();
        
        if (enableDebugLogging)
        {
            Debug.Log($"[QuestUI] InteractableColorVisual: {(interactableColorVisual != null ? "FOUND" : "NOT FOUND")}");
        }
        
        if (interactableColorVisual != null)
        {
            // Use reflection to access the private _normalColorState field
            var normalColorStateField = interactableColorVisual.GetType().GetField("_normalColorState", BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (normalColorStateField != null)
            {
                // Get the ColorState object
                var normalColorState = normalColorStateField.GetValue(interactableColorVisual);
                
                if (enableDebugLogging)
                {
                    Debug.Log($"[QuestUI] Found normalColorState: {normalColorState}");
                    var fields = normalColorState.GetType().GetFields();
                    Debug.Log($"[QuestUI] ColorState fields:");
                    foreach (var field in fields)
                    {
                        Debug.Log($"[QuestUI]   Field: {field.Name} ({field.FieldType.Name})");
                    }
                }
                
                // Set the Color FIELD of the ColorState (not property!)
                var colorField = normalColorState.GetType().GetField("Color");
                if (colorField != null)
                {
                    Color oldColor = (Color)colorField.GetValue(normalColorState);
                    colorField.SetValue(normalColorState, color);
                    
                    // Force the InteractableColorVisual to update by calling UpdateVisual
                    var updateMethod = interactableColorVisual.GetType().GetMethod("UpdateVisual", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (updateMethod != null)
                    {
                        updateMethod.Invoke(interactableColorVisual, null);
                        if (enableDebugLogging)
                        {
                            Debug.Log($"[QuestUI] Called UpdateVisual() to refresh InteractableColorVisual");
                        }
                    }
                    
                    if (enableDebugLogging)
                    {
                        Debug.Log($"[QuestUI] Updated InteractableColorVisual normalColorState Color field from {oldColor} to {color}");
                    }
                    return;
                }
                else if (enableDebugLogging)
                {
                    Debug.LogWarning("[QuestUI] Could not find Color field on ColorState!");
                }
            }
            else if (enableDebugLogging)
            {
                Debug.LogWarning("[QuestUI] Could not find _normalColorState field on InteractableColorVisual!");
            }
        }
        
        if (enableDebugLogging)
        {
            Debug.LogWarning("[QuestUI] Could not find InteractableColorVisual component to change button color properly!");
        }
    }


} 