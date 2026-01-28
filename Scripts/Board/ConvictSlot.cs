using UnityEngine;

public class ConvictSlot : MonoBehaviour
{
    [HideInInspector] public Card convictCard;

    [SerializeField] float cardOffset = 0f;
    [SerializeField] Canvas ConvictSlotUI;
    [SerializeField] float attachCooldown = 0.5f;

    private float lastDetachTime;

    private void Start()
    {
        if (GameManager.Instance.convictedCard != null)
        {
            AttachConvict(GameManager.Instance.convictedCard);
        }
    }

    public void AttachConvict(Card card)
    {
        if (convictCard != null) return;
        if (Time.time - lastDetachTime < attachCooldown) return;

        convictCard = card;
        GameManager.Instance.convictedCard = card;

        // Enable UI only for suspect cards
        if (card.cardData.cardType == CardType.Suspect) 
            ConvictSlotUI.gameObject.SetActive(true);
        else
            GameManager.Instance.ScreenLog.Log($"Cannot convict card of type: {card.cardData.cardType}");
    }

    public void DetachConvict()
    {
        if (convictCard == null) return;

        convictCard = null;
        GameManager.Instance.convictedCard = null;
        lastDetachTime = Time.time;

        ConvictSlotUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (convictCard != null)
        {
            Vector3 offset = new Vector3(0, cardOffset, 0);
            convictCard.MoveTo(transform.position + offset);
        }
    }

    public void DeclareGuilty()
    {
        if (convictCard != null)
        {
            GameManager.Instance.ScreenLog.Log($"You have declared {convictCard.cardData.cardName} guilty!");
        }
    }
}
