using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinBannerUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI winTitleText;
    public TextMeshProUGUI winMessageText;
    public Button restartButton;
    public Button mainMenuButton;
    public Button quitButton;
    
    [Header("Text Content")]
    [TextArea(2, 4)]
    public string winTitle = "YOU WIN!";
    [TextArea(2, 4)]
    public string winMessage = "Congratulations! You've escaped the horror!\n\nPress ESC to continue...";
    
    [Header("Animation")]
    public bool animateOnShow = true;
    public float fadeInDuration = 0.5f;
    
    private CanvasGroup canvasGroup;
    private WinTrigger winTrigger;
    
    void Awake()
    {
        // Get or add CanvasGroup for fade animation
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null && animateOnShow)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        // Find the WinTrigger in the scene
        winTrigger = FindFirstObjectByType<WinTrigger>();
        
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
    }
    
    void OnEnable()
    {
        // Set text content
        if (winTitleText != null)
        {
            winTitleText.text = winTitle;
        }
        
        if (winMessageText != null)
        {
            winMessageText.text = winMessage;
        }
        
        // Unlock cursor when win banner shows
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Animate fade in
        if (animateOnShow && canvasGroup != null)
        {
            StartCoroutine(FadeIn());
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
    
    void OnRestartClicked()
    {
        if (winTrigger != null)
        {
            winTrigger.RestartGame();
        }
    }
    
    void OnMainMenuClicked()
    {
        if (winTrigger != null)
        {
            winTrigger.GoToMainMenu();
        }
    }
    
    void OnQuitClicked()
    {
        if (winTrigger != null)
        {
            winTrigger.QuitGame();
        }
    }
    
    /* Usage:
     * 1. This script goes on your Win Banner Panel (child of Canvas)
     * 2. Assign TextMeshProUGUI components for title and message
     * 3. Assign buttons for restart, main menu, and quit
     * 4. Customize win text in the inspector
     * 5. The banner will automatically fade in when shown
     */
}
