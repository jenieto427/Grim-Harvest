using UnityEngine;

public class CameraLookController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform orientation; // Reference to the player's body for rotation and possibly positioning

    private float xRotation;
    private float yRotation;
    public UIManager uiManager;
    public MinigameManager minigameManager;
    public InteractionManager interactionManager;
    public Player player;

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
        float maxDistance = 7f; // Maximum distance for raycast
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxDistance))
        {
            // Handle interaction based on mouse input
            HandleInteraction(hit, Input.GetMouseButtonDown(0));
        }
        else
        {
            uiManager.UpdateInteractionUI(""); // Clear UI text when no hit is detected
        }
    }
    // Reticle raycast handling
    void HandleInteraction(RaycastHit hit, bool isMouseClicked)
    {
        string tag = hit.collider.tag;

        // Decide on action based on tag and whether the mouse was clicked
        if (tag == "Herb")
        {
            if (isMouseClicked)
            {
                // Check if the player has energy
                if (player.energy <= 1)
                {
                    uiManager.UpdateNotificationQueue("Your brain isn't capable of work");
                    return;
                }
                hit.collider.enabled = false; // Disable collider to prevent re-harvesting
                minigameManager.TriggerMinigame(hit.collider.gameObject); // Trigger minigame
            }
            uiManager.UpdateInteractionUI(isMouseClicked ? "" : "Study Plant \n(Left Click)");
        }
        else if (tag == "NPC Vendor")
        {
            if (isMouseClicked)
            {
                interactionManager.HandleVendorInteraction(player, hit.collider.gameObject);
            }
            uiManager.UpdateInteractionUI(isMouseClicked ? "" : "Buy Stimulants, 5 samples ea. \n(Left Click)");
        }
    }
}
