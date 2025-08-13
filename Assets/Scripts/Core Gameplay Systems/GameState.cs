using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public enum PlayerProgression
{
    None, // Not started
    Step1_AcquireAsylumApplicationForm,
    Step2_AcquireID,
    Step3_AcquireBiometrics,
    Step4_AcquireTravelDocument,
    Step5_AcquireFirstInterview,
    CompletedApplication
}

public enum DocumentType
{
    AsylumApplicationFormDHA1590,
    ID,
    Biometrics,
    TravelDocument,
    FirstInterview
}


public class GameState : MonoBehaviour
{
    public static GameState Instance;

    public PlayerProgression playerProgression = PlayerProgression.Step1_AcquireAsylumApplicationForm;

    public int currentDay = 1;
    public int currentWeek = 1;

    public HashSet<DocumentType> acquiredDocuments = new HashSet<DocumentType>();

    public bool clueGivenToday = false;

    public Dictionary<DocumentType, string> clueHistory = new Dictionary<DocumentType, string>();

    private void Awake()
    {
        Debug.Log($"[GameState] Awake called in scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}, object: {gameObject.name}");
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"[GameState] Duplicate detected! Destroying object: {gameObject.name} in scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Debug.Log($"[GameState] Instance set to: {gameObject.name} in scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        DontDestroyOnLoad(gameObject);
        Debug.Log($"[GameState] DontDestroyOnLoad called for: {gameObject.name}");
    }

    public void AdvanceDay()
    {
        clueGivenToday = false;

        currentDay++;
        if (currentDay > 4)
        {
            currentDay = 1;
            currentWeek++;
        }

        if (currentWeek > 12)
        {
            Debug.Log("Game Over - Time Expired!");
        }
    }

    // Call this when the player acquires a document
    public void AcquireDocument(DocumentType doc)
    {
        if (acquiredDocuments.Add(doc))
        {
            Debug.Log($"Document acquired: {doc}");
            CheckAndAdvanceProgression();
        }
    }

    public bool HasDocument(DocumentType doc)
    {
        return acquiredDocuments.Contains(doc);
    }

    // Automatically advances progression if the required document for the current step is acquired
    private void CheckAndAdvanceProgression()
    {
        switch (playerProgression)
        {
            case PlayerProgression.Step1_AcquireAsylumApplicationForm:
                if (HasDocument(DocumentType.AsylumApplicationFormDHA1590))
                    playerProgression = PlayerProgression.Step2_AcquireID;
                break;
            case PlayerProgression.Step2_AcquireID:
                if (HasDocument(DocumentType.ID))
                    playerProgression = PlayerProgression.Step3_AcquireBiometrics;
                break;
            case PlayerProgression.Step3_AcquireBiometrics:
                if (HasDocument(DocumentType.Biometrics))
                    playerProgression = PlayerProgression.Step4_AcquireTravelDocument;
                break;
            case PlayerProgression.Step4_AcquireTravelDocument:
                if (HasDocument(DocumentType.TravelDocument))
                    playerProgression = PlayerProgression.Step5_AcquireFirstInterview;
                break;
            case PlayerProgression.Step5_AcquireFirstInterview:
                if (HasDocument(DocumentType.FirstInterview))
                    playerProgression = PlayerProgression.CompletedApplication;
                break;
            default:
                break;
        }
    }

    public bool IsGameComplete()
    {
        return playerProgression == PlayerProgression.CompletedApplication;
    }
}
