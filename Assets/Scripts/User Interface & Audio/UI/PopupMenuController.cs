using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenuController : MonoBehaviour
{
    // References to all panels (including all pop-ups)
    public GameObject[] allPanels; // List of all panels (main menu, popups, etc.)

    // References to specific popup panels
    public GameObject dailyObjectivesPopup;
    public GameObject foodPopup;

    // Reference to the main panel (e.g., background or menu)
    public GameObject mainPanel;

    // Track the currently active popup
    private GameObject activePopup = null;

    void Start()
    {
        // Hide all pop-ups on start
        HideAllPopups();
    }

    // Function to toggle the daily objectives popup
    public void ToggleDailyObjectivesPopup()
    {
        if (activePopup == dailyObjectivesPopup)
        {
            HideAllPopups(); // Hide pop-up if it's already active
        }
        else
        {
            ShowPopup(dailyObjectivesPopup); // Show daily objectives pop-up
        }
    }

    // Function to toggle the food popup
    public void ToggleFoodPopup()
    {
        if (activePopup == foodPopup)
        {
            HideAllPopups(); // Hide pop-up if it's already active
        }
        else
        {
            ShowPopup(foodPopup); // Show food pop-up
        }
    }

    // Function to close the pop-up and go back to the main panel
    public void ClosePopupAndShowMainPanel()
    {
        HideAllPopups(); // Hide all pop-ups
        if (mainPanel != null)
        {
            mainPanel.SetActive(true); // Show the main panel
        }
    }

    // Helper function to show a specific popup and hide others
    private void ShowPopup(GameObject popup)
    {
        HideAllPopups(); // Hide any previously active pop-ups
        popup.SetActive(true); // Show the selected pop-up
        activePopup = popup; // Mark this pop-up as active

        // Hide the main panel when a pop-up is shown
        if (mainPanel != null)
        {
            mainPanel.SetActive(false); // Hide the main panel
        }
    }

    // Function to hide all popups and show the main panel
    private void HideAllPopups()
    {
        // Hide all popups (dailyObjectives, food, etc.)
        dailyObjectivesPopup.SetActive(false);
        foodPopup.SetActive(false);

        // Reset the active popup tracker
        activePopup = null;

        // Show the main panel when no pop-up is visible
        if (mainPanel != null)
        {
            mainPanel.SetActive(true); // Show the main panel
        }
    }
}
