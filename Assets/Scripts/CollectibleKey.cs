using UnityEngine;

public class CollectibleKey : MonoBehaviour
{
    [Header("Key Settings")]
    [Tooltip("The name/description of this key")]
    public string keyName = "Key";
    
    [Header("Visual Effects")]
    public bool rotateKey = true;
    public float rotationSpeed = 50f;
    public bool floatKey = true;
    public float floatAmplitude = 0.3f;
    public float floatSpeed = 2f;
    
    [Header("Particle Effect (Optional)")]
    public GameObject collectParticles;
    
    [Header("Audio")]
    public AudioClip collectSound;
    
    [Header("UI Feedback")]
    public bool showCollectMessage = true;
    public float messageDisplayTime = 2f;
    
    private Vector3 startPosition;
    private AudioSource audioSource;
    
    void Start()
    {
        startPosition = transform.position;
        
        // Get or create AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && collectSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }
    
    void Update()
    {
        // Rotate the key
        if (rotateKey)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        
        // Float up and down
        if (floatKey)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Check if player collected the key
        if (other.CompareTag("Player"))
        {
            CollectKey(other.gameObject);
        }
    }
    
    void CollectKey(GameObject player)
    {
        // Find the KeyManager and add key
        KeyManager keyManager = FindFirstObjectByType<KeyManager>();
        if (keyManager != null)
        {
            keyManager.CollectKey(keyName);
        }
        else
        {
            Debug.LogWarning("KeyManager not found! Please add KeyManager to the scene.");
        }
        
        // Play collect sound
        if (collectSound != null)
        {
            // Play sound at this position (continues after object destroyed)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
        
        // Spawn particle effect
        if (collectParticles != null)
        {
            Instantiate(collectParticles, transform.position, Quaternion.identity);
        }
        
        // Show message
        if (showCollectMessage)
        {
            Debug.Log($"🔑 Collected: {keyName}");
        }
        
        // Destroy the key
        Destroy(gameObject);
    }
    
    /* Usage:
     * 1. Create a 3D object for the key (Cube, or import a key model)
     * 2. Add this script to the key GameObject
     * 3. Add a Collider to the key (set "Is Trigger" = true)
     * 4. Add a Rigidbody (set "Is Kinematic" = true) - optional but recommended
     * 5. Set the key name in inspector (e.g., "Red Key", "Door Key 1")
     * 6. Make sure Player has "Player" tag
     * 7. Make sure KeyManager exists in the scene
     * 8. Duplicate the key 3 times for 3 collectible keys
     */
}
