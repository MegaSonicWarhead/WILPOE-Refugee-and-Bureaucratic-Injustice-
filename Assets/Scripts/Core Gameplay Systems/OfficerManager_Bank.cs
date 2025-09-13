using UnityEngine;
using UnityEngine.UI;

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
    public UnityEngine.UI.Button GivebiometricsButton;
    public UnityEngine.UI.Button GiveWrongDocButton;
    public UnityEngine.UI.Button leaveButton;
    public Button getOfficerButton;
    public GameObject officerPanel;

    private void Start()
    {
        getOfficerButton.onClick.AddListener(OpenClerkPanel);
        officerPanel.SetActive(false);

        actionButton1.onClick.AddListener(() => HandleResponse(1));
        actionButton2.onClick.AddListener(() => HandleResponse(2));
        GivebiometricsButton.onClick.AddListener(GiveCorrectDocument);
        GiveWrongDocButton.onClick.AddListener(GiveWrongDocument);
        leaveButton.onClick.AddListener(ClosePanel);

        // Ensure WrongDoc button is hidden/disabled at start
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

        // ✅ Update Wrong Doc button text with the last wrong document (if available)
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
        responseText.text = $"{bankClerk.officerName}: Here is your Fingerprints document.";
        var docItemData = DocumentDatabase.Instance.GetItemDataForDocument(DocumentType.Biometrics);
        if (docItemData != null)
        {
            InventoryManager.Instance.AddItem(docItemData); // ✅ Add to inventory
            GameState.Instance.AcquireDocument(DocumentType.Biometrics); // ✅ Update progression

            responseText.text = $"{bankClerk.officerName}: Here is your Biometrics document.";
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
        // ✅ Use the exact wrong document chosen in OfficerManager
        string wrongDoc = OfficerManager.LastWrongDocument ?? "some random document";
        responseText.text = $"{bankClerk.officerName}: Here is the {wrongDoc} you asked for.";
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