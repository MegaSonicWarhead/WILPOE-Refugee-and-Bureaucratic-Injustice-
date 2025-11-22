using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanel : MonoBehaviour
{
    [Header("Panel to Open")]
    public GameObject targetPanel;

    public void OpenRefugeePanel()
    {
        if (targetPanel != null)
            targetPanel.SetActive(true);
    }
}
