using UnityEngine;

[CreateAssetMenu(fileName = "Interview Dialog", menuName = "Interview/InterviewDialog")]
public class InterviewDialog : ScriptableObject
{
    public AudioClip audio;

    [TextArea]
    public string subtitleLine;

    [Space(3)]
    [Tooltip("The layer level shows in step of the interview the dialog option is shown")]
    public int layerLevel = 0;

    [Space(3)]
    [Tooltip("The text of the button which will trigger this dialog")]
    public string dialogButtonText;
}