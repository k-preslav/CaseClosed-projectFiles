using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InterviewManager : MonoBehaviour
{
    [SerializeField] InterviewUI interviewUI;
    [SerializeField] AudioSource interviewAudioSource;

    InterviewData currentInterview;

    int currentDialogLevel = 0;
    List<InterviewDialog> usedDialogs = new List<InterviewDialog>();

    private void Start()
    {
        currentInterview = GameManager.Instance.selectedCardData.linkedInterviewData;
        GameManager.Instance.SceneTransitionUI.FadeOut();

        ShowNextDialogOptions();
    }

    void ShowNextDialogOptions()
    {
        List<InterviewDialog> dialogsWithCurrentLevel = currentInterview.dialogs
            .FindAll(d => d.layerLevel == currentDialogLevel);

        // Increment level only if all dialogs for the current layer level have been used
        if (dialogsWithCurrentLevel.Count == usedDialogs.FindAll(d => d.layerLevel == currentDialogLevel).Count)
        {
            currentDialogLevel++;

            // refetch dialogs for the new current level
            dialogsWithCurrentLevel = currentInterview.dialogs
                .FindAll(d => d.layerLevel == currentDialogLevel);
        }

        interviewUI.ClearOptionButtons();

        foreach (var dialog in dialogsWithCurrentLevel)
        {
            if (usedDialogs.Contains(dialog)) 
                continue;

            interviewUI.CreateOptionButton(dialog);
        }

        interviewUI.OpenInterviewUI();
    }

    public void StartDialog(InterviewDialog dialog)
    {
        usedDialogs.Add(dialog);
        interviewUI.CloseInterviewUI();

        interviewAudioSource.clip = dialog.audio;
        interviewAudioSource.loop = false;
        interviewAudioSource.Play();

        StartCoroutine(WaitForAudioToFinish());
    }

    public void EndInterview()
    {
        GameManager.Instance.completedInterviews.Add(currentInterview);

        interviewUI.CloseInterviewUI();
        GameManager.Instance.GoToBoard();
    }

    IEnumerator WaitForAudioToFinish()
    {
        yield return new WaitWhile(() => interviewAudioSource.isPlaying);
        OnAudioFinished();
    }

    void OnAudioFinished()
    {
        ShowNextDialogOptions();
        interviewUI.OpenInterviewUI();
    }
}