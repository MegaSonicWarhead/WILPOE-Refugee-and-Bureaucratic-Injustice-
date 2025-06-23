using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfficerManager : MonoBehaviour
{
    [Header("Officer Data")]
    public List<OfficerData> officers;

    [Header("UI Elements")]
    public Image officerImage;
    public TMP_Text officerNameText;
    public TMP_Text responseText;
    public Button getOfficerButton;
    public Button actionButton1;
    public Button actionButton2;
    public Button helpButton;
    public Button leaveButton;
    public GameObject officerPanel;

    private OfficerData currentOfficer;

    private string[] wrongDocumentAnswers = new string[]
    {
        "BI-947 form: Application form for refugee status",
        "Valid passport: For yourself and family members",
        "Application for certification (BI 1754)",
        "Proof of 10 years continuous refugee status",
        "Valid refugee status document (Section 24 visa)",
        "Information about dependents: Refugee status, birth certificates, marriage certificates",
        "Certified copies of passports",
        "Police affidavit (if applicable): If you claim not to have a passport",
        "Marriage certificate: If married and spouse is accompanying you",
        "Proof of parental responsibilities: If bringing dependent children",
        "Birth certificates: For yourself and any dependents",
        "Police clearance certificate: If 18 or older",
        "Yellow fever vaccination certificate: If applicable"
    };

    private void Start()
    {
        officerPanel.SetActive(false);
        getOfficerButton.onClick.AddListener(AssignOfficer);
        actionButton1.onClick.AddListener(() => HandleResponse(1));
        actionButton2.onClick.AddListener(() => HandleResponse(2));
        helpButton.onClick.AddListener(GiveDocumentClue);
        leaveButton.onClick.AddListener(ClosePanel);
    }

    void ClosePanel() => officerPanel.SetActive(false);

    void AssignOfficer()
    {
        currentOfficer = GetRandomOfficer();
        if (currentOfficer == null) return;

        officerPanel.SetActive(true);
        officerImage.sprite = currentOfficer.officerImage;
        officerNameText.text = currentOfficer.officerName;
        responseText.text = currentOfficer.initialGreeting;

        SetActionButtonTextsByProgression();
    }

    void SetActionButtonTextsByProgression()
    {
        string docText = "Temporary Permit";

        switch (GameState.Instance.playerProgression)
        {
            case PlayerProgression.FirstVisit:
                SetButtons($"{docText} First Required Document", "I Don't Know?");
                break;
            case PlayerProgression.SecondVisit:
                SetButtons($"{docText} Second Required Document", "I Don't Know?");
                break;
            case PlayerProgression.ThirdVisit:
                SetButtons($"{docText} Third Required Document", "I Don't Know?");
                break;
            case PlayerProgression.FourthVisit:
                SetButtons($"{docText} Fourth Required Document", "I Don't Know?");
                break;
            case PlayerProgression.FifthVisit:
                SetButtons($"{docText} Fifth Required Document", "I Don't Know?");
                break;
            case PlayerProgression.AskedPermit1:
                SetButtons("I Have the First Document", "I Need Help");
                break;
            case PlayerProgression.AskedPermit2:
                SetButtons("I Have the Second Document", "I Need Help");
                break;
            case PlayerProgression.AskedPermit3:
                SetButtons("I Have the Third Document", "I Need Help");
                break;
            case PlayerProgression.AskedPermit4:
                SetButtons("I Have the Fourth Document", "I Need Help");
                break;
            case PlayerProgression.AskedPermit5:
                SetButtons("I Have the Fifth Document", "I Need Help");
                break;
            case PlayerProgression.HasDocuments:
                SetButtons("Submit Application", "Missing Something?");
                break;
            case PlayerProgression.CompletedApplication:
                SetButtons("What's Next?", "Thank You");
                break;
        }
    }

    void SetButtons(string text1, string text2)
    {
        actionButton1.GetComponentInChildren<TMP_Text>().text = text1;
        actionButton2.GetComponentInChildren<TMP_Text>().text = text2;
    }

    void HandleResponse(int option)
    {
        if (currentOfficer == null) return;

        string text = option == 1
            ? actionButton1.GetComponentInChildren<TMP_Text>().text
            : actionButton2.GetComponentInChildren<TMP_Text>().text;

        responseText.text = $"{currentOfficer.officerName}: {GetResponseForButton(text)}";
    }

    void GiveDocumentClue()
    {
        if (currentOfficer == null) return;

        if (GameState.Instance.clueGivenToday)
        {
            responseText.text = $"{currentOfficer.officerName}: I already helped you today.";
            return;
        }

        DocumentType neededDoc = GetExpectedDocumentForProgression();
        string correctLocation = GetCorrectLocationForDocument(neededDoc);

        if (GameState.Instance.HasDocument(neededDoc))
        {
            responseText.text = $"{currentOfficer.officerName}: You already have that document.";
            return;
        }

        string message;

        if (currentOfficer.officerType == OfficerType.Nice)
        {
            message = $"You’ll find your {neededDoc} at the {correctLocation}.";
        }
        else if (currentOfficer.officerType == OfficerType.Corrupt)
        {
            if (TryBribeForClue(out string bribeMsg))
                message = bribeMsg;
            else
                message = $"Maybe you need a {GetRandomWrongDocument()} from the {GetRandomLocation()}?";
        }
        else
        {
            message = $"Maybe you need a {GetRandomWrongDocument()} from the {GetRandomLocation()}?";
        }

        GameState.Instance.clueGivenToday = true;
        GameState.Instance.clueHistory[neededDoc] = correctLocation;
        responseText.text = $"{currentOfficer.officerName}: {message}";
    }

    bool TryBribeForClue(out string message)
    {
        DocumentType neededDoc = GetExpectedDocumentForProgression();
        string correctLocation = GetCorrectLocationForDocument(neededDoc);

        if (MoneySystem.Instance != null && MoneySystem.Instance.SpendMoney(50))
        {
            message = $"Alright... You’ll find your {neededDoc} at the {correctLocation}.";
            return true;
        }

        message = null;
        return false;
    }

    string GetRandomWrongDocument()
    {
        return wrongDocumentAnswers[Random.Range(0, wrongDocumentAnswers.Length)];
    }

    DocumentType GetExpectedDocumentForProgression()
    {
        return GameState.Instance.playerProgression switch
        {
            PlayerProgression.FirstVisit => DocumentType.AsylumApplicationFormDHA1590,
            PlayerProgression.SecondVisit => DocumentType.ID,
            PlayerProgression.ThirdVisit => DocumentType.Biometrics,
            PlayerProgression.FourthVisit => DocumentType.TravelDocument,
            PlayerProgression.FifthVisit => DocumentType.FirstInterview,
            _ => DocumentType.ID
        };
    }

    string GetCorrectLocationForDocument(DocumentType doc) => doc switch
    {
        DocumentType.ID => "Post Office",
        DocumentType.TravelDocument => "Post Office",
        DocumentType.Biometrics => "Bank",
        DocumentType.AsylumApplicationFormDHA1590 => "Embassy",
        DocumentType.FirstInterview => "Embassy",
        _ => "Unknown"
    };

    string GetRandomLocation()
    {
        string[] locations = { "Post Office", "Embassy", "Bank" };
        return locations[Random.Range(0, locations.Length)];
    }

    string GetResponseForButton(string text) => text switch
    {
        "Temporary Permit First Required Document" => Progress(PlayerProgression.AskedPermit1, "You need Asylum Application Form DHA-1590."),
        "Temporary Permit Second Required Document" => Progress(PlayerProgression.AskedPermit2, "You need a copy of your ID."),
        "Temporary Permit Third Required Document" => Progress(PlayerProgression.AskedPermit3, "You need to do your biometrics: fingerprints and ID photos."),
        "Temporary Permit Fourth Required Document" => Progress(PlayerProgression.AskedPermit4, "You need a Travel Document."),
        "Temporary Permit Fifth Required Document" => Progress(PlayerProgression.AskedPermit5, "You need to complete your First Interview and submit proof."),
        "I Have all the Documents" => Progress(PlayerProgression.HasDocuments, "Great. Let me check what you have."),
        "Submit Application" => Progress(PlayerProgression.CompletedApplication, "Application submitted. Now wait for a response."),
        "I Need Help" => "Ask specific questions or check the checklist.",
        "Missing Something?" => "Make sure you have all required documents.",
        "What's Next?" => "You’ll be contacted by an officer for next steps.",
        "Thank You" => "You're welcome. Good luck.",
        "I Don't Know?" => "Please come back when you're ready.",
        _ => "I don't understand your request."
    };

    string Progress(PlayerProgression newProgress, string msg)
    {
        GameState.Instance.playerProgression = newProgress;
        return msg;
    }

    OfficerData GetRandomOfficer()
    {
        if (officers.Count == 0) return null;
        return officers[Random.Range(0, officers.Count)];
    }

}
