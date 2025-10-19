using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverBannerUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI gameOverTitleText;
    public TextMeshProUGUI deathMessageText;
    public Button restartButton;
    public Button mainMenuButton;
    public Button quitButton;
    
    [Header("Text Content")]
    [TextArea(2, 4)]
    public string gameOverTitle = "GAME OVER";
    [TextArea(2, 4)]
    public string defaultDeathMessage = "You Died\n\nPress Restart to try again";
    
    [Header("Animation")]
    public bool animateOnShow = true;
    public float fadeInDuration = 0.8f;
    public bool shakeOnShow = false;
    public float shakeDuration = 0.3f;
    public float shakeIntensity = 10f;
    
    [Header("Screen Effect (Optional)")]
    public Image bloodVignetteOverlay;
    public float vignetteAlpha = 0.5f;
    
    private CanvasGroup canvasGroup;
    private GameOverTrigger gameOverTrigger;
    private RectTransform rectTransform;
    private string currentDeathMessage;
    
    void Awake()
    {
        // Get components
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null && animateOnShow)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        rectTransform = GetComponent<RectTransform>();
        
        // Find the GameOverTrigger in the scene
        gameOverTrigger = FindFirstObjectByType<GameOverTrigger>();
        
        // Setup button listeners
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }
        
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitClicked);
        }
        
        currentDeathMessage = defaultDeathMessage;
    }
    
    void OnEnable()
    {
        Debug.Log("GameOverBannerUI: OnEnable called - banner is now active!");
        
        // Check buttons
        Debug.Log($"Restart Button: {(restartButton != null ? "Found" : "NOT ASSIGNED")}");
        Debug.Log($"Main Menu Button: {(mainMenuButton != null ? "Found" : "NOT ASSIGNED")}");
        Debug.Log($"Quit Button: {(quitButton != null ? "Found" : "NOT ASSIGNED")}");
        
        // Set text content
        if (gameOverTitleText != null)
        {
            gameOverTitleText.text = gameOverTitle;
            Debug.Log($"Set title to: {gameOverTitle}");
        }
        else
        {
            Debug.LogWarning("GameOverBannerUI: gameOverTitleText is not assigned!");
        }
        
        if (deathMessageText != null)
        {
            deathMessageText.text = currentDeathMessage;
            Debug.Log($"Set death message to: {currentDeathMessage}");
        }
        else
        {
            Debug.LogWarning("GameOverBannerUI: deathMessageText is not assigned!");
        }
        
        // Unlock cursor when game over banner shows
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Animate fade in
        if (animateOnShow && canvasGroup != null)
        {
            StartCoroutine(FadeIn());
        }
        
        // Animate shake
        if (shakeOnShow && rectTransform != null)
        {
            StartCoroutine(Shake());
        }
        
        // Show blood vignette
        if (bloodVignetteOverlay != null)
        {
            StartCoroutine(FadeInVignette());
        }
    }
    
    public void SetDeathMessage(string message)
    {
        currentDeathMessage = message;
        if (deathMessageText != null && gameObject.activeInHierarchy)
        {
            deathMessageText.text = message;
        }
    }
    
    System.Collections.IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0f;
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Use unscaled for paused game
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
    }
    
    System.Collections.IEnumerator Shake()
    {
        Vector3 originalPosition = rectTransform.localPosition;
        float elapsedTime = 0f;
        
        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            
            rectTransform.localPosition = originalPosition + new Vector3(x, y, 0f);
            yield return null;
        }
        
        rectTransform.localPosition = originalPosition;
    }
    
    System.Collections.IEnumerator FadeInVignette()
    {
        if (bloodVignetteOverlay == null) yield break;
        
        Color color = bloodVignetteOverlay.color;
        color.a = 0f;
        bloodVignetteOverlay.color = color;
        
        float elapsedTime = 0f;
        float duration = fadeInDuration * 0.5f; // Faster than main fade
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            color.a = Mathf.Lerp(0f, vignetteAlpha, elapsedTime / duration);
            bloodVignetteOverlay.color = color;
            yield return null;
        }
        
        color.a = vignetteAlpha;
        bloodVignetteOverlay.color = color;
    }
    
    void OnRestartClicked()
    {
        if (gameOverTrigger != null)
        {
            gameOverTrigger.RestartGame();
        }
    }
    
    void OnMainMenuClicked()
    {
        if (gameOverTrigger != null)
        {
            gameOverTrigger.GoToMainMenu();
        }
    }
    
    void OnQuitClicked()
    {
        if (gameOverTrigger != null)
        {
            gameOverTrigger.QuitGame();
        }
    }
    
    /* Usage:
     * 1. This script goes on your Game Over Banner Panel (child of Canvas)
     * 2. Assign TextMeshProUGUI components for title and death message
     * 3. Assign buttons for restart, main menu, and quit
     * 4. Customize game over text in the inspector
     * 5. Optional: Add blood vignette image for horror effect
     * 6. Enable shake effect for dramatic impact
     * 7. The banner will automatically fade in when shown
     */
}
