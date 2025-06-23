using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClosePopupMenuBtton : MonoBehaviour
{
    public GameObject popupPanel;
    public Button getOfficerButton;
    public void ClosePopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
            //Debug.LogError("popup menu is false ...");
            if (getOfficerButton != null)
            {
                getOfficerButton.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("QueueButton: PopupPanel reference is missing when trying to close popup!");
        }
    }
}
