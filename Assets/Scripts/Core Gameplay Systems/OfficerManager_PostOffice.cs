using UnityEngine;
using UnityEngine.UI;

public class OfficerManager_PostOffice : MonoBehaviour
{
    [Header("Post Office Clerk Data")]
    public OfficerData postOfficeClerk;

    [Header("UI Elements")]
    public UnityEngine.UI.Image officerImage;
    public TMPro.TMP_Text officerNameText;
    public TMPro.TMP_Text responseText;
    public UnityEngine.UI.Button actionButton1;
    public UnityEngine.UI.Button actionButton2;
    public UnityEngine.UI.Button helpButton;
    public UnityEngine.UI.Button leaveButton;
    public GameObject officerPanel;
    public Button getOfficerButton;

    private void Start()
    {
        getOfficerButton.onClick.AddListener(OpenClerkPanel);
        officerPanel.SetActive(false);
        actionButton1.onClick.AddListener(() => HandleResponse(1));
        actionButton2.onClick.AddListener(() => HandleResponse(2));
        helpButton.onClick.AddListener(GiveDocumentClue);
        leaveButton.onClick.AddListener(ClosePanel);
    }

    void ClosePanel() => officerPanel.SetActive(false);

    public void OpenClerkPanel()
    {
        officerPanel.SetActive(true);
        officerImage.sprite = postOfficeClerk.officerImage;
        officerNameText.text = postOfficeClerk.officerName;
        responseText.text = postOfficeClerk.initialGreeting;
        SetActionButtonTextsByProgression();
    }

    void SetActionButtonTextsByProgression()
    {
        switch (GameState.Instance.playerProgression)
        {
            case PlayerProgression.Step2_AcquireID:
                SetButtons("Ask about ID", "I Don't Know?");
                break;
            case PlayerProgression.Step4_AcquireTravelDocument:
                SetButtons("Ask about Travel Document", "I Don't Know?");
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
        responseText.text = $"{postOfficeClerk.officerName}: {GetResponseForButton(text)}";
    }

    void GiveDocumentClue()
    {
        // Implement as needed for Post Office
        responseText.text = $"{postOfficeClerk.officerName}: You can get your ID and Travel Document here.";
    }

    string GetResponseForButton(string text)
    {
        switch (GameState.Instance.playerProgression)
        {
            case PlayerProgression.Step2_AcquireID:
                if (text == "Ask about ID")
                    return "You need to get your ID document. Please fill out the form at the counter.";
                break;
            case PlayerProgression.Step4_AcquireTravelDocument:
                if (text == "Ask about Travel Document")
                    return "You need to get your Travel Document. Please provide your ID and fill out the application.";
                break;
            default:
                break;
        }
        if (text == "I Don't Know?")
            return "Please come back when you're ready.";
        return "I don't understand your request.";
    }
}