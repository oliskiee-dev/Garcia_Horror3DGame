using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class KeyManager : MonoBehaviour
{
    [Header("Key Requirements")]
    [Tooltip("How many keys needed to win?")]
    public int keysRequiredToWin = 3;
    
    [Header("UI Elements")]
    public TextMeshProUGUI keyCounterText;
    public GameObject keyCollectMessageUI;
    public TextMeshProUGUI keyCollectMessageText;
    
    [Header("UI Display Format")]
    public string keyCounterFormat = "Keys: {0}/{1}";
    public string keyCollectMessageFormat = "🔑 {0} Collected!";
    
    [Header("Visual Feedback")]
    public bool animateOnCollect = true;
    public float messageDisplayDuration = 2f;
    public AudioClip keyCollectSound;
    
    // Internal tracking
    private int keysCollected = 0;
    private List<string> collectedKeyNames = new List<string>();
    private AudioSource audioSource;
    
    // Singleton pattern for easy access
    public static KeyManager Instance { get; private set; }
    
    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Setup audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && keyCollectSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }
    
    void Start()
    {
        // Initialize UI
        UpdateKeyCounterUI();
        
        // Hide collect message
        if (keyCollectMessageUI != null)
        {
            keyCollectMessageUI.SetActive(false);
        }
    }
    
    public void CollectKey(string keyName)
    {
        // Add to collection
        keysCollected++;
        collectedKeyNames.Add(keyName);
        
        Debug.Log($"🔑 Key collected: {keyName} ({keysCollected}/{keysRequiredToWin})");
        
        // Update UI
        UpdateKeyCounterUI();
        
        // Show collect message
        if (keyCollectMessageUI != null && keyCollectMessageText != null)
        {
            keyCollectMessageText.text = string.Format(keyCollectMessageFormat, keyName);
            keyCollectMessageUI.SetActive(true);
            
            // Hide after duration
            if (messageDisplayDuration > 0)
            {
                Invoke(nameof(HideCollectMessage), messageDisplayDuration);
            }
        }
        
        // Play sound
        if (audioSource != null && keyCollectSound != null)
        {
            audioSource.PlayOneShot(keyCollectSound);
        }
        
        // Animate if enabled
        if (animateOnCollect && keyCounterText != null)
        {
            StartCoroutine(AnimateKeyCounter());
        }
        
        // Check if all keys collected
        if (HasAllKeys())
        {
            Debug.Log("✅ All keys collected! You can now reach the final door to win!");
            OnAllKeysCollected();
        }
    }
    
    void UpdateKeyCounterUI()
    {
        if (keyCounterText != null)
        {
            keyCounterText.text = string.Format(keyCounterFormat, keysCollected, keysRequiredToWin);
            
            // Change color when all keys collected
            if (HasAllKeys())
            {
                keyCounterText.color = Color.green;
            }
        }
    }
    
    void HideCollectMessage()
    {
        if (keyCollectMessageUI != null)
        {
            keyCollectMessageUI.SetActive(false);
        }
    }
    
    System.Collections.IEnumerator AnimateKeyCounter()
    {
        if (keyCounterText == null) yield break;
        
        Vector3 originalScale = keyCounterText.transform.localScale;
        float duration = 0.3f;
        float elapsed = 0f;
        
        // Scale up
        while (elapsed < duration / 2)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 1.3f, elapsed / (duration / 2));
            keyCounterText.transform.localScale = originalScale * scale;
            yield return null;
        }
        
        // Scale back down
        elapsed = 0f;
        while (elapsed < duration / 2)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(1.3f, 1f, elapsed / (duration / 2));
            keyCounterText.transform.localScale = originalScale * scale;
            yield return null;
        }
        
        keyCounterText.transform.localScale = originalScale;
    }
    
    void OnAllKeysCollected()
    {
        // Optional: Unlock the final door visually
        // Optional: Show a message "Final door unlocked!"
        // Optional: Play special sound/effect
        
        // You can add custom logic here
        // For example, change the color of the final door, add particles, etc.
    }
    
    // Public methods for other scripts to check key status
    public bool HasAllKeys()
    {
        return keysCollected >= keysRequiredToWin;
    }
    
    public int GetKeysCollected()
    {
        return keysCollected;
    }
    
    public int GetKeysRequired()
    {
        return keysRequiredToWin;
    }
    
    public List<string> GetCollectedKeyNames()
    {
        return new List<string>(collectedKeyNames);
    }
    
    /* Usage:
     * 1. Create an empty GameObject in your scene
     * 2. Name it "KeyManager"
     * 3. Add this script to it
     * 4. Set "Keys Required To Win" to 3 (or any number)
     * 5. Create UI elements:
     *    - Canvas → Text (TMP) for key counter (top-left corner)
     *    - Canvas → Panel for collect message popup
     *    - Panel → Text (TMP) for message text
     * 6. Assign UI elements in inspector
     * 7. Place 3 CollectibleKey objects in your scene
     * 8. KeyManager will track all collected keys automatically
     */
}
