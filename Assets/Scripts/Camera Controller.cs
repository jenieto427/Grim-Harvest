using UnityEngine;

public class CameraLookController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public bool enableBounce = false; // Toggle for the bounce effect
    public float walkBounceAmount = 0.5f; // Bounce amount when walking
    public float sprintBounceAmount = 1.0f; // Increased bounce when sprinting
    public float bounceSpeed = 2f; // Speed of the bounce effect
    public float sprintSpeedThreshold = 5f; // Speed threshold to consider the player is sprinting

    private float xRotation = 0f;
    private float originalYPos = 0f;
    private bool isSprinting = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        originalYPos = transform.localPosition.y; // Store the original Y position for bouncing
    }

    void Update()
    {
        HandleMouseLook();
        HandleCameraBounce();
        HandleRaycastUIInteraction();
    }

    //Handles 1st person camera looking with mouse
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent over-rotation

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    //Handles camera's bounce during walking and sprinting of the player gameobject
    void HandleCameraBounce()
    {
        if (!enableBounce) return;

        // Determine if the player is sprinting based on their speed
        isSprinting = playerBody.GetComponent<Rigidbody>().velocity.magnitude > sprintSpeedThreshold;

        float bounceAmount = isSprinting ? sprintBounceAmount : walkBounceAmount;
        float newY = originalYPos + Mathf.Sin(Time.time * bounceSpeed) * bounceAmount;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }

    //Handles Raycasting from the UI reticle to active events based on collision
    void HandleRaycastUIInteraction()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            RaycastHit hit;
            // Adjust the maxDistance as needed
            float maxDistance = 100f; // Example max distance for raycast

            // Perform the raycast from the camera position forward
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
            {
                // Check if the hit object has the correct tag
                if (hit.collider.CompareTag("Plant"))
                {
                    // Assuming GameManager has a public method to handle minigame triggering
                    MinigameManager.Instance.TriggerMinigame(playerBody);
                }
            }
        }
    }
}
