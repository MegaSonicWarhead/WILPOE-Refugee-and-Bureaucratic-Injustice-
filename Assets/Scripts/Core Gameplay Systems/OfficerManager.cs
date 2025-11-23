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
    public Button bribeButton;
    public Button noButton;
    public Button giveDocumentButton;
    public GameObject officerPanel;

    private OfficerData currentOfficer;
    private DocumentType pendingBribeDocument;
    private string pendingBribeLocation;
    private bool awaitingBribeChoice = false;
    private bool corruptOfficerAskedForHelp = false;

    public static string LastWrongDocument { get; private set; }

    private string[] wrongDocumentAnswers = new string[]
    {
        "BI-947 form: Application form for refugee status",
        "Valid passport",
        "Application for certification (BI 1754)",
        "Proof of 10 years continuous refugee status",
        "Valid refugee status document (Section 24 visa)",
        "Information about dependents",
        "Certified copies of passports",
        "Police affidavit",
        "Marriage certificate",
        "Proof of parental responsibilities",
        "Birth certificates: For yourself and any dependents",
        "Police clearance certificate",
        "Yellow fever vaccination certificate"
    };

    private void Start()
    {
        officerPanel.SetActive(false);
        getOfficerButton.onClick.AddListener(AssignOfficer);
        actionButton1.onClick.AddListener(() => HandleResponse(1));
        actionButton2.onClick.AddListener(() => HandleResponse(2));
        helpButton.onClick.AddListener(GiveDocumentClue);
        leaveButton.onClick.AddListener(ClosePanel);

        if (giveDocumentButton != null)
            giveDocumentButton.onClick.AddListener(OnGiveDocumentClicked); // ✅ handle document hand-in
        if (bribeButton != null)
            bribeButton.onClick.AddListener(OnBribeButtonClicked);
        if (noButton != null)
            noButton.onClick.AddListener(OnNoButtonClicked);
    }

    void ClosePanel() => officerPanel.SetActive(false);

    void AssignOfficer()
    {
        currentOfficer = GetRandomOfficer();
        if (currentOfficer == null) return;

        officerPanel.SetActive(true);
        officerImage.sprite = currentOfficer.officerImage;
        officerNameText.text = currentOfficer.officerName;

        // 🎲 50% chance systems are down
        if (Random.value < 0.5f)
        {
            responseText.text = $"{currentOfficer.officerName}: Sorry, our systems are down. Please come back tomorrow.";

            // Disable all relevant buttons
            actionButton1.interactable = false;
            actionButton2.interactable = false;
            helpButton.interactable = false;
            bribeButton.interactable = false;
            noButton.interactable = false;
            if (giveDocumentButton != null)
                giveDocumentButton.interactable = false;

            return; // Exit early, skip normal setup
        }

        // Normal officer setup
        responseText.text = currentOfficer.initialGreeting;
        corruptOfficerAskedForHelp = false;
        SetActionButtonTextsByProgression();

        // Ensure buttons are re-enabled for normal case
        actionButton1.interactable = true;
        actionButton2.interactable = true;
        helpButton.interactable = true;
        bribeButton.interactable = true;
        noButton.interactable = true;
        if (giveDocumentButton != null)
            giveDocumentButton.interactable = true;
    }

    // -----------------------
    // ✅ New: Give Document Button
    // -----------------------
    void OnGiveDocumentClicked()
    {
        if (currentOfficer == null) return;

        // Determine which document is expected for current progression
        DocumentType neededDoc = GetExpectedDocumentForProgression();

        // Validate that the needed document is one of the correct progression documents
        if (!IsValidProgressionDocument(neededDoc))
        {
            responseText.text = $"{currentOfficer.officerName}: That document is not needed at this stage.";
            Debug.LogWarning($"[HomeAffairs] Invalid document type requested: {neededDoc}");
            return;
        }

        if (DocumentDatabase.Instance == null)
        {
            Debug.LogError("DocumentDatabase.Instance is null! Make sure it's in the scene.");
            return;
        }

        InventoryItemData docItemData = DocumentDatabase.Instance.GetItemDataForDocument(neededDoc);

        if (docItemData == null)
        {
            responseText.text = $"{currentOfficer.officerName}: I don't recognize that document.";
            Debug.LogError($"ScriptableObject for {neededDoc} not found in DocumentDatabase!");
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager.Instance is null! Make sure it's in the scene.");
            return;
        }

        // First check if document was already acquired/submitted
        if (GameState.Instance.HasDocument(neededDoc))
        {
            responseText.text = $"{currentOfficer.officerName}: You've already submitted this document. Your application is progressing.";
            Debug.Log($"[HomeAffairs] Player tried to hand in {neededDoc}, but it was already acquired.");
            return;
        }

        // Check if player has the item in inventory
        if (InventoryManager.Instance.HasItem(docItemData))
        {
            // Store current progression to check if it changes
            PlayerProgression previousProgression = GameState.Instance.playerProgression;
            Debug.Log($"[HomeAffairs] Before submitting {neededDoc}. Current progression: {previousProgression}");

            // Remove 1 from inventory
            InventoryManager.Instance.RemoveItem(docItemData);

            // Advance game progression (only updates if correct document)
            GameState.Instance.AcquireDocument(neededDoc);

            // Check if progression actually advanced
            PlayerProgression newProgression = GameState.Instance.playerProgression;
            if (newProgression != previousProgression)
            {
                // Update UI to reflect new progression
                SetActionButtonTextsByProgression();
                Debug.Log($"[HomeAffairs] Progression advanced from {previousProgression} to {newProgression}");
            }
            else
            {
                Debug.LogWarning($"[HomeAffairs] Progression did NOT advance! Still at {newProgression}. Document {neededDoc} may have already been acquired.");
            }

            // Feedback to player
            responseText.text = $"{currentOfficer.officerName}: Thank you for providing your {neededDoc}. Your application is moving forward!";
            Debug.Log($"[HomeAffairs] {neededDoc} handed in successfully. Final progression: {newProgression}");
        }
        else
        {
            responseText.text = $"{currentOfficer.officerName}: You don't have the correct document yet.";
            Debug.Log($"[HomeAffairs] Player tried to hand in {neededDoc}, but it wasn't in inventory.");
        }
    }

    // Validate that a document is one of the correct progression documents
    bool IsValidProgressionDocument(DocumentType doc)
    {
        return doc == DocumentType.AsylumApplicationFormDHA1590 ||
               doc == DocumentType.ID ||
               doc == DocumentType.Biometrics ||
               doc == DocumentType.TravelDocument ||
               doc == DocumentType.FirstInterview;
    }

    void SetActionButtonTextsByProgression()
    {
        switch (GameState.Instance.playerProgression)
        {
            case PlayerProgression.Step1_AcquireAsylumApplicationForm:
                SetButtons("  Ask about Asylum Application Form", "  I Don't Know?");
                break;
            case PlayerProgression.Step2_AcquireID:
                SetButtons("  Ask about ID", "  I Don't Know?");
                break;
            case PlayerProgression.Step3_AcquireBiometrics:
                SetButtons("  Ask about Biometrics", " I  Don't Know?");
                break;
            case PlayerProgression.Step4_AcquireTravelDocument:
                SetButtons("  Ask about Travel Document", "  I Don't Know?");
                break;
            case PlayerProgression.Step5_AcquireFirstInterview:
                SetButtons("  Ask about First Interview", "  I Don't Know?");
                break;
            case PlayerProgression.CompletedApplication:
                SetButtons("  What's Next?", "  Thank You");
                break;
            default:
                SetButtons("  Ask for Help", "  I Don't Know?");
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

        // Always get the current document needed based on current player progression
        // This reads directly from GameState.Instance.playerProgression, so it's always up-to-date
        DocumentType neededDoc = GetExpectedDocumentForProgression();
        string correctLocation = GetCorrectLocationForDocument(neededDoc);
        
        // Debug: Log current progression and needed document
        Debug.Log($"[HelpButton] Current progression: {GameState.Instance.playerProgression}, Needed document: {neededDoc}");

        // Check if a clue was already given for THIS specific document today
        //if (GameState.Instance.clueHistory.ContainsKey(neededDoc))
       // {
           // responseText.text = $"{currentOfficer.officerName}: I already helped you today.";
           // return;
       // }

        string message;

        if (currentOfficer.officerType == OfficerType.Nice)
        {
            // Nice officer gives correct document and location
            message = $"You'll find your {neededDoc} at the {correctLocation}.";
            GameState.Instance.clueHistory[neededDoc] = correctLocation;
            responseText.text = $"{currentOfficer.officerName}: {message}";
            return;
        }
        else if (currentOfficer.officerType == OfficerType.Corrupt)
        {
            // Corrupt officer starts bribe flow for the new document
            StartBribeFlow(neededDoc, correctLocation);
            corruptOfficerAskedForHelp = true;
            return;
        }
        else
        {
            // Other officers give wrong document from the array
            message = $"Maybe you need a {GetRandomWrongDocument()} from the {GetRandomLocation()}?";
            GameState.Instance.clueHistory[neededDoc] = correctLocation; // Track that clue was given (even if wrong)
            responseText.text = $"{currentOfficer.officerName}: {message}";
            return;
        }
    }

    void StartBribeFlow(DocumentType neededDoc, string correctLocation)
    {
        responseText.text = $"{currentOfficer.officerName}: I can give you that information if you make it worth my time. I think R100 sounds good. What do you think?";
        pendingBribeDocument = neededDoc;
        pendingBribeLocation = correctLocation;
        awaitingBribeChoice = true;
    }

    void OnBribeButtonClicked()
    {
        if (currentOfficer == null) return;

        DocumentType doc = awaitingBribeChoice ? pendingBribeDocument : GetExpectedDocumentForProgression();
        string location = awaitingBribeChoice ? pendingBribeLocation : GetCorrectLocationForDocument(doc);

        if (MoneySystem.Instance != null && MoneySystem.Instance.SpendMoney(100))
        {
            // Corrupt officer gives correct document and location after bribe
            string msg = $"Alright... You'll find your {doc} at the {location}.";
            GameState.Instance.clueHistory[doc] = location;
            responseText.text = $"{currentOfficer.officerName}: {msg}";
        }
        else
        {
            responseText.text = $"{currentOfficer.officerName}: You don't have enough money.";
        }

        awaitingBribeChoice = false;
        corruptOfficerAskedForHelp = false;
    }

    void OnNoButtonClicked()
    {
        if (currentOfficer == null) return;

        DocumentType doc = awaitingBribeChoice ? pendingBribeDocument : GetExpectedDocumentForProgression();
        string location = awaitingBribeChoice ? pendingBribeLocation : GetCorrectLocationForDocument(doc);

        if (currentOfficer.officerType == OfficerType.Corrupt && corruptOfficerAskedForHelp)
        {
            // Corrupt officer gives wrong document when bribe is refused
            string msg = $"Maybe you need a {GetRandomWrongDocument()} from the {GetRandomLocation()}?";
            GameState.Instance.clueHistory[doc] = location; // Track that clue was given (even if wrong)
            responseText.text = $"{currentOfficer.officerName}: {msg}";
            corruptOfficerAskedForHelp = false;
            awaitingBribeChoice = false;
            return;
        }

        string fallbackMsg = $"Maybe you need a {GetRandomWrongDocument()} from the {GetRandomLocation()}?";
        GameState.Instance.clueHistory[doc] = location; // Track that clue was given (even if wrong)
        responseText.text = $"{currentOfficer.officerName}: {fallbackMsg}";

        awaitingBribeChoice = false;
    }

    string GetRandomWrongDocument()
    {
        LastWrongDocument = wrongDocumentAnswers[Random.Range(0, wrongDocumentAnswers.Length)];
        return LastWrongDocument;
    }

    DocumentType GetExpectedDocumentForProgression()
    {
        // Always read directly from GameState to ensure we get the latest progression
        PlayerProgression currentProgression = GameState.Instance.playerProgression;
        
        return currentProgression switch
        {
            PlayerProgression.None => DocumentType.AsylumApplicationFormDHA1590, // Start with first document
            PlayerProgression.Step1_AcquireAsylumApplicationForm => DocumentType.AsylumApplicationFormDHA1590,
            PlayerProgression.Step2_AcquireID => DocumentType.ID,
            PlayerProgression.Step3_AcquireBiometrics => DocumentType.Biometrics,
            PlayerProgression.Step4_AcquireTravelDocument => DocumentType.TravelDocument,
            PlayerProgression.Step5_AcquireFirstInterview => DocumentType.FirstInterview,
            PlayerProgression.CompletedApplication => DocumentType.FirstInterview, // No more documents needed
            _ => DocumentType.AsylumApplicationFormDHA1590 // Default to first document
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
                if (text == "What's Next?") return "You'll be contacted by an officer for next steps.";
                if (text == "Thank You") return "You're welcome. Good luck.";
                break;
            default: break;
        }
        if (text == "I Don't Know?") return "Please come back when you're ready.";
        return "I understand that you are a refugee. In South Africa, refugees are required to have a temporary permit, which can only be obtained with the correct supporting documents.";
    }

    OfficerData GetRandomOfficer()
    {
        if (officers.Count == 0) return null;
        return officers[Random.Range(0, officers.Count)];
    }

}
