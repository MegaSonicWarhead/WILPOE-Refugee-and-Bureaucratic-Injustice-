using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public void OnBackPressed()
    {
        if (BackButtonManager.Instance != null)
        {
            BackButtonManager.Instance.GoBack();
        }
    }
}
