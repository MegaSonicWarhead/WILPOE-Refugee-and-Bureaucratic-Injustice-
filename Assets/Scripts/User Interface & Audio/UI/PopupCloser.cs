using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PopupCloser : MonoBehaviour
{
    [Header("Assign the panel you want to close")]
    public GameObject popupPanel;

    public void Close()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
            Debug.Log("Popup closed.");
        }
        else
        {
            Debug.LogError("PopupCloser: No popup panel assigned!");
        }
    }
}
