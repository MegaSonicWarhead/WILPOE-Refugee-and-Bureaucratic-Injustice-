using UnityEngine;

public class OfficerManager_Bank : MonoBehaviour
{
    [Header("Bank Clerk Data")]
    public OfficerData bankClerk;

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

    public void OpenClerkPanel()
    {
        officerPanel.SetActive(true);
        officerImage.sprite = bankClerk.officerImage;
        officerNameText.text = bankClerk.officerName;
        responseText.text = bankClerk.initialGreeting;
        SetActionButtonTextsByProgression();
    }

    void SetActionButtonTextsByProgression()
    {
        switch (GameState.Instance.playerProgression)
        {
            case PlayerProgression.Step3_AcquireBiometrics:
                SetButtons("Ask about Biometrics", "I Don't Know?");
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
        responseText.text = $"{bankClerk.officerName}: {GetResponseForButton(text)}";
    }

    void GiveDocumentClue()
    {
        // Implement as needed for Bank
        responseText.text = $"{bankClerk.officerName}: You can complete your biometrics here.";
    }

    string GetResponseForButton(string text)
    {
        switch (GameState.Instance.playerProgression)
        {
            case PlayerProgression.Step3_AcquireBiometrics:
                if (text == "Ask about Biometrics")
                    return "You need to complete your biometrics. Please proceed to the fingerprint scanner.";
                break;
            default:
                break;
        }
        if (text == "I Don't Know?")
            return "Please come back when you're ready.";
        return "I don't understand your request.";
    }
}