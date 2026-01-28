using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [HideInInspector] public CardData cardData;
    [HideInInspector] public Board board;

    [SerializeField] TextMeshPro titleText;
    [SerializeField] float dragThreshold = 0.5f;
    [SerializeField] float moveSmoothTime = 0.1f;
    [SerializeField] float hoverZOffset = -0.2f;
    [SerializeField] float dragZOffset = -0.4f;
    [SerializeField] float zOffsetSmoothTime = 0.15f;
    [SerializeField] float cardOverlapDistance = 0.8f;

    private bool isDragging;
    private bool isHovered;
    private Vector3? dragStartWorld;
    private Vector3 velocity;
    private Collider cardCollider;
    private Vector3 targetPosition;
    private Vector3 dragStartPosition;
    private float currentZOffset;
    private float zOffsetVelocity;
    private Bounds? boardBounds;

    public void ProcessCardData()
    {
        if (cardData != null)
            titleText.text = cardData.cardName;
    }

    private void Awake()
    {
        cardCollider = GetComponent<Collider>();
        targetPosition = transform.position;
        dragStartPosition = transform.position;
    }

    private void Start()
    {
        if (board != null)
        {
            var boundsCollider = board.GetComponentInChildren<Collider>();
            if (boundsCollider != null)
                boardBounds = boundsCollider.bounds;
        }
    }

    private void Update()
    {
        if (Camera.main == null || GameManager.Instance.selectedCardData != null) return;

        UpdateHoverState();
        HandleInput();
        UpdatePosition();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
            HandleMouseDown();

        if (Input.GetMouseButton(0) && dragStartWorld.HasValue)
            HandleMouseDrag();

        if (Input.GetMouseButtonUp(0))
            HandleMouseUp();
    }

    private void UpdatePosition()
    {
        Vector3 currentPos = transform.position;
        Vector3 targetXY = new Vector3(targetPosition.x, targetPosition.y, currentPos.z);
        Vector3 smoothedPos = Vector3.SmoothDamp(currentPos, targetXY, ref velocity, moveSmoothTime);

        float targetZ = targetPosition.z;
        if (isHovered || isDragging)
        {
            float targetZOffset = isDragging ? dragZOffset : hoverZOffset;
            currentZOffset = Mathf.SmoothDamp(currentZOffset, targetZOffset, ref zOffsetVelocity, zOffsetSmoothTime);
            targetZ += currentZOffset;
        }
        else if (currentZOffset != 0f)
        {
            currentZOffset = Mathf.SmoothDamp(currentZOffset, 0f, ref zOffsetVelocity, zOffsetSmoothTime);
            targetZ += currentZOffset;
        }

        transform.position = new Vector3(smoothedPos.x, smoothedPos.y, targetZ);
    }

    private void UpdateHoverState()
    {
        if (isDragging)
        {
            isHovered = false;
            return;
        }

        var allCards = FindObjectsByType<Card>(FindObjectsSortMode.None);
        foreach (var card in allCards)
        {
            if (card != this && card.isDragging)
            {
                isHovered = false;
                return;
            }
        }

        isHovered = TryRaycast(out var hit) && hit.transform == transform;
    }

    private void HandleMouseDown()
    {
        if (TryRaycast(out var hit) && hit.transform == transform)
        {
            dragStartWorld = hit.point;
            dragStartPosition = targetPosition;
        }
        else
        {
            dragStartWorld = null;
        }
    }

    private void HandleMouseDrag()
    {
        if (!TryRaycast(out var hit)) return;

        var current = hit.point;
        if (!isDragging && Vector2.Distance(dragStartWorld.Value.ToXZ(), current.ToXZ()) > dragThreshold)
        {
            isDragging = true;
            if (cardCollider != null)
                cardCollider.enabled = false;
        }

        if (!isDragging) return;

        var convictSlot = FindAnyObjectByType<ConvictSlot>();
        bool isOverConvictSlot = hit.collider.gameObject.CompareTag("Convict Slot");

        if (isOverConvictSlot && convictSlot.convictCard != this)
        {
            convictSlot.AttachConvict(this);
        }
        else if (!isOverConvictSlot)
        {
            if (convictSlot.convictCard == this)
                convictSlot.DetachConvict();
            MoveTo(current);
        }
    }

    private void HandleMouseUp()
    {
        if (dragStartWorld.HasValue && !isDragging)
            OnClicked();

        dragStartWorld = null;
        isDragging = false;

        if (cardCollider != null)
            cardCollider.enabled = true;

        if (IsOverlappingWithCard())
        {
            targetPosition = dragStartPosition;
            velocity = Vector3.zero;
            currentZOffset = 0f;
            zOffsetVelocity = 0f;
            isHovered = false;

            if (cardData != null)
                cardData.positionOnBoard = new Vector2(dragStartPosition.x, dragStartPosition.y);

            GameManager.Instance.ScreenLog.LogWarn("Card placement blocked - too close to another card!");
            return;
        }

        if (TryRaycast(out var hit) && hit.transform == transform)
            isHovered = true;
    }

    private bool IsOverlappingWithCard()
    {
        var allCards = FindObjectsByType<Card>(FindObjectsSortMode.None);

        foreach (var otherCard in allCards)
        {
            if (otherCard == this) continue;

            Vector2 thisPos = new Vector2(targetPosition.x, targetPosition.y);
            Vector2 otherPos = new Vector2(otherCard.targetPosition.x, otherCard.targetPosition.y);

            if (Vector2.Distance(thisPos, otherPos) < cardOverlapDistance)
                return true;
        }

        return false;
    }

    private bool TryRaycast(out RaycastHit hit)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit);
    }

    public void MoveTo(Vector3 worldPos)
    {
        worldPos.z = targetPosition.z;

        // Clamp position within board bounds
        if (boardBounds.HasValue)
        {
            var b = boardBounds.Value;
            worldPos.x = Mathf.Clamp(worldPos.x, b.min.x, b.max.x);
            worldPos.y = Mathf.Clamp(worldPos.y, b.min.y, b.max.y);
        }

        transform.position = Vector3.SmoothDamp(transform.position, worldPos, ref velocity, moveSmoothTime);
        targetPosition = new Vector3(worldPos.x, worldPos.y, targetPosition.z);

        if (cardData != null)
            cardData.positionOnBoard = new Vector2(worldPos.x, worldPos.y);
    }

    private void OnClicked()
    {
        board.BoardCamera.ZoomOnCard(transform);
        board.CardSelectUI.AssignSelectedCardData(cardData);
        board.CardSelectUI.SetupUI();
        GameManager.Instance.selectedCardData = cardData;
    }
}

public static class VectorExtensions
{
    public static Vector2 ToXZ(this Vector3 v) => new Vector2(v.x, v.z);
}