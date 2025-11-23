using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        //Debug.Log($"[GameState] Awake in scene: {SceneManager.GetActiveScene().name}, object: {gameObject.name}");

        if (Instance != null && Instance != this)
        {
           // Debug.LogWarning($"[GameState] Duplicate detected -> Destroying {gameObject.name} in scene: {SceneManager.GetActiveScene().name}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Debug.Log($"[GameState] Singleton Instance set to: {gameObject.name}");

        DontDestroyOnLoad(gameObject);
       // Debug.Log($"[GameState] DontDestroyOnLoad called for: {gameObject.name}");
    }

    private void OnEnable()
    {
        //Debug.Log($"[GameState] OnEnable called for {gameObject.name}");
    }

    private void Start()
    {
        //Debug.Log($"[GameState] Start called for {gameObject.name}");
    }

    private void OnDisable()
    {
        //Debug.Log($"[GameState] OnDisable called for {gameObject.name}");
    }

    private void OnDestroy()
    {
        //Debug.LogError($"[GameState] OnDestroy called for {gameObject.name} in scene: {SceneManager.GetActiveScene().name}");
    }

    public void AdvanceDay()
    {
        clueGivenToday = false;
        clueHistory.Clear(); // Clear clue history when day advances so players can ask for clues again
        currentDay++;

        if (currentDay > 4)
        {
            currentDay = 1;
            currentWeek++;
        }

        if (currentWeek > 12)
        {
            Debug.Log("[GameState] Game Over - Time Expired! Player should be deported.");
            LoadDeportedScene();
        }
    }

    private void LoadDeportedScene()
    {
        if (Application.CanStreamedLevelBeLoaded("Deported"))
        {
            Debug.Log("[GameState] Loading Deported scene - Time limit exceeded.");
            SceneManager.LoadScene("Deported");
        }
        else
        {
            Debug.LogError("[GameState] Deported scene not found in Build Settings!");
        }
    }

    private void CheckWinCondition()
    {
        // Check if player completed application within 12 weeks
        if (playerProgression == PlayerProgression.CompletedApplication && currentWeek <= 12)
        {
            LoadWinScene();
        }
    }

    private void LoadWinScene()
    {
        // Change "Win" to your actual win scene name
        string winSceneName = "Win"; // Update this to your actual win scene name
        
        if (Application.CanStreamedLevelBeLoaded(winSceneName))
        {
            Debug.Log($"[GameState] Loading {winSceneName} scene - Application completed within 12 weeks!");
            SceneManager.LoadScene(winSceneName);
        }
        else
        {
            Debug.LogWarning($"[GameState] Win scene '{winSceneName}' not found in Build Settings! Please add it or update the scene name.");
        }
    }

    public void AcquireDocument(DocumentType doc)
    {
        if (acquiredDocuments.Add(doc))
        {
            Debug.Log($"[GameState] Document acquired: {doc}. Current progression before check: {playerProgression}");
            CheckAndAdvanceProgression();
            Debug.Log($"[GameState] Progression after check: {playerProgression}");
        }
        else
        {
            Debug.LogWarning($"[GameState] Document {doc} was already acquired. Current progression: {playerProgression}");
        }
    }

    public bool HasDocument(DocumentType doc)
    {
        return acquiredDocuments.Contains(doc);
    }

    private void CheckAndAdvanceProgression()
    {
        PlayerProgression oldProgression = playerProgression;
        
        // Handle None case - initialize progression based on what documents are acquired
        if (playerProgression == PlayerProgression.None)
        {
            if (HasDocument(DocumentType.FirstInterview))
                playerProgression = PlayerProgression.CompletedApplication;
            else if (HasDocument(DocumentType.TravelDocument))
                playerProgression = PlayerProgression.Step5_AcquireFirstInterview;
            else if (HasDocument(DocumentType.Biometrics))
                playerProgression = PlayerProgression.Step4_AcquireTravelDocument;
            else if (HasDocument(DocumentType.ID))
                playerProgression = PlayerProgression.Step3_AcquireBiometrics;
            else if (HasDocument(DocumentType.AsylumApplicationFormDHA1590))
                playerProgression = PlayerProgression.Step2_AcquireID;
            else
                playerProgression = PlayerProgression.Step1_AcquireAsylumApplicationForm;
            
            if (playerProgression != oldProgression)
            {
                Debug.Log($"[GameState] Progression initialized from None: {oldProgression} -> {playerProgression}");
                // After initializing, continue to check if we can advance further
                oldProgression = playerProgression;
            }
        }
        
        // Now check and advance based on current progression
        switch (playerProgression)
        {
            case PlayerProgression.Step1_AcquireAsylumApplicationForm:
                if (HasDocument(DocumentType.AsylumApplicationFormDHA1590))
                {
                    playerProgression = PlayerProgression.Step2_AcquireID;
                    Debug.Log($"[GameState] Progression advanced: {oldProgression} -> {playerProgression}");
                }
                break;
            case PlayerProgression.Step2_AcquireID:
                if (HasDocument(DocumentType.ID))
                {
                    playerProgression = PlayerProgression.Step3_AcquireBiometrics;
                    Debug.Log($"[GameState] Progression advanced: {oldProgression} -> {playerProgression}");
                }
                break;
            case PlayerProgression.Step3_AcquireBiometrics:
                if (HasDocument(DocumentType.Biometrics))
                {
                    playerProgression = PlayerProgression.Step4_AcquireTravelDocument;
                    Debug.Log($"[GameState] Progression advanced: {oldProgression} -> {playerProgression}");
                }
                break;
            case PlayerProgression.Step4_AcquireTravelDocument:
                if (HasDocument(DocumentType.TravelDocument))
                {
                    playerProgression = PlayerProgression.Step5_AcquireFirstInterview;
                    Debug.Log($"[GameState] Progression advanced: {oldProgression} -> {playerProgression}");
                }
                break;
            case PlayerProgression.Step5_AcquireFirstInterview:
                if (HasDocument(DocumentType.FirstInterview))
                {
                    playerProgression = PlayerProgression.CompletedApplication;
                    Debug.Log($"[GameState] Progression advanced: {oldProgression} -> {playerProgression}");
                    // Check if player won (completed within 12 weeks)
                    CheckWinCondition();
                }
                break;
            case PlayerProgression.CompletedApplication:
                // Already completed, no need to advance
                break;
            default:
                Debug.LogWarning($"[GameState] CheckAndAdvanceProgression called with unexpected progression: {playerProgression}");
                break;
        }
    }

    public bool IsGameComplete()
    {
        return playerProgression == PlayerProgression.CompletedApplication;
    }
}
