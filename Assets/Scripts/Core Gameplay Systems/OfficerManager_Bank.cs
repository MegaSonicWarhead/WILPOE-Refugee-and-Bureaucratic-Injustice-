using UnityEngine;
using UnityEngine.UI;

public class OfficerManager_Bank : MonoBehaviour
{
    [Header("Bank Clerk Data")]
    public OfficerData bankClerk;

    [Header("UI Elements")]
    public Image officerImage;
    public TMPro.TMP_Text officerNameText;
    public TMPro.TMP_Text responseText;
    public Button actionButton1;
    public Button actionButton2;
    public Button GivebiometricsButton;
    public Button GiveWrongDocButton;
    public Button leaveButton;
    public Button getOfficerButton;
    public GameObject officerPanel;
    public GameObject NotifyPanel; // Panel with NotificationText
    public TMPro.TMP_Text NotificationText; // Reference to the text field

    private void Start()
    {
        getOfficerButton.onClick.AddListener(OpenClerkPanel);
        officerPanel.SetActive(false);

        actionButton1.onClick.AddListener(() => HandleResponse(1));
        actionButton2.onClick.AddListener(() => HandleResponse(2));
        GivebiometricsButton.onClick.AddListener(GiveCorrectDocument);
        GiveWrongDocButton.onClick.AddListener(GiveWrongDocument);
        leaveButton.onClick.AddListener(ClosePanel);

        GiveWrongDocButton.interactable = false;
        GiveWrongDocButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Give Wrong Document";
    }

    void ClosePanel() => officerPanel.SetActive(false);

    public void OpenClerkPanel()
    {
        officerPanel.SetActive(true);
        officerImage.sprite = bankClerk.officerImage;
        officerNameText.text = bankClerk.officerName;
        responseText.text = bankClerk.initialGreeting;

        SetActionButtonTextsByProgression();

        if (!string.IsNullOrEmpty(OfficerManager.LastWrongDocument))
        {
            GiveWrongDocButton.interactable = true;
            GiveWrongDocButton.GetComponentInChildren<TMPro.TMP_Text>().text =
                $"Pay R20 to get {OfficerManager.LastWrongDocument}";
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

    void GiveCorrectDocument()
    {
        // Payment check
        if (!MoneySystem.Instance.SpendMoney(20))
        {
            if (NotificationText != null)
                NotificationText.text = "You need R20 to request this document.";
            return;
        }

        var docItemData = DocumentDatabase.Instance.GetItemDataForDocument(DocumentType.Biometrics);
        if (docItemData != null)
        {
            InventoryManager.Instance.AddItem(docItemData);
            GameState.Instance.AcquireDocument(DocumentType.Biometrics);

            responseText.text = $"{bankClerk.officerName}: Here is your Biometrics document.";
            if (NotificationText != null)
                NotificationText.text = "You received the Biometrics document.";
            Debug.Log("[Bank Clerk] Biometrics document added to inventory.");
        }
        else
        {
            Debug.LogError("Biometrics ScriptableObject not found in DocumentDatabase!");
            responseText.text = $"{bankClerk.officerName}: Hmm, I can’t seem to find your biometrics record right now.";
        }
    }

    void GiveWrongDocument()
    {
        // Payment check
        if (!MoneySystem.Instance.SpendMoney(20))
        {
            if (NotificationText != null)
                NotificationText.text = "You need R20 to request this document.";
            return;
        }

        string wrongDoc = OfficerManager.LastWrongDocument ?? "some random document";
        responseText.text = $"{bankClerk.officerName}: Here is the {wrongDoc} you asked for.";
        if (NotificationText != null)
            NotificationText.text = "You received a document.";
    }

    string GetResponseForButton(string text)
    {
        switch (GameState.Instance.playerProgression)
        {
            case PlayerProgression.Step3_AcquireBiometrics:
                if (text == "Ask about Biometrics")
                    return "You need to complete your biometrics. Please proceed to the fingerprint scanner.";
                break;
        }

        if (text == "I Don't Know?")
            return "Please come back when you're ready.";

        return "You can complete your biometrics here.";
    }
}