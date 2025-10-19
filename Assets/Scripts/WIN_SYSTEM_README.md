# Win Banner System Setup Guide

This guide explains how to set up a win banner that displays when the player reaches the final door.

## 📦 Components

### `WinTrigger.cs`
Detects when the player enters the final door area and triggers the win banner.

### `WinBannerUI.cs`
Manages the UI display, animations, and button functionality for the win screen.

---

## 🎯 Setup Instructions

### Step 1: Create the Win Trigger Zone

1. **Create Empty GameObject**
   - Right-click in Hierarchy → Create Empty
   - Name it "FinalDoor_WinTrigger"
   - Position it at your final door location

2. **Add Trigger Collider**
   - Select the WinTrigger GameObject
   - Add Component → Physics → Box Collider
   - Check "Is Trigger" ✅
   - Adjust size to cover the door entrance area (e.g., Size: X=3, Y=3, Z=1)

3. **Attach WinTrigger Script**
   - Add Component → Scripts → Win Trigger
   - Leave "Win Banner UI" empty for now (we'll assign it later)
   - Configure settings:
     - **Pause Game On Win**: ✅ Checked (stops gameplay)
     - **Lock Cursor On Win**: ❌ Unchecked (shows cursor for buttons)

4. **Tag Your Player**
   - Select your Player GameObject
   - In Inspector, set Tag to "Player" (create if it doesn't exist)

### Step 2: Create the Win Banner UI

1. **Create Canvas** (if you don't have one)
   - Right-click in Hierarchy → UI → Canvas
   - Set Canvas Scaler to "Scale With Screen Size"
   - Reference Resolution: 1920x1080

2. **Create Win Banner Panel**
   - Right-click Canvas → UI → Panel
   - Name it "WinBanner"
   - Set Anchor to stretch full screen (hold Alt+Shift, click bottom-right anchor preset)
   - Set Color to semi-transparent black: `RGBA(0, 0, 0, 200)`

3. **Add Win Title Text**
   - Right-click WinBanner → UI → Text - TextMeshPro
   - Name it "WinTitle"
   - Position at top center
   - Set text to "YOU WIN!"
   - Font Size: 80-100
   - Color: Gold or White
   - Alignment: Center
   - Enable Best Fit if needed

4. **Add Win Message Text**
   - Right-click WinBanner → UI → Text - TextMeshPro
   - Name it "WinMessage"
   - Position below title
   - Set text to "Congratulations! You've escaped!"
   - Font Size: 30-40
   - Alignment: Center

5. **Add Restart Button**
   - Right-click WinBanner → UI → Button - TextMeshPro
   - Name it "RestartButton"
   - Position in center-bottom area
   - Change button text to "Restart"
   - Style the button (colors, size, etc.)

6. **Optional: Add More Buttons**
   - Main Menu Button (if you have a menu scene)
   - Quit Button (to exit game)

### Step 3: Connect Everything

1. **Attach WinBannerUI Script**
   - Select the WinBanner Panel
   - Add Component → Scripts → Win Banner UI
   - Assign references:
     - **Win Title Text**: Drag WinTitle here
     - **Win Message Text**: Drag WinMessage here
     - **Restart Button**: Drag RestartButton here
     - **Main Menu Button**: (optional)
     - **Quit Button**: (optional)
   - Customize text in inspector if desired

2. **Connect WinTrigger to UI**
   - Select the FinalDoor_WinTrigger GameObject
   - Drag the WinBanner Panel into the "Win Banner UI" field

3. **Hide the Win Banner by Default**
   - Select WinBanner Panel
   - Uncheck the checkbox at the top of Inspector to disable it
   - It will automatically show when triggered

### Step 4: Optional Enhancements

#### Add Win Sound Effect
1. Import a victory/win sound effect
2. Select FinalDoor_WinTrigger
3. Drag sound clip into "Win Sound" field
4. The sound will play when player wins

#### Add Fade Animation
- Win banner automatically fades in (already implemented)
- Adjust "Fade In Duration" in WinBannerUI (default: 0.5 seconds)

#### Customize Win Message
- Select WinBanner Panel
- In WinBannerUI script, edit:
  - **Win Title**: "YOU WIN!", "ESCAPED!", "VICTORY!", etc.
  - **Win Message**: Add congratulations, stats, or story text

---

## 🎮 Testing

1. **Test Setup:**
   - Press Play
   - Navigate to the final door
   - Walk through the trigger area

2. **Expected Behavior:**
   - Win banner appears with fade-in animation
   - Game pauses (Time.timeScale = 0)
   - Cursor becomes visible and unlocked
   - Restart button works to reload scene
   - Win sound plays (if assigned)

3. **Troubleshooting:**
   - **Banner doesn't show**: Check that WinBanner is assigned in WinTrigger
   - **Trigger doesn't activate**: Make sure Player has "Player" tag
   - **Trigger is too small**: Increase Box Collider size
   - **Buttons don't work**: Check button onClick events in WinBannerUI
   - **Text is missing**: Install TextMesh Pro (Window → TextMeshPro → Import TMP Essentials)

---

## 🎨 Customization Ideas

### Visual Enhancements
- Add particle effects (confetti, sparkles) when winning
- Animate the banner with scale/rotation animations
- Add a background image or victory screen art
- Display game stats (time, score, collectibles)

### Gameplay Features
- Play a victory animation on the player
- Unlock next level button
- Show achievements or medals
- Display leaderboard or high scores

### Advanced Features
```csharp
// In WinTrigger.cs, add:
public float completionTime;
public int itemsCollected;

void Start()
{
    completionTime = Time.time;
}

void TriggerWin()
{
    // Show stats in UI
    completionTime = Time.time;
    Debug.Log($"Completed in {completionTime:F2} seconds!");
}
```

---

## 📋 Checklist

- [ ] FinalDoor_WinTrigger created at final door
- [ ] Box Collider added and set to "Is Trigger"
- [ ] Player GameObject has "Player" tag
- [ ] Canvas created with WinBanner panel
- [ ] Win title and message text added
- [ ] Restart button created and styled
- [ ] WinBannerUI script attached to panel
- [ ] All UI references assigned in WinBannerUI
- [ ] WinBanner assigned to WinTrigger
- [ ] WinBanner disabled by default in hierarchy
- [ ] Tested in Play mode

---

## 🔧 Script Reference

### WinTrigger Public Methods
```csharp
RestartGame()     // Reloads current scene
GoToMainMenu()    // Loads main menu scene
QuitGame()        // Exits application
```

### WinBannerUI Properties
- `winTitle` - Main title text
- `winMessage` - Description/congratulations text
- `animateOnShow` - Enable fade-in animation
- `fadeInDuration` - Animation duration

---

**Need help?** Check the inline comments in the scripts or Unity's documentation on Triggers and UI.
