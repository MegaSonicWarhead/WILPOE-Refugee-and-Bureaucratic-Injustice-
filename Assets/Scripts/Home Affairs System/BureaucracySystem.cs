using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BureaucracySystem : MonoBehaviour
{
    public int paperworkProgress = 0;
    public int requiredProgress = 100;

    public void SubmitPaperwork()
    {
        int progressGain = UnityEngine.Random.Range(5, 15);
        paperworkProgress += progressGain;
        Debug.Log("Paperwork submitted! Progress: " + paperworkProgress + "/" + requiredProgress);

        if (paperworkProgress >= requiredProgress)
        {
            Debug.Log("Paperwork Approved! You win the game.");
            GameManager.Instance.EndGame();
        }
    }

    public void EncounterDelay()
    {
        Debug.Log("Bureaucratic delay! No progress today.");
    }
}
