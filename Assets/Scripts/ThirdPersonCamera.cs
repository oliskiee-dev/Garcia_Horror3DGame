using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    public bool invertY = false;
    public float minPitch = -40f;
    public float maxPitch = 80f;
    
    [Header("Camera Distance")]
    public float cameraDistance = 5f;
    public float cameraHeight = 2f;
    public float minDistance = 1f;
    public float maxDistance = 10f;
    
    [Header("Camera Smoothing")]
    public float rotationSmoothTime = 0.12f;
    public float positionSmoothTime = 0.12f;
    
    [Header("Camera Collision")]
    public bool enableCollision = true;
    public LayerMask collisionLayers = -1; // Everything by default
    public float collisionRadius = 0.3f;
    
    [Header("References")]
    [Tooltip("The player transform to follow (required).")]
    public Transform playerBody;
    
    [Header("Input Actions")]
    public InputActionReference lookAction;
    
    private float yaw = 0f;   // Horizontal rotation
    private float pitch = 0f; // Vertical rotation
    
    private float currentYaw = 0f;
    private float currentPitch = 0f;
    private float yawVelocity = 0f;
    private float pitchVelocity = 0f;
    
    private Vector3 currentPosition;
    private Vector3 positionVelocity = Vector3.zero;
    
    void Start()
    {
        // Get player body from parent if not assigned
        if (playerBody == null && transform.parent != null)
            playerBody = transform.parent;
        
        if (playerBody == null)
        {
            Debug.LogError("ThirdPersonCamera: playerBody is not assigned! Please assign the player transform.");
        }
        
        // Lock cursor for camera control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Initialize camera position
        if (playerBody != null)
        {
            yaw = playerBody.eulerAngles.y;
            currentYaw = yaw;
            currentPitch = pitch;
            currentPosition = CalculateIdealPosition();
            transform.position = currentPosition;
        }
    }
    
    void OnEnable()
    {
        lookAction?.action?.Enable();
    }
    
    void OnDisable()
    {
        lookAction?.action?.Disable();
    }
    
    void LateUpdate()
    {
        if (playerBody == null) return;
        
        // Read Look input (Vector2: x = horizontal, y = vertical)
        Vector2 lookInput = lookAction?.action?.ReadValue<Vector2>() ?? Vector2.zero;
        
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        
        if (invertY) mouseY = -mouseY;
        
        // Update rotation
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        
        // Smooth rotation
        currentYaw = Mathf.SmoothDampAngle(currentYaw, yaw, ref yawVelocity, rotationSmoothTime);
        currentPitch = Mathf.SmoothDampAngle(currentPitch, pitch, ref pitchVelocity, rotationSmoothTime);
        
        // Calculate ideal camera position
        Vector3 idealPosition = CalculateIdealPosition();
        
        // Handle camera collision
        if (enableCollision)
        {
            idealPosition = HandleCameraCollision(idealPosition);
        }
        
        // Smooth position
        currentPosition = Vector3.SmoothDamp(currentPosition, idealPosition, ref positionVelocity, positionSmoothTime);
        
        // Apply position and rotation
        transform.position = currentPosition;
        transform.LookAt(playerBody.position + Vector3.up * cameraHeight);
        
        // Rotate player to face camera forward direction (horizontal only)
        Vector3 cameraForward = transform.forward;
        cameraForward.y = 0f;
        if (cameraForward != Vector3.zero)
        {
            playerBody.rotation = Quaternion.Euler(0f, currentYaw, 0f);
        }
        
        // Quick cursor unlock for debugging (press Escape)
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    Vector3 CalculateIdealPosition()
    {
        // Calculate position behind and above the player
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, 0f, -cameraDistance);
        
        Vector3 targetPosition = playerBody.position + Vector3.up * cameraHeight + offset;
        return targetPosition;
    }
    
    Vector3 HandleCameraCollision(Vector3 idealPosition)
    {
        Vector3 targetPoint = playerBody.position + Vector3.up * cameraHeight;
        Vector3 direction = idealPosition - targetPoint;
        float distance = direction.magnitude;
        
        if (distance < 0.1f) return idealPosition;
        
        RaycastHit hit;
        if (Physics.SphereCast(targetPoint, collisionRadius, direction.normalized, out hit, distance, collisionLayers))
        {
            // Move camera to collision point
            float safeDistance = Mathf.Max(hit.distance - collisionRadius, minDistance);
            return targetPoint + direction.normalized * safeDistance;
        }
        
        return idealPosition;
    }
    
    /* Usage:
     - Detach this script from being a child of the player.
     - Attach this script to a separate Camera GameObject in the scene.
     - Set `playerBody` to the Player GameObject in the inspector.
     - Adjust camera distance, height, and sensitivity in inspector.
     - In the Input Actions section:
       * Assign Look Action to InputSystem_Actions > Player > Look
     - The camera will follow the player and rotate based on mouse movement.
     - Enable Camera Collision to prevent the camera from clipping through walls.
     - Set Collision Layers to exclude the player layer to avoid self-collision.
    */
}
