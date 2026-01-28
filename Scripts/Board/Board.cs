using UnityEngine;

public class Board : MonoBehaviour
{
    public BoardCamera BoardCamera;
    public CardSelectUI CardSelectUI;

    [Space(5)]
    [SerializeField] GameObject boardCardSpawnBounds;

    public Bounds? CardBounds { get; private set; }

    private void Start()
    {
        var collider = boardCardSpawnBounds.GetComponent<Collider>();
        if (collider != null)
            CardBounds = collider.bounds;

        foreach (var card in GameManager.Instance.discoveredCards)
        {
            SpawnDiscoveredCard(card);
        }
    }

    void SpawnDiscoveredCard(CardData card)
    {
        Vector2 spawnPosition = Vector2.zero;
        if (card.positionOnBoard == new Vector2(1000, 1000)) spawnPosition = GetRandomSpawnPosition();
        else spawnPosition = card.positionOnBoard;

        var cardObj = Instantiate(GameManager.Instance.cardPrefab, new Vector3(spawnPosition.x, spawnPosition.y, -0.0325f), Quaternion.identity);
        var cardComponent = cardObj.GetComponent<Card>();
        cardComponent.cardData = card;
        cardComponent.cardData.positionOnBoard = spawnPosition;
        cardComponent.board = this;
        cardComponent.ProcessCardData();
    }

    Vector2 GetRandomSpawnPosition()
    {
        if (CardBounds.HasValue)
        {
            var b = CardBounds.Value;
            float x = Random.Range(b.min.x, b.max.x);
            float z = Random.Range(b.min.z, b.max.z);
            return new Vector2(x, z);
        }

        GameManager.Instance.ScreenLog.LogWarn("No collider found on Board to determine spawn area.");
        return Vector2.zero;
    }
}
