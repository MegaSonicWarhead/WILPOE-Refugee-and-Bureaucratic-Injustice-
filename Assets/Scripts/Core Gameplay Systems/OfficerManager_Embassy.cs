using UnityEngine;

public class OfficerManager_Embassy : MonoBehaviour
{
    [Header("Embassy Employee Data")]
    public OfficerData embassyEmployee;

    [Header("UI Elements")]
    public UnityEngine.UI.Image officerImage;
    public TMPro.TMP_Text officerNameText;
    public TMPro.TMP_Text responseText;
    public UnityEngine.UI.Button actionButton1;
    public UnityEngine.UI.Button actionButton2;
    public UnityEngine.UI.Button helpButton;
    public UnityEngine.UI.Button leaveButton;
    public GameObject officerPanel;

    private void Start()
    {
        officerPanel.SetActive(false);
        actionButton1.onClick.AddListener(() => HandleResponse(1));
        actionButton2.onClick.AddListener(() => HandleResponse(2));
        helpButton.onClick.AddListener(GiveDocumentClue);
        leaveButton.onClick.AddListener(ClosePanel);
    }

    void ClosePanel() => officerPanel.SetActive(false);

    public void OpenEmployeePanel()
    {
        officerPanel.SetActive(true);
        officerImage.sprite = embassyEmployee.officerImage;
        officerNameText.text = embassyEmployee.officerName;
        responseText.text = embassyEmployee.initialGreeting;
        SetActionButtonTextsByProgression();
    }

    void SetActionButtonTextsByProgression()
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

    void SetButtons(string text1, string text2)
    {
        actionButton1.GetComponentInChildren<TMPro.TMP_Text>().text = text1;
        actionButton2.GetComponentInChildren<TMPro.TMP_Text>().text = text2;
    }

    void HandleResponse(int option)
    {
        string text = option == 1
            ? actionButton1.GetComponentInChildren<TMPro.TMP_Text>().text
            : actionButton2.GetComponentInChildren<TMPro.TMP_Text>().text;
        responseText.text = $"{embassyEmployee.officerName}: {GetResponseForButton(text)}";
    }

    void GiveDocumentClue()
    {
        // Implement as needed for Embassy
        responseText.text = $"{embassyEmployee.officerName}: You can get your Asylum Application Form and complete your First Interview here.";
    }

    string GetResponseForButton(string text)
    {
        switch (GameState.Instance.playerProgression)
        {
            case PlayerProgression.Step1_AcquireAsylumApplicationForm:
                if (text == "Ask about Asylum Application Form")
                    return "You need to get the Asylum Application Form (DHA-1590). Please speak to the front desk.";
                break;
            case PlayerProgression.Step5_AcquireFirstInterview:
                if (text == "Ask about First Interview")
                    return "You need to complete your First Interview. Please wait for your name to be called.";
                break;
            default:
                break;
        }
        if (text == "I Don't Know?")
            return "Please come back when you're ready.";
        return "I don't understand your request.";
    }
}