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

    private void Start()
    {
        officerPanel.SetActive(false);

        getOfficerButton.onClick.AddListener(AssignOfficer);
        actionButton1.onClick.AddListener(() => HandleResponse(1));
        actionButton2.onClick.AddListener(() => HandleResponse(2));
        helpButton.onClick.AddListener(HandleHelpRequest);
        leaveButton.onClick.AddListener(ClosePanel);
    }

    void ClosePanel()
    {
        officerPanel.SetActive(false);
    }

    void AssignOfficer()
    {
        currentOfficer = GetRandomOfficer();

        if (currentOfficer == null)
        {
            Debug.LogWarning("No valid officer was assigned.");
            return;
        }

        officerPanel.SetActive(true);
        UpdateOfficerUI();
        responseText.text = currentOfficer.initialGreeting;
        SetActionButtonTextsByProgression();
    }

    void UpdateOfficerUI()
    {
        if (currentOfficer != null)
        {
            officerImage.sprite = currentOfficer.officerImage;
            officerNameText.text = currentOfficer.officerName;
        }
    }

    void SetActionButtonTextsByProgression()
    {
        switch (GameState.Instance.playerProgression)
        {
            case PlayerProgression.FirstVisit:
                actionButton1.GetComponentInChildren<TMP_Text>().text = "Temporary Permit";
                actionButton2.GetComponentInChildren<TMP_Text>().text = "I Don't Know?";
                break;
            case PlayerProgression.AskedPermit:
                actionButton1.GetComponentInChildren<TMP_Text>().text = "I Have Some Documents";
                actionButton2.GetComponentInChildren<TMP_Text>().text = "I Need Help";
                break;
            case PlayerProgression.HasDocuments:
                actionButton1.GetComponentInChildren<TMP_Text>().text = "Submit Application";
                actionButton2.GetComponentInChildren<TMP_Text>().text = "Missing Something?";
                break;
            case PlayerProgression.CompletedApplication:
                actionButton1.GetComponentInChildren<TMP_Text>().text = "What's Next?";
                actionButton2.GetComponentInChildren<TMP_Text>().text = "Thank You";
                break;
        }
    }

    void HandleResponse(int option)
    {
        if (currentOfficer == null) return;

        string buttonText = option == 1
            ? actionButton1.GetComponentInChildren<TMP_Text>().text
            : actionButton2.GetComponentInChildren<TMP_Text>().text;

        string response = GetResponseForButton(buttonText);
        responseText.text = $"{currentOfficer.officerName}: {response}";
    }

    void HandleHelpRequest()
    {
        if (currentOfficer == null) return;

        string helpResponse = GetHelpResponseByOfficerType(currentOfficer.officerType);
        responseText.text = $"{currentOfficer.officerName}: {helpResponse}";
    }

    string GetHelpResponseByOfficerType(OfficerType type)
    {
        switch (type)
        {
            case OfficerType.Nice:
                return "You can get the forms at the refugee center front desk. They're free of charge.";
            case OfficerType.Difficult:
                return "It’s not my job to tell you. Figure it out yourself.";
            case OfficerType.Lazy:
                return "Try that kiosk... or maybe ask someone else, I dunno.";
            case OfficerType.Corrupt:
                return "I *might* help... for a little favor, if you know what I mean.";
            default:
                return "I’m not sure where to find that.";
        }
    }

    string GetResponseForButton(string buttonText)
    {
        switch (buttonText)
        {
            case "Temporary Permit":
                GameState.Instance.playerProgression = PlayerProgression.AskedPermit;
                return "You need Form DHA-1590, ID, travel doc, biometrics, and a first interview.";
            case "I Don't Know?":
                return "Please come back when you're ready.";
            case "I Have Some Documents":
                GameState.Instance.playerProgression = PlayerProgression.HasDocuments;
                return "Great. Let me check what you have.";
            case "I Need Help":
                return "Ask specific questions or look at the checklist.";
            case "Submit Application":
                GameState.Instance.playerProgression = PlayerProgression.CompletedApplication;
                return "Application submitted. Now wait for a response.";
            case "Missing Something?":
                return "Make sure you have all required documents.";
            case "What's Next?":
                return "You’ll be contacted by an officer for next steps.";
            case "Thank You":
                return "You're welcome. Good luck.";
            default:
                return "I don't understand your request.";
        }
    }

    OfficerData GetRandomOfficer()
    {
        float rand = Random.value;

        OfficerType selectedType;

        if (rand < 0.10f)
            selectedType = OfficerType.Nice;
        else if (rand < 0.40f)
            selectedType = OfficerType.Difficult;
        else if (rand < 0.70f)
            selectedType = OfficerType.Lazy;
        else
            selectedType = OfficerType.Corrupt;

        List<OfficerData> filtered = officers.FindAll(o => o.officerType == selectedType);

        if (filtered.Count > 0)
        {
            return filtered[Random.Range(0, filtered.Count)];
        }
        else
        {
            Debug.LogWarning($"No officers of type {selectedType} found. Choosing any available officer.");
            if (officers.Count > 0)
                return officers[Random.Range(0, officers.Count)];
            else
            {
                Debug.LogError("No officers available at all!");
                return null;
            }
        }
    }
}
