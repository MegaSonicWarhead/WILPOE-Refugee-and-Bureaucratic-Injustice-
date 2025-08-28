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
    public UnityEngine.UI.Button Give_ID_documentButton;
    public UnityEngine.UI.Button GiveTravel_DocumentButton;
    public UnityEngine.UI.Button GiveWrongDocButton;
    public UnityEngine.UI.Button leaveButton;
    public GameObject officerPanel;
    public Button getOfficerButton;

    private void Start()
    {
        getOfficerButton.onClick.AddListener(OpenClerkPanel);
        officerPanel.SetActive(false);

        actionButton1.onClick.AddListener(() => HandleResponse(1));
        actionButton2.onClick.AddListener(() => HandleResponse(2));
        Give_ID_documentButton.onClick.AddListener(GiveFirstCorrectDocument);
        GiveTravel_DocumentButton.onClick.AddListener(GiveSecondCorrectDocument);
        GiveWrongDocButton.onClick.AddListener(GiveWrongDocument);
        leaveButton.onClick.AddListener(ClosePanel);

        // ✅ Wrong Doc button starts disabled/with placeholder
        GiveWrongDocButton.interactable = false;
        GiveWrongDocButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Give Wrong Document";
    }

    void ClosePanel() => officerPanel.SetActive(false);

    public void OpenClerkPanel()
    {
        officerPanel.SetActive(true);
        officerImage.sprite = postOfficeClerk.officerImage;
        officerNameText.text = postOfficeClerk.officerName;
        responseText.text = postOfficeClerk.initialGreeting;

        SetActionButtonTextsByProgression();

        // ✅ Update Wrong Doc button dynamically
        if (!string.IsNullOrEmpty(OfficerManager.LastWrongDocument))
        {
            GiveWrongDocButton.interactable = true;
            GiveWrongDocButton.GetComponentInChildren<TMPro.TMP_Text>().text =
                $"Give {OfficerManager.LastWrongDocument}";
        }
        else
        {
            GiveWrongDocButton.interactable = false;
            GiveWrongDocButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Give Wrong Document";
        }
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

    void GiveFirstCorrectDocument()
    {
        responseText.text = $"{postOfficeClerk.officerName}: Here is your ID document.";
    }

    void GiveSecondCorrectDocument()
    {
        responseText.text = $"{postOfficeClerk.officerName}: Here is your Travel Document.";
    }

    void GiveWrongDocument()
    {
        // ✅ Pull wrong doc from OfficerManager
        string wrongDoc = OfficerManager.LastWrongDocument ?? "some random document";
        responseText.text = $"{postOfficeClerk.officerName}: Here is the {wrongDoc} you asked for.";
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
        }

        if (text == "I Don't Know?")
            return "Please come back when you're ready.";

        return "You can get your ID and Travel Document here.";
    }
}