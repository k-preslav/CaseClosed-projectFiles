using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterviewUI : MonoBehaviour
{
    [SerializeField] GameObject interviewOverlay;

    [Space(3)]
    [SerializeField] Transform optionsBox;
    [SerializeField] GameObject optionButtonPrefab;

    public void OpenInterviewUI()
    {
        interviewOverlay.SetActive(true);

        GameManager.Instance.Cursor.SetVisible(true);
        GameManager.Instance.Cursor.SetLock(false);
    }
    public void CloseInterviewUI()
    {
        interviewOverlay.SetActive(false);

        GameManager.Instance.Cursor.SetVisible(false);
        GameManager.Instance.Cursor.SetLock(true);
    }

    public void CreateOptionButton(InterviewDialog linkedDialog)
    {
        var butnObj = Instantiate(optionButtonPrefab, optionsBox);
        var butnComp = butnObj.GetComponent<Button>();

        butnComp.onClick.AddListener(() =>
        {
            FindAnyObjectByType<InterviewManager>().StartDialog(linkedDialog);
        });
        butnComp.GetComponentInChildren<TMP_Text>().text = linkedDialog.dialogButtonText;
    }

    public void ClearOptionButtons()
    {
        foreach (Transform child in optionsBox)
        {
            Destroy(child.gameObject);
        }
    }
}
