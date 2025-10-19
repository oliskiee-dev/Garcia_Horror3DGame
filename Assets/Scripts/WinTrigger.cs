using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [Header("Win Settings")]
    [Tooltip("The UI panel that displays the win banner")]
    public GameObject winBannerUI;
    
    [Header("Key Requirements")]
    [Tooltip("Require all keys to be collected before winning?")]
    public bool requireKeys = true;
    
    [Header("Blocked Message")]
    [Tooltip("Message to show when player doesn't have all keys")]
    public string blockedMessage = "🔒 You need all keys to escape!";
    public float blockedMessageDuration = 3f;
    
    [Header("Game Over Options")]
    public bool pauseGameOnWin = true;
    public bool lockCursorOnWin = false;
    
    [Header("Audio (Optional)")]
    public AudioClip winSound;
    public AudioClip blockedSound;
    private AudioSource audioSource;
    
    private bool hasWon = false;
    private KeyManager keyManager;
    
    void Start()
    {
        // Make sure win banner is hidden at start
        if (winBannerUI != null)
        {
            winBannerUI.SetActive(false);
        }
        
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && (winSound != null || blockedSound != null))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Find KeyManager
        keyManager = FindFirstObjectByType<KeyManager>();
        if (requireKeys && keyManager == null)
        {
            Debug.LogWarning("WinTrigger: KeyManager not found! Disable 'Require Keys' or add KeyManager to scene.");
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (!hasWon && other.CompareTag("Player"))
        {
            // Check if keys are required
            if (requireKeys && keyManager != null)
            {
                if (keyManager.HasAllKeys())
                {
                    TriggerWin();
                }
                else
                {
                    // Player doesn't have all keys
                    ShowBlockedMessage();
                }
            }
            else
            {
                // No keys required, just win
                TriggerWin();
            }
        }
    }
    
    void ShowBlockedMessage()
    {
        if (keyManager != null)
        {
            int collected = keyManager.GetKeysCollected();
            int required = keyManager.GetKeysRequired();
            string message = $"{blockedMessage}\n({collected}/{required} keys collected)";
            Debug.LogWarning(message);
        }
        else
        {
            Debug.LogWarning(blockedMessage);
        }
        
        // Play blocked sound
        if (audioSource != null && blockedSound != null)
        {
            audioSource.PlayOneShot(blockedSound);
        }
        
        // Optional: Show UI message (you can add this later)
        // For now, just log to console
    }
    
    void TriggerWin()
    {
        hasWon = true;
        
        Debug.Log("🎉 PLAYER WINS! 🎉");
        
        // Show win banner
        if (winBannerUI != null)
        {
            winBannerUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Win Banner UI not assigned to WinTrigger!");
        }
        
        // Play win sound
        if (audioSource != null && winSound != null)
        {
            audioSource.PlayOneShot(winSound);
        }
        
        // Unlock and show cursor
        if (lockCursorOnWin)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        // Pause game
        if (pauseGameOnWin)
        {
            Time.timeScale = 0f;
        }
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
    
    /* Usage:
     * 1. Create an empty GameObject at your final door position
     * 2. Add this script to it
     * 3. Add a Box Collider or Trigger Collider to it
     * 4. Check "Is Trigger" on the collider
     * 5. Adjust collider size to cover the door area
     * 6. Make sure your Player GameObject has the "Player" tag
     * 7. Create a UI Canvas with a win banner panel
     * 8. Assign the win banner panel to this script's "Win Banner UI" field
     * 9. The banner will show when player walks through the door!
     */
}
