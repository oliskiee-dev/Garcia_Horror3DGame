using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;

    [Header("Jump / Gravity")]
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Input")]
    public InputActionAsset inputActions;

    CharacterController controller;
    Vector3 velocity;
    bool isGrounded;
    
    // Input System actions
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (groundCheck == null)
        {
            // create a fallback ground check at the object's feet
            GameObject go = new GameObject("GroundCheck");
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(0f, -1f, 0f);
            groundCheck = go.transform;
        }
        
        // Get actions from the Input Action Asset
        if (inputActions != null)
        {
            var playerMap = inputActions.FindActionMap("Player");
            moveAction = playerMap.FindAction("Move");
            jumpAction = playerMap.FindAction("Jump");
            sprintAction = playerMap.FindAction("Sprint");
            
            Debug.Log($"Input Actions loaded: Move={moveAction != null}, Jump={jumpAction != null}, Sprint={sprintAction != null}");
        }
        else
        {
            Debug.LogError("InputActions asset not assigned! Please assign InputSystem_Actions in the inspector.");
        }
    }

    void OnEnable()
    {
        moveAction?.Enable();
        jumpAction?.Enable();
        sprintAction?.Enable();
    }

    void OnDisable()
    {
        moveAction?.Disable();
        jumpAction?.Disable();
        sprintAction?.Disable();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            // small negative value to keep the controller grounded
            velocity.y = -2f;
        }

        // Input - read from Input Actions
        Vector2 moveInput = moveAction?.ReadValue<Vector2>() ?? Vector2.zero;
        float x = moveInput.x;
        float z = moveInput.y;

        Vector3 move = transform.right * x + transform.forward * z;

        // Optional run when holding Sprint
        bool isSprinting = sprintAction?.IsPressed() ?? false;
        float speed = isSprinting ? runSpeed : walkSpeed;

        controller.Move(move * speed * Time.deltaTime);

        // Jump
        bool jumpPressed = jumpAction?.WasPressedThisFrame() ?? false;
        
        if (jumpPressed && isGrounded)
        {
            // v = sqrt(h * -2 * g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log($"JUMPING! velocity.y = {velocity.y}");
        }
        else if (jumpPressed && !isGrounded)
        {
            Debug.Log("Can't jump - not grounded!");
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Visualize ground check sphere in editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }

    /* Usage:
     - Attach this script to your Player GameObject.
     - Add a CharacterController component to the same GameObject (the script requires it).
     - Assign the player's Camera or an empty child Transform as the Ground Check (or leave empty and a default will be created).
     - Set the Ground Mask to the layers that count as ground (e.g. Default, Terrain).
     - Assign the InputSystem_Actions asset to the Input Actions field in the inspector.
     - Attach `FirstPersonCamera.cs` to the Camera and set its Player Body reference to this GameObject.
    */
}
