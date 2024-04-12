using UnityEngine;

public class CameraLookController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

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

    //Handles Raycasting from the UI reticle to active events based on collision
    void HandleRaycastUIInteraction()
    {
        RaycastHit hit;
        // Adjust the maxDistance as needed
        float maxDistance = 7f; // Example max distance for raycast
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;

        // Continuously perform the raycast from the camera position forward
        bool hasHit = Physics.Raycast(rayOrigin, rayDirection, out hit, maxDistance);
        if (hasHit)
        {
            //Debug.Log("Raycast hitting: " + hit.collider.name);
            // Check for left mouse button click and correct tag on the hit object
            if (Input.GetMouseButtonDown(0) && hit.collider.CompareTag("Herb"))
            {
                MinigameManager.Instance.TriggerMinigame(playerBody);
            }
        }
    }
}
