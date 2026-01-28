using UnityEngine;

public class LocationExplorer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.8f;
    
    [Space(5)]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 2f;

    [Space(5)]
    [SerializeField] PickupObjectUI pickupObjectUI;

    private CharacterController characterController;
    private Vector3 velocity;
    private float pitch = 0f;

    bool isObjectSelected = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            GameManager.Instance.ScreenLog.LogErr("CharacterController component is missing on this GameObject.");
        }

        GameManager.Instance.Cursor.SetVisible(false);
        GameManager.Instance.Cursor.SetLock(true);

        pickupObjectUI.OnUIClose += () =>
        {
            isObjectSelected = false;
        };
    }

    void Update()
    {
        if (characterController == null) return;

        if (!isObjectSelected)
        {
            HandleMovement();
            HandleCameraRotation();
        }

        HandleObjectSelect();
    }

    private void HandleMovement()
    {
        // Get input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Calculate movement direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply movement
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        // Reset vertical velocity if grounded
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
    }

    private void HandleCameraRotation()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the player around the Y-axis
        transform.Rotate(Vector3.up * mouseX);

        // Adjust the camera pitch (clamp to avoid flipping)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void HandleObjectSelect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                var pickupObject = hitInfo.transform.GetComponent<PickupObject>();
                if (pickupObject != null)
                {
                    isObjectSelected = true;
                    pickupObjectUI.SetupUI(pickupObject);
                    pickupObject.Pickup();
                }
            }
        }
    }
}
