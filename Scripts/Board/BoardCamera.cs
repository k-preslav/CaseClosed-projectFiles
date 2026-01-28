using UnityEngine;

public class BoardCamera : MonoBehaviour
{
    [SerializeField] float boardZoom = -2.2f;
    [SerializeField] float cardZoom = -1.5f;

    [Space(5)]
    [SerializeField] float zoomSpeed = 5f;

    Vector3 targetPosition;

    private void Start()
    {
        GameManager.Instance.Cursor.SetVisible(true);
        GameManager.Instance.Cursor.SetLock(false);

        ZoomOnBoard();
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * zoomSpeed);
    }

    public void ZoomOnCard(Transform card)
    {
        targetPosition = new Vector3(card.position.x, card.position.y, cardZoom);
    }

    public void ZoomOnBoard()
    {
        targetPosition = new Vector3(0, 0, boardZoom);
    }
}
