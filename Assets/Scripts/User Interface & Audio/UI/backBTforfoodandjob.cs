using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backBTforfoodandjob : MonoBehaviour
{
    // Start is called before the first frame update
    // Function to close the pop-up and go back to the main panel

    // References to specific popup panels
    //public GameObject dailyObjectivesPopup;
    public GameObject foodPopup;

    // Reference to the main panel (e.g., background or menu)
    public GameObject mainPanel;

    public GameObject JobPanel;

    // Track the currently active popup
    private GameObject activePopup = null;

    public void ClosePopupAndShowMainPanel()
    {
        HideAllPopups(); // Hide all pop-ups
        if (mainPanel != null)
        {
            mainPanel.SetActive(true); // Show the main panel
        }
    }

    // Function to hide all popups and show the main panel
    private void HideAllPopups()
    {
        // Hide all popups (dailyObjectives, food, etc.)

        foodPopup.SetActive(false);
        JobPanel.SetActive(false);

        // Reset the active popup tracker
        activePopup = null;

        // Show the main panel when no pop-up is visible
        if (mainPanel != null)
        {
            mainPanel.SetActive(true); // Show the main panel
        }
    }
}
