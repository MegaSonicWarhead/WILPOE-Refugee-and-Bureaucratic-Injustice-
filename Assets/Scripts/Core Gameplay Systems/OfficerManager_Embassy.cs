using UnityEngine;
using UnityEngine.UI;

public class OfficerManager_Embassy : MonoBehaviour
{
    [Header("Embassy Employee Data")]
    public OfficerData embassyEmployee;

    [Header("UI Elements")]
    public Image officerImage;
    public TMPro.TMP_Text officerNameText;
    public TMPro.TMP_Text responseText;
    public Button actionButton1;
    public Button actionButton2;
    public Button giveAsylumApplicationFormButton;
    public Button giveFirstInterviewButton;
    public Button giveWrongDocButton;
    public Button leaveButton;
    public Button getOfficerButton;
    public GameObject officerPanel;
    public GameObject NotifyPanel;               // Panel with NotificationText
    public TMPro.TMP_Text NotificationText;      // Reference to the text field

    private void Start()
    {
        officerPanel.SetActive(false);

        getOfficerButton.onClick.AddListener(OpenEmployeePanel);
        leaveButton.onClick.AddListener(ClosePanel);

        actionButton1.onClick.AddListener(() => HandleResponse(1));
        actionButton2.onClick.AddListener(() => HandleResponse(2));

        giveAsylumApplicationFormButton.onClick.AddListener(GiveCorrectDocument);
        if (giveFirstInterviewButton != null)
            giveFirstInterviewButton.onClick.AddListener(GiveFirstInterview);
        giveWrongDocButton.onClick.AddListener(GiveWrongDocument);
    }

    private void ClosePanel() => officerPanel.SetActive(false);

    public void OpenEmployeePanel()
    {
        officerPanel.SetActive(true);
        officerImage.sprite = embassyEmployee.officerImage;
        officerNameText.text = embassyEmployee.officerName;
        responseText.text = embassyEmployee.initialGreeting;

        SetActionButtonTextsByProgression();

        // Allow getting Asylum Application Form if progression is None or Step1, and player doesn't already have it
        bool canGetAsylumForm = (GameState.Instance.playerProgression == PlayerProgression.None || 
                                 GameState.Instance.playerProgression == PlayerProgression.Step1_AcquireAsylumApplicationForm) &&
                                !GameState.Instance.HasDocument(DocumentType.AsylumApplicationFormDHA1590);
        
        if (canGetAsylumForm)
            giveAsylumApplicationFormButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Pay R20 to get Asylum Application Form";
        else
            giveAsylumApplicationFormButton.GetComponentInChildren<TMPro.TMP_Text>().text = "No Document Available";

        // Allow getting First Interview if progression is Step5, and player doesn't already have it
        if (giveFirstInterviewButton != null)
        {
            bool canGetFirstInterview = GameState.Instance.playerProgression == PlayerProgression.Step5_AcquireFirstInterview &&
                                        !GameState.Instance.HasDocument(DocumentType.FirstInterview);
            
            if (canGetFirstInterview)
                giveFirstInterviewButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Pay R20 to get First Interview";
            else
                giveFirstInterviewButton.GetComponentInChildren<TMPro.TMP_Text>().text = "No Document Available";
        }
    }

    private void SetActionButtonTextsByProgression()
    {
        switch (GameState.Instance.playerProgression)
        {
            case PlayerProgression.Step1_AcquireAsylumApplicationForm:
                SetButtons("Ask about Asylum Application Form", "I Don't Know?");
                break;
            case PlayerProgression.Step5_AcquireFirstInterview:
                SetButtons("Ask about First Interview", "I Don't Know?");
                break;
            default:
                SetButtons("Ask for Help", "I Don't Know?");
                break;
        }
    }

    private void SetButtons(string text1, string text2)
    {
        actionButton1.GetComponentInChildren<TMPro.TMP_Text>().text = text1;
        actionButton2.GetComponentInChildren<TMPro.TMP_Text>().text = text2;
    }

    private void HandleResponse(int option)
    {
        string selectedText = option == 1
            ? actionButton1.GetComponentInChildren<TMPro.TMP_Text>().text
            : actionButton2.GetComponentInChildren<TMPro.TMP_Text>().text;

        responseText.text = $"{embassyEmployee.officerName}: {GetResponseForButton(selectedText)}";
    }

