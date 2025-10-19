using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    [Header("Game Over Settings")]
    [Tooltip("The UI panel that displays the game over banner")]
    public GameObject gameOverBannerUI;
    
    [Header("Trigger Type")]
    [Tooltip("How should the game over be triggered?")]
    public GameOverType triggerType = GameOverType.OnTriggerEnter;
    
    [Header("Game Over Options")]
    public bool pauseGameOnGameOver = true;
    public bool unlockCursorOnGameOver = true;
    
    [Header("Visual Effects")]
    public bool shakeCamera = false;
    public float shakeDuration = 0.3f;
    public float shakeIntensity = 0.5f;
    public GameObject deathEffectPrefab;
    
    [Header("Audio")]
    public AudioClip gameOverSound;
    private AudioSource audioSource;
    
    [Header("Death Cause")]
    [TextArea(2, 3)]
    public string deathMessage = "You Died";
    
    [Header("Timing")]
    public float delayBeforeBanner = 0f;
    
    private bool hasGameOver = false;
    private Vector3 deathPosition;
    
    public enum GameOverType
    {
        OnTriggerEnter,     // Player walks into trigger zone
        OnCollisionEnter,   // Player collides with this object
        Manual              // Call TriggerGameOver() from another script
    }
    
    void Start()
    {
        // Make sure game over banner is hidden at start
        if (gameOverBannerUI != null)
        {
            gameOverBannerUI.SetActive(false);
        }
        
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && gameOverSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (triggerType == GameOverType.OnTriggerEnter && !hasGameOver)
        {
            // Check if the player entered the trigger
            if (other.CompareTag("Player"))
            {
                deathPosition = other.transform.position;
                TriggerGameOver();
            }
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (triggerType == GameOverType.OnCollisionEnter && !hasGameOver)
        {
            // Check if the player collided with this object
            if (collision.gameObject.CompareTag("Player"))
            {
                deathPosition = collision.contacts.Length > 0 ? collision.contacts[0].point : collision.transform.position;
                TriggerGameOver();
            }
        }
    }
    
    // Public method to trigger game over from other scripts
    public void TriggerGameOver()
    {
        if (hasGameOver) return;
        
        hasGameOver = true;
        
        Debug.Log($"💀 GAME OVER! Cause: {deathMessage}");
        
        // Play game over sound at death position
        if (audioSource != null && gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }
        else if (gameOverSound != null)
        {
            // Play sound at death position if no AudioSource
            AudioSource.PlayClipAtPoint(gameOverSound, deathPosition);
        }
        
        // Spawn death effect (blood, particles, etc.)
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, deathPosition, Quaternion.identity);
        }
        
        // Camera shake
        if (shakeCamera)
        {
            StartCoroutine(CameraShake());
        }
        
        // Show game over banner (with optional delay)
        if (delayBeforeBanner > 0)
        {
            Invoke(nameof(ShowGameOverBanner), delayBeforeBanner);
        }
        else
        {
            ShowGameOverBanner();
        }
    }
    
    void ShowGameOverBanner()
    {
        Debug.Log("Showing Game Over Banner...");
        
        // Show game over banner
        if (gameOverBannerUI != null)
        {
            Debug.Log("GameOver Banner UI found - activating it now!");
            gameOverBannerUI.SetActive(true);
            
            // Pass death message to the UI if it has the script
            GameOverBannerUI bannerUI = gameOverBannerUI.GetComponent<GameOverBannerUI>();
            if (bannerUI != null && !string.IsNullOrEmpty(deathMessage))
            {
                Debug.Log($"Setting death message: {deathMessage}");
                bannerUI.SetDeathMessage(deathMessage);
            }
            else if (bannerUI == null)
            {
                Debug.LogError("GameOverBannerUI component not found on the assigned GameObject!");
            }
        }
        else
        {
            Debug.LogError("Game Over Banner UI not assigned to GameOverTrigger!");
        }
        
        // Unlock and show cursor
        if (unlockCursorOnGameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        // Pause game
        if (pauseGameOnGameOver)
        {
            Time.timeScale = 0f;
        }
    }
    
    System.Collections.IEnumerator CameraShake()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) yield break;
        
        Vector3 originalPosition = mainCamera.transform.localPosition;
        float elapsed = 0f;
        
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            
            mainCamera.transform.localPosition = originalPosition + new Vector3(x, y, 0f);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        mainCamera.transform.localPosition = originalPosition;
    }
    
    // Public method to restart (call from UI button)
    public void RestartGame()
    {
        Time.timeScale = 1f; // Unpause
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
    
    // Public method to go to main menu (call from UI button)
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Unpause
        // Change "MainMenu" to your actual menu scene name
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    
    // Public method to quit game (call from UI button)
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    /* Usage Examples:
     * 
     * 1. DEATH ZONE (Player falls off map):
     *    - Create empty GameObject below the map
     *    - Add large Box Collider, set "Is Trigger" = true
     *    - Add this script, set Trigger Type = "OnTriggerEnter"
     *    - Set Death Message = "You fell into the void"
     * 
     * 2. ENEMY KILL (Player touches enemy):
     *    - Add this script to enemy GameObject
     *    - Set Trigger Type = "OnCollisionEnter" or "OnTriggerEnter"
     *    - Set Death Message = "Killed by [Enemy Name]"
     *    - Optional: Enable Camera Shake for impact
     * 
     * 3. SPIKE TRAP (Environmental hazard):
     *    - Add this script to spike GameObject
     *    - Add Box Collider, set "Is Trigger" = true
     *    - Set Death Message = "Impaled by spikes"
     *    - Enable "Shake Camera" for dramatic effect
     *    - Set Shake Duration = 0.3, Shake Intensity = 0.5
     *    - Add spike hit sound to "Game Over Sound"
     *    - Optional: Add blood particle prefab to "Death Effect Prefab"
     * 
     * 4. FIRE/ACID/POISON TRAP:
     *    - Same as spike trap
     *    - Adjust Death Message: "Burned alive" / "Dissolved in acid" / "Poisoned"
     *    - Use appropriate sound effects and particle effects
     * 
     * 5. INSTANT DEATH vs DELAYED DEATH:
     *    - For instant death (spikes): Set Delay Before Banner = 0
     *    - For dramatic death (fall): Set Delay Before Banner = 1.5 (shows death animation first)
     * 
     * 6. MANUAL TRIGGER (From another script):
     *    - Set Trigger Type = "Manual"
     *    - Call: FindFirstObjectByType<GameOverTrigger>().TriggerGameOver()
     * 
     * 7. HEALTH SYSTEM:
     *    - In your PlayerHealth script, when health reaches 0:
     *      GameOverTrigger trigger = FindFirstObjectByType<GameOverTrigger>();
     *      trigger.deathMessage = "You ran out of health";
     *      trigger.TriggerGameOver();
     * 
     * VISUAL EFFECTS OPTIONS:
     * - Shake Camera: Adds screen shake on death
     * - Death Effect Prefab: Spawns particle system (blood, explosion, etc.)
     * - Game Over Sound: Plays audio on death
     * - Delay Before Banner: Seconds to wait before showing game over screen
     */
}
