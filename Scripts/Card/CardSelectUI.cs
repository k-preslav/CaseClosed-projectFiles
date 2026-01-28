using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardSelectUI : MonoBehaviour
{
    public Button cardActionButton;
    public TMP_Text cardActionButtonText;

    [Space(3)]
    public TMP_Text descriptionText;

    private void Start()
    {
        Close();
    }

    public void Close()
    {
        FindAnyObjectByType<BoardCamera>().ZoomOnBoard();

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        GameManager.Instance.selectedCardData = null;
    }

    public void PerformCardAction()
    {
        if (GameManager.Instance.selectedCardData == null) return;

        var cardData = GameManager.Instance.selectedCardData;

        if (cardData.cardType == CardType.Location)
        {
            GameManager.Instance.GoToLocation(cardData.linkedSceneName);
        }
        else if (cardData.cardType == CardType.Suspect)
        {
            GameManager.Instance.GoToInterview();
        }
    }

    public void SetupUI()
    {
        if (GameManager.Instance.selectedCardData == null) return;
        
        descriptionText.text = GameManager.Instance.selectedCardData.description;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        cardActionButton.interactable = true;

        switch (GameManager.Instance.selectedCardData.cardType)
        {
            case CardType.Location:
                cardActionButtonText.text = "Inspect";
                cardActionButton.gameObject.SetActive(true);
                break;
            case CardType.Suspect:
                var interviewData = GameManager.Instance.selectedCardData.linkedInterviewData;
                if (interviewData == null)
                {
                    cardActionButton.gameObject.SetActive(false);
                    break;
                }

                if (GameManager.Instance.completedInterviews.Contains(interviewData)) 
                    cardActionButton.interactable = false;

                cardActionButton.gameObject.SetActive(true);
                cardActionButtonText.text = "Interview";
                break;
            case CardType.GenericClue:
                cardActionButton.gameObject.SetActive(false);
                break;
        }
    }

    public void AssignSelectedCardData(CardData cardData)
    {
        GameManager.Instance.selectedCardData = cardData;
    }
}
