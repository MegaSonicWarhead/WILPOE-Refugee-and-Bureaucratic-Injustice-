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

        if (GameState.Instance.playerProgression == PlayerProgression.Step1_AcquireAsylumApplicationForm)
            giveAsylumApplicationFormButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Pay R20 to get Asylum Application Form";
        else
            giveAsylumApplicationFormButton.GetComponentInChildren<TMPro.TMP_Text>().text = "No Document Available";
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

        if (GameState.Instance.playerProgression == PlayerProgression.Step1_AcquireAsylumApplicationForm)
        {
            GameState.Instance.AcquireDocument(DocumentType.AsylumApplicationFormDHA1590);
            responseText.text = $"{embassyEmployee.officerName}: Here you go, one Asylum Application Form (DHA-1590).";

            if (NotificationText != null)
                NotificationText.text = "You received the Asylum Application Form.";
        }
        else
        {
            responseText.text = $"{embassyEmployee.officerName}: You already have what you need from me.";
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