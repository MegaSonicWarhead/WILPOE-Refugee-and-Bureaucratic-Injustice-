using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public enum PlayerProgression
{
    FirstVisit,
    SecondVisit,
    ThirdVisit,
    FourthVisit,
    FifthVisit,
    AskedPermit1,
    AskedPermit2,
    AskedPermit3,
    AskedPermit4,
    AskedPermit5,
    HasDocuments,
    CompletedApplication
}

public enum DocumentType
{
    AsylumApplicationFormDHA1590,
    ID,
    TravelDocument,
    Biometrics,
    FirstInterview
}


public class GameState : MonoBehaviour
{
    public static GameState Instance;

    public PlayerProgression playerProgression = PlayerProgression.FirstVisit;

    public int currentDay = 1;
    public int currentWeek = 1;

    public HashSet<DocumentType> acquiredDocuments = new HashSet<DocumentType>();

    public bool clueGivenToday = false;

    public Dictionary<DocumentType, string> clueHistory = new Dictionary<DocumentType, string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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

    public void AcquireDocument(DocumentType doc)
    {
        if (acquiredDocuments.Add(doc))
        {
            Debug.Log($"Document acquired: {doc}");
        }
    }

    public bool HasDocument(DocumentType doc)
    {
        return acquiredDocuments.Contains(doc);
    }

    public bool IsGameComplete()
    {
        return acquiredDocuments.Count == System.Enum.GetNames(typeof(DocumentType)).Length;
    }
}
