using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles Queue Button behavior: triggers event, shows popup, updates time.
/// </summary>
[RequireComponent(typeof(Button))]
public class QueueButton : MonoBehaviour
{
    [Header("References")]
    public EventManager eventManager;
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;

    private Button button;

    private void Awake()
    {
        // Ensure the popup is hidden immediately when the object loads
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("QueueButton: PopupPanel reference is missing!");
        }
    }

    private void Start()
    {
        // Cache the Button component
        button = GetComponent<Button>();

        // Safety checks
        if (button == null)
        {
            Debug.LogError("QueueButton: No Button component found on this GameObject!");
            return;
        }
        if (eventManager == null)
        {
            Debug.LogError("QueueButton: EventManager reference is missing!");
            return;
        }
        if (popupText == null)
        {
            Debug.LogError("QueueButton: PopupText reference is missing!");
            return;
        }

        // Register the button click event
        button.onClick.AddListener(OnQueueButtonClicked);
    }

    private void OnQueueButtonClicked()
    {
        EventOutcome outcome = eventManager.TriggerEvent();

        if (outcome != null)
        {
            popupPanel.SetActive(true);
            popupPanel.transform.SetAsLastSibling(); // Force popup to front of Canvas
            popupText.text = $"You waited {outcome.hoursToAdvance} hours.";

            if (GameTime.Instance != null)
            {
                GameTime.Instance.AdvanceHours(outcome.hoursToAdvance);
            }
            else
            {
                Debug.LogError("QueueButton: GameTime.Instance is null! Make sure GameTime prefab exists in this scene!");
            }
        }
        else
        {
            Debug.LogError("QueueButton: No outcome received from EventManager!");
        }
    }

    /// <summary>
    /// Called by the close button on the popup panel to hide it.
    /// </summary>
    public void ClosePopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
            Debug.LogError("popup menu is false ...");
        }
        else
        {
            Debug.LogError("QueueButton: PopupPanel reference is missing when trying to close popup!");
        }
    }
}
