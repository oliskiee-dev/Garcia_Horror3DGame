# Troubleshooting: Game Over Banner Not Showing

## ✅ Step-by-Step Checklist

### 1. Check GameOverBanner Panel Setup

**In Unity Hierarchy:**
- [ ] Canvas exists in scene
- [ ] GameOverBanner Panel exists as child of Canvas
- [ ] GameOverBanner Panel has the **GameOverBannerUI** script attached
- [ ] GameOverBanner Panel is **DISABLED** by default (unchecked checkbox in Inspector)

**Important:** The panel must be disabled at start. It will be enabled by the SpikeTrap script.

### 2. Check SpikeTrap Setup

**Select your Spike GameObject:**
- [ ] SpikeTrap script is attached
- [ ] "Game Over Banner UI" field has the **GameOverBanner Panel** assigned (drag it from hierarchy)
- [ ] Spike has a Collider component
- [ ] Collider has "Is Trigger" ✅ CHECKED
- [ ] Death Message is set (e.g., "Impaled by Spikes")

### 3. Check Player Setup

**Select Player GameObject:**
- [ ] Player has Tag set to **"Player"** (top of Inspector)
- [ ] Player has CharacterController or Collider component

### 4. Check GameOverBannerUI References

**Select GameOverBanner Panel:**
- [ ] GameOverBannerUI script is attached
- [ ] "Game Over Title Text" field is assigned (drag TextMeshProUGUI component)
- [ ] "Death Message Text" field is assigned (drag TextMeshProUGUI component)
- [ ] "Restart Button" field is assigned (optional but recommended)

### 5. Run Diagnostic Test

**When you walk into the spikes, check the Console for these messages:**

✅ **Expected messages if working:**
```
💀 Player hit spikes! Impaled by Spikes
Showing Game Over Banner...
GameOver Banner UI found - activating it now!
Setting death message: Impaled by Spikes
GameOverBannerUI: OnEnable called - banner is now active!
Set title to: GAME OVER
Set death message to: Impaled by Spikes
```

❌ **Error messages and fixes:**

| Error Message | Problem | Solution |
|--------------|---------|----------|
| "Game Over Banner UI not assigned to SpikeTrap!" | Banner not assigned | Drag GameOverBanner panel into SpikeTrap's field |
| "GameOverBannerUI component not found..." | Script missing | Add GameOverBannerUI script to banner panel |
| No message at all | Player tag wrong | Set Player tag to "Player" |
| "gameOverTitleText is not assigned!" | UI not connected | Assign text elements in GameOverBannerUI |

---

## 🔧 Common Issues & Fixes

### Issue 1: Banner is Already Active in Scene
**Problem:** If GameOverBanner is enabled by default, it shows immediately when game starts.
**Fix:** Select GameOverBanner Panel → Uncheck the checkbox at top of Inspector

### Issue 2: Banner Behind Other UI
**Problem:** Banner exists but is behind other UI elements.
**Fix:** In Canvas hierarchy, drag GameOverBanner to the BOTTOM (renders last/on top)

### Issue 3: Canvas Not in Screen Space
**Problem:** Canvas render mode might be wrong.
**Fix:** 
- Select Canvas
- Set Render Mode to "Screen Space - Overlay"

### Issue 4: Banner Too Small or Invisible
**Problem:** Banner panel might be tiny or transparent.
**Fix:**
- Select GameOverBanner Panel
- Set Anchor Preset to Stretch (Alt+Shift + bottom-right anchor)
- Set Color to visible (not fully transparent)

### Issue 5: TextMeshPro Not Installed
**Problem:** Text fields are broken or missing.
**Fix:**
- Window → TextMeshPro → Import TMP Essential Resources
- Recreate text elements as Text - TextMeshPro

---

## 🎯 Quick Fix (Start Fresh)

If nothing works, follow this from scratch:

1. **Delete old GameOverBanner** (if it exists)

2. **Create new Canvas:**
   - Right-click Hierarchy → UI → Canvas
   - Canvas Scaler → Scale With Screen Size
   - Reference Resolution: 1920x1080

3. **Create GameOverBanner Panel:**
   - Right-click Canvas → UI → Panel
   - Name: "GameOverBanner"
   - Anchor: Stretch to full screen (Alt+Shift + bottom-right)
   - Color: Dark red/black `RGBA(20, 0, 0, 230)`

4. **Add Title Text:**
   - Right-click GameOverBanner → UI → Text - TextMeshPro
   - Name: "GameOverTitle"
   - Text: "GAME OVER"
   - Font Size: 100
   - Color: Red
   - Alignment: Center
   - Position at top-center

5. **Add Message Text:**
   - Right-click GameOverBanner → UI → Text - TextMeshPro
   - Name: "DeathMessage"
   - Text: "You Died"
   - Font Size: 40
   - Alignment: Center
   - Position at center

6. **Add Restart Button:**
   - Right-click GameOverBanner → UI → Button - TextMeshPro
   - Name: "RestartButton"
   - Text: "Restart"
   - Position at bottom-center

7. **Add GameOverBannerUI Script:**
   - Select GameOverBanner Panel
   - Add Component → GameOverBannerUI
   - Assign:
     - Game Over Title Text → Drag GameOverTitle
     - Death Message Text → Drag DeathMessage
     - Restart Button → Drag RestartButton

8. **DISABLE the Panel:**
   - Select GameOverBanner Panel
   - Uncheck the checkbox at top of Inspector

9. **Assign to SpikeTrap:**
   - Select your Spike GameObject
   - Find SpikeTrap script
   - Drag GameOverBanner Panel into "Game Over Banner UI" field

10. **Test:**
    - Press Play
    - Walk into spikes
    - Banner should appear!

---

## 📝 What Console Should Show

When working correctly:
```
💀 Player hit spikes! Impaled by Spikes
Showing Game Over Banner...
GameOver Banner UI found - activating it now!
Setting death message: Impaled by Spikes
GameOverBannerUI: OnEnable called - banner is now active!
Set title to: GAME OVER
Set death message to: Impaled by Spikes
```

If you see all these messages but still no banner:
- Check Canvas render mode (should be Screen Space - Overlay)
- Check if banner is behind camera or other objects
- Check if Canvas Scaler settings are correct
- Make sure banner panel has an Image component with visible color

---

## 🆘 Still Not Working?

Share the console output when you hit the spikes, and I can help diagnose the exact issue!
