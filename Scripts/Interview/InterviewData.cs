using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Interview Data", menuName = "Interview/InterviewData")]
public class InterviewData : ScriptableObject
{
    public List<InterviewDialog> dialogs = new();

    [Space(3)]
    public GameObject suspectModelPrefab;
}
