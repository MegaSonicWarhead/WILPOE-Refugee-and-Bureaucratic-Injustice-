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
        // Set button text based on the current progression step
        switch (GameState.Instance.playerProgression)
        {
            case PlayerProgression.Step1_AcquireAsylumApplicationForm:
                SetButtons("Ask about Asylum Application Form", "I Don't Know?");
                break;
            case PlayerProgression.Step2_AcquireID:
                SetButtons("Ask about ID", "I Don't Know?");
                break;
            case PlayerProgression.Step3_AcquireBiometrics:
                SetButtons("Ask about Biometrics", "I Don't Know?");
                break;
            case PlayerProgression.Step4_AcquireTravelDocument:
                SetButtons("Ask about Travel Document", "I Don't Know?");
                break;
            case PlayerProgression.Step5_AcquireFirstInterview:
                SetButtons("Ask about First Interview", "I Don't Know?");
                break;
            case PlayerProgression.CompletedApplication:
                SetButtons("What's Next?", "Thank You");
                break;
            default:
                SetButtons("Ask for Help", "I Don't Know?");
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
            message = $"You�ll find your {neededDoc} at the {correctLocation}.";
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
            message = $"Alright... You�ll find your {neededDoc} at the {correctLocation}.";
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
            PlayerProgression.Step1_AcquireAsylumApplicationForm => DocumentType.AsylumApplicationFormDHA1590,
            PlayerProgression.Step2_AcquireID => DocumentType.ID,
            PlayerProgression.Step3_AcquireBiometrics => DocumentType.Biometrics,
            PlayerProgression.Step4_AcquireTravelDocument => DocumentType.TravelDocument,
            PlayerProgression.Step5_AcquireFirstInterview => DocumentType.FirstInterview,
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

    string GetResponseForButton(string text)
    {
        // Provide feedback based on the current progression step
        switch (GameState.Instance.playerProgression)
        {
            case PlayerProgression.Step1_AcquireAsylumApplicationForm:
                if (text == "Ask about Asylum Application Form")
                    return "You need to get the Asylum Application Form (DHA-1590).";
                break;
            case PlayerProgression.Step2_AcquireID:
                if (text == "Ask about ID")
                    return "You need to get your ID document.";
                break;
            case PlayerProgression.Step3_AcquireBiometrics:
                if (text == "Ask about Biometrics")
                    return "You need to complete your biometrics (fingerprints and ID photos).";
                break;
            case PlayerProgression.Step4_AcquireTravelDocument:
                if (text == "Ask about Travel Document")
                    return "You need to get your Travel Document.";
                break;
            case PlayerProgression.Step5_AcquireFirstInterview:
                if (text == "Ask about First Interview")
                    return "You need to complete your First Interview and submit proof.";
                break;
            case PlayerProgression.CompletedApplication:
                if (text == "What's Next?")
                    return "You'll be contacted by an officer for next steps.";
                if (text == "Thank You")
                    return "You're welcome. Good luck.";
                break;
            default:
                break;
        }
        if (text == "I Don't Know?")
            return "Please come back when you're ready.";
        return "I don't understand your request.";
    }

    OfficerData GetRandomOfficer()
    {
        if (officers.Count == 0) return null;
        return officers[Random.Range(0, officers.Count)];
    }

}