    private void GiveCorrectDocument()
    {
        // ✅ Payment check
        if (!MoneySystem.Instance.SpendMoney(20))
        {
            if (NotificationText != null)
                NotificationText.text = "You need R20 to request this document.";
            return;
        }

        // Check if player can get Asylum Application Form (progression is None or Step1, and doesn't already have it)
        bool canGetAsylumForm = (GameState.Instance.playerProgression == PlayerProgression.None || 
                                 GameState.Instance.playerProgression == PlayerProgression.Step1_AcquireAsylumApplicationForm) &&
                                !GameState.Instance.HasDocument(DocumentType.AsylumApplicationFormDHA1590);

        if (canGetAsylumForm)
        {
            var docItemData = DocumentDatabase.Instance.GetItemDataForDocument(DocumentType.AsylumApplicationFormDHA1590);
            if (docItemData != null)
            {
                InventoryManager.Instance.AddItem(docItemData);
                // Note: Document is only "acquired" when given to Home Affairs officer, not when obtained here
                responseText.text = $"{embassyEmployee.officerName}: Here you go, one Asylum Application Form (DHA-1590).";

                if (NotificationText != null)
                    NotificationText.text = "You received the Asylum Application Form.";
                Debug.Log("[Embassy] Asylum Application Form added to inventory.");
            }
            else
            {
                Debug.LogError("AsylumApplicationFormDHA1590 ScriptableObject not found in DocumentDatabase!");
                responseText.text = $"{embassyEmployee.officerName}: I'm sorry, we're out of forms right now.";
            }
        }
        else
        {
            if (GameState.Instance.HasDocument(DocumentType.AsylumApplicationFormDHA1590))
                responseText.text = $"{embassyEmployee.officerName}: You already have the Asylum Application Form.";
            else
                responseText.text = $"{embassyEmployee.officerName}: You already have what you need from me.";
        }
    }

    private void GiveFirstInterview()
    {
        // ✅ Payment check
        if (!MoneySystem.Instance.SpendMoney(20))
        {
            if (NotificationText != null)
                NotificationText.text = "You need R20 to request this document.";
            return;
        }

        // Check if player can get First Interview (progression is Step5, and doesn't already have it)
        bool canGetFirstInterview = GameState.Instance.playerProgression == PlayerProgression.Step5_AcquireFirstInterview &&
                                    !GameState.Instance.HasDocument(DocumentType.FirstInterview);

        if (canGetFirstInterview)
        {
            var docItemData = DocumentDatabase.Instance.GetItemDataForDocument(DocumentType.FirstInterview);
            if (docItemData != null)
            {
                InventoryManager.Instance.AddItem(docItemData);
                // Note: Document is only "acquired" when given to Home Affairs officer, not when obtained here
                responseText.text = $"{embassyEmployee.officerName}: Your First Interview has been completed. Here is your proof of completion.";

                if (NotificationText != null)
                    NotificationText.text = "You received the First Interview document.";
                Debug.Log("[Embassy] First Interview document added to inventory.");
            }
            else
            {
                Debug.LogError("FirstInterview ScriptableObject not found in DocumentDatabase!");
                responseText.text = $"{embassyEmployee.officerName}: I'm sorry, there was an issue processing your interview.";
            }
        }
        else
        {
            if (GameState.Instance.HasDocument(DocumentType.FirstInterview))
                responseText.text = $"{embassyEmployee.officerName}: You already have the First Interview document.";
            else
                responseText.text = $"{embassyEmployee.officerName}: You're not ready for your First Interview yet.";
        }
    }

    private void GiveWrongDocument()
    {
        // ✅ Payment check
        if (!MoneySystem.Instance.SpendMoney(20))
        {
            if (NotificationText != null)
                NotificationText.text = "You need R20 to request this document.";
            return;
        }

        responseText.text = $"{embassyEmployee.officerName}: This is not the correct document. Please check again.";
        if (NotificationText != null)
            NotificationText.text = "You received a document.";
    }

    private string GetResponseForButton(string text)
    {
        switch (GameState.Instance.playerProgression)
        {
            case PlayerProgression.Step1_AcquireAsylumApplicationForm:
                if (text == "Ask about Asylum Application Form")
                    return "You need to get the Asylum Application Form (DHA-1590). Please speak to the front desk.";
                break;

            case PlayerProgression.Step5_AcquireFirstInterview:
                if (text == "Ask about First Interview")
                    return "Your First Interview is scheduled here. Please wait for your name to be called.";
                break;
        }

        if (text == "I Don't Know?")
            return "Please come back when you're ready.";

        return "You can get your Asylum Application Form and attend your First Interview here.";
    }
}