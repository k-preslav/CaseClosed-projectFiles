using UnityEngine;

public class PickupObject : MonoBehaviour
{
    // TODO: Make sure if an object is selected, to not be able to select another one

    public CardData assignedCardData;
    public string objectDescription;

    [SerializeField] float pickupZ = 1.3f;
    [SerializeField] float maxTilt = 8f;
    [SerializeField] float smooth = 10f;

    Vector3 startPosition;
    Quaternion startRotation;

    Vector3 targetPosition;
    Quaternion targetRotation;

    Transform playerCamera;
    bool isPickedUp;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        targetPosition = startPosition;
        targetRotation = startRotation;
    }

    void Update()
    {
        if (isPickedUp && playerCamera != null)
        {
            // Keep object in front of camera
            targetPosition = playerCamera.position + playerCamera.forward * pickupZ;

            // Mouse position normalized to [-1, 1]
            float nx = (Input.mousePosition.x / Screen.width) * 2f - 1f;
            float ny = (Input.mousePosition.y / Screen.height) * 2f - 1f;

            // Counter-tilt (note the minus signs)
            float tiltX = -ny * maxTilt;
            float tiltY = -nx * maxTilt;

            Quaternion tiltRotation = Quaternion.Euler(tiltX, -tiltY, 0f);

            targetRotation = playerCamera.rotation * tiltRotation;
        }

        transform.position =
            Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);

        transform.rotation =
            Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smooth);
    }

    public void Pickup()
    {
        playerCamera = Camera.main.transform;
        isPickedUp = true;
    }

    public void Drop()
    {
        targetPosition = startPosition;
        targetRotation = startRotation;

        isPickedUp = false;
        playerCamera = null;
    }

    public void AddAsClue()
    {
        GameManager.Instance.DiscoverCard(assignedCardData);
    }
}
