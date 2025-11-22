using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    [Header("Panel to Close")]
    public GameObject targetPanel;

    public void CloseRefugeePanel()
    {
        if (targetPanel != null)
            targetPanel.SetActive(false);
    }
}
