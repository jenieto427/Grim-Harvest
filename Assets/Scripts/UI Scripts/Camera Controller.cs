using UnityEngine;

public class CameraLookController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform orientation; // Reference to the player's body for rotation and possibly positioning

    private float xRotation;
    private float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    private void LateUpdate()
    {
        HandleMouseLook();
        HandleRaycastUIInteraction();
    }

    // Handles 1st person camera looking with mouse
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yRotation += mouseX * Time.deltaTime;

        xRotation -= mouseY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamps the vertical rotation

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    // Handles Raycasting from the UI reticle to active events based on collision
    void HandleRaycastUIInteraction()
    {
        RaycastHit hit;
        float maxDistance = 7f; // Example max distance for raycast
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;

        if (!Physics.Raycast(rayOrigin, rayDirection, out hit, maxDistance) || !Input.GetMouseButtonDown(0))
            return; // Exit if no collision or mouse button is not pressed

        if (hit.collider.CompareTag("Herb"))
        {
            hit.collider.enabled = false; // Disable herb collider, to prevent re-harvesting
            MinigameManager.Instance.TriggerMinigame(hit.collider.gameObject); // Activate the minigame
        }
    }
}
