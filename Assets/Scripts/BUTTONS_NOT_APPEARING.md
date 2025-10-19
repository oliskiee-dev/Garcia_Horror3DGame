# Troubleshooting: Game Over Buttons Not Appearing

## Common Causes & Solutions

### ✅ Checklist

Run through these in order:

---

## 1. Buttons Not Created in UI

**Problem:** The button GameObjects don't exist in your hierarchy.

**Solution:** Create the buttons in Unity

1. **Select GameOverBanner Panel**
2. **Right-click → UI → Button - TextMeshPro**
3. **Name it "RestartButton"**
4. **Position it at bottom-center of the panel**
5. **Repeat for MainMenuButton and QuitButton** (optional)

---

## 2. Buttons Not Assigned in Inspector

**Problem:** Buttons exist but aren't connected to the script.

**Solution:** Assign them in Unity Inspector

1. **Select GameOverBanner Panel**
2. **Find GameOverBannerUI component**
3. **In "UI Elements" section:**
   - Drag **RestartButton** into "Restart Button" field
   - Drag **MainMenuButton** into "Main Menu Button" field (optional)
   - Drag **QuitButton** into "Quit Button" field (optional)

---

## 3. Buttons Outside Panel or Hidden

**Problem:** Buttons exist but are positioned outside the visible area.

**Solution:** Position buttons properly

1. **Select RestartButton**
2. **Check RectTransform:**
   - Position should be within panel bounds
   - Try: Pos X=0, Pos Y=-150 (bottom-center)
3. **Make sure it's a child of GameOverBanner Panel**
4. **Check the panel size** - should cover full screen

---

## 4. Buttons Behind Other Elements

**Problem:** Buttons are there but behind text or other UI.

**Solution:** Reorder hierarchy

1. **In GameOverBanner Panel hierarchy:**
   ```
   GameOverBanner (Panel)
   ├── GameOverTitle (Text) <- TOP
   ├── DeathMessage (Text)
   ├── RestartButton <- BOTTOM (renders last/on top)
   ├── MainMenuButton
   └── QuitButton
   ```
2. **Drag buttons to bottom of the list** (they render last)

---

## 5. Panel Too Small or Wrong Anchor

**Problem:** Panel doesn't fill screen, buttons cut off.

**Solution:** Fix panel anchoring

1. **Select GameOverBanner Panel**
2. **In RectTransform:**
   - Click **Anchor Preset** (top-left icon)
   - Hold **Alt + Shift**
   - Click **bottom-right preset** (stretch to fill)
3. **Set offsets to 0:**
   - Left: 0, Top: 0, Right: 0, Bottom: 0

---

## 6. Buttons Disabled in Hierarchy

**Problem:** Button GameObjects are disabled.

**Solution:** Enable them

1. **Select each button (RestartButton, etc.)**
2. **Check the checkbox at top of Inspector is CHECKED ✅**
3. All child objects of GameOverBanner should be enabled

---

## 7. Button Has No Text

**Problem:** Button exists but text is empty or missing.

**Solution:** Add button text

1. **Select RestartButton**
2. **Expand it in hierarchy → find "Text (TMP)" child**
3. **Select the Text child**
4. **Set text to "Restart"**
5. **Make sure text is visible:**
   - Font Size: 24-30
   - Color: White or visible color
   - Alignment: Center

---

## 8. Canvas Not in Correct Mode

**Problem:** Canvas render mode is wrong.

**Solution:** Fix canvas settings

1. **Select Canvas (parent of GameOverBanner)**
2. **Canvas component:**
   - Render Mode: **Screen Space - Overlay**
   - NOT World Space or Camera
3. **Canvas Scaler:**
   - UI Scale Mode: **Scale With Screen Size**
   - Reference Resolution: 1920 x 1080

---

## 9. Check Console for Errors

**Solution:** Look for these messages when game over triggers:

**Good messages:**
```
Restart Button: Found
Main Menu Button: Found
Quit Button: Found
```

**Bad messages:**
```
Restart Button: NOT ASSIGNED
Main Menu Button: NOT ASSIGNED
Quit Button: NOT ASSIGNED
```

If you see "NOT ASSIGNED", go back to Step 2.

---

## 🎯 Quick Setup (Start Fresh)

If nothing works, create buttons from scratch:

### 1. Create Restart Button

1. **Select GameOverBanner Panel**
2. **Right-click → UI → Button - TextMeshPro**
3. **Name: "RestartButton"**
4. **RectTransform:**
   - Anchor: Bottom-Center
   - Pos X: 0, Pos Y: 80 (above bottom)
   - Width: 200, Height: 50
5. **Expand RestartButton → Select Text (TMP) child:**
   - Text: "Restart"
   - Font Size: 24
   - Color: White
   - Alignment: Center
6. **Button colors (optional):**
   - Normal: Dark gray/red
   - Highlighted: Lighter
   - Pressed: Darker

### 2. Duplicate for More Buttons

1. **Duplicate RestartButton (Ctrl+D)**
2. **Rename to "MainMenuButton"**
3. **Move up:** Pos Y: 150
4. **Change text to "Main Menu"**

### 3. Assign to Script

1. **Select GameOverBanner Panel**
2. **GameOverBannerUI component:**
   - Drag **RestartButton** → "Restart Button" field
   - Drag **MainMenuButton** → "Main Menu Button" field

### 4. Test

1. **Press Play**
2. **Trigger game over**
3. **Buttons should appear and be clickable**

---

## 📝 Proper Hierarchy Structure

Your GameOverBanner should look like this:

```
Canvas
└── GameOverBanner (Panel - Has GameOverBannerUI script)
    ├── GameOverTitle (Text - TMP)
    ├── DeathMessage (Text - TMP)
    ├── RestartButton (Button)
    │   └── Text (TMP) <- "Restart"
    ├── MainMenuButton (Button) [Optional]
    │   └── Text (TMP) <- "Main Menu"
    └── QuitButton (Button) [Optional]
        └── Text (TMP) <- "Quit"
```

---

## 🔍 Debug Checklist

When game over triggers, check Console for:

- [x] "GameOverBannerUI: OnEnable called"
- [x] "Restart Button: Found"
- [x] RestartButton is visible on screen
- [x] RestartButton is clickable (cursor appears on hover)
- [x] Clicking button restarts the game

If ANY of these fail, you found the problem!

---

## 🆘 Still Not Working?

**Try this diagnostic:**

1. **Press Play**
2. **Trigger game over**
3. **Alt+Tab to Unity Editor** (keep game running)
4. **In Hierarchy, expand Canvas → GameOverBanner**
5. **Manually enable/disable RestartButton**
6. **Does it appear/disappear in Game view?**

**If YES:** Button exists but something is wrong with positioning/rendering
**If NO:** Button GameObject might not exist or is in wrong place

Share what you see in the Console when game over triggers, and I can help pinpoint the exact issue!
