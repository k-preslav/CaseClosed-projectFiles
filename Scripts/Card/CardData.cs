using UnityEngine;
using UnityEditor;

[System.Serializable]
public enum CardType
{
    GenericClue,
    Suspect,
    Location
}

[CreateAssetMenu(fileName = "Card Data", menuName = "Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    [TextArea] public string description;

    public CardType cardType;

    [Header("Location Card Settings")]
    //[SerializeField] private SceneAsset linkedSceneAsset; // Editor-only reference
    public string linkedSceneName; // Build-compatible scene name

    [Header("Suspect Card Settings")]
    public InterviewData linkedInterviewData;

    [HideInInspector] public Vector2 positionOnBoard = new Vector2(1000, 1000);
}
