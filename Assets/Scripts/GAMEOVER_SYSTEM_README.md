# Game Over Banner System Setup Guide

This guide explains how to set up a game over banner that displays when the player dies or fails.

## 📦 Components

### `GameOverTrigger.cs`
Detects when the player should die (trigger zones, enemy contact, traps, etc.) and triggers the game over banner.

### `GameOverBannerUI.cs`
Manages the UI display, animations, effects, and button functionality for the game over screen.

---

## 🎯 Setup Instructions

### Step 1: Create the Game Over Trigger

#### Option A: Death Zone (Fall off map)

1. **Create Empty GameObject**
   - Right-click in Hierarchy → Create Empty
   - Name it "DeathZone"
   - Position it below the map (where players would fall)

2. **Add Trigger Collider**
   - Select the DeathZone GameObject
   - Add Component → Physics → Box Collider
   - Check "Is Trigger" ✅
   - Make it LARGE to cover the entire fall area (e.g., Size: X=100, Y=10, Z=100)

3. **Attach GameOverTrigger Script**
   - Add Component → Scripts → Game Over Trigger
   - Set **Trigger Type**: "OnTriggerEnter"
   - Set **Death Message**: "You fell into the abyss"
   - Leave "Game Over Banner UI" empty for now

#### Option B: Enemy Kill Zone

1. **Select your enemy GameObject**
2. **Add GameOverTrigger Script**
   - Set **Trigger Type**: "OnTriggerEnter" or "OnCollisionEnter"
   - Set **Death Message**: "Killed by [Enemy Name]"
3. **Ensure enemy has a collider**
   - Add Collider if missing
   - For OnTriggerEnter: Check "Is Trigger" ✅
   - For OnCollisionEnter: Leave "Is Trigger" unchecked

#### Option C: Trap (Spikes, Fire, Poison, etc.)

1. **Select your trap GameObject**
2. **Add GameOverTrigger Script**
   - Set **Trigger Type**: "OnTriggerEnter"
   - Set **Death Message**: "Impaled by spikes" (or custom)
   - **Enable Camera Shake**: ✅ Checked (for impact effect)
   - **Shake Duration**: 0.3 seconds
   - **Shake Intensity**: 0.5
   - **Delay Before Banner**: 0 (instant) or 0.5 (brief pause)
3. **Add Trigger Collider if needed**
   - Add Box/Sphere Collider
   - Check "Is Trigger" ✅
4. **Optional: Add Death Effects**
   - **Game Over Sound**: Add impact/stabbing sound
   - **Death Effect Prefab**: Add blood particle system

#### Option D: Manual Trigger (For Health System)

1. **Create Empty GameObject** in scene
   - Name it "GameOverManager"
   - Add GameOverTrigger Script
   - Set **Trigger Type**: "Manual"
2. **From your PlayerHealth script**, call:
```csharp
GameOverTrigger trigger = FindObjectOfType<GameOverTrigger>();
trigger.deathMessage = "You ran out of health";
trigger.TriggerGameOver();
```

### Step 2: Create the Game Over Banner UI

1. **Create Canvas** (if you don't have one)
   - Right-click in Hierarchy → UI → Canvas
   - Set Canvas Scaler to "Scale With Screen Size"
   - Reference Resolution: 1920x1080

2. **Create Game Over Panel**
   - Right-click Canvas → UI → Panel
   - Name it "GameOverBanner"
   - Set Anchor to stretch full screen (Alt+Shift + bottom-right anchor)
   - Set Color to dark red/black: `RGBA(20, 0, 0, 230)` for horror feel

3. **Add Game Over Title Text**
   - Right-click GameOverBanner → UI → Text - TextMeshPro
   - Name it "GameOverTitle"
   - Position at top-center
   - Set text to "GAME OVER"
   - Font Size: 80-120
   - Color: Blood Red `RGB(180, 0, 0)` or White
   - Alignment: Center
   - Font Style: Bold

4. **Add Death Message Text**
   - Right-click GameOverBanner → UI → Text - TextMeshPro
   - Name it "DeathMessage"
   - Position in center
   - Set text to "You Died"
   - Font Size: 40-50
   - Color: White or Light Gray
   - Alignment: Center

5. **Add Restart Button**
   - Right-click GameOverBanner → UI → Button - TextMeshPro
   - Name it "RestartButton"
   - Position in lower-center area
   - Change button text to "Restart"
   - Style with dark colors (fits horror theme)

6. **Optional: Add Main Menu Button**
   - Right-click GameOverBanner → UI → Button - TextMeshPro
   - Name it "MainMenuButton"
   - Position below Restart button
   - Change text to "Main Menu"

7. **Optional: Add Quit Button**
   - Right-click GameOverBanner → UI → Button - TextMeshPro
   - Name it "QuitButton"
   - Position below Main Menu button
   - Change text to "Quit Game"

8. **Optional: Add Blood Vignette Effect** (Horror Enhancement)
   - Right-click GameOverBanner → UI → Image
   - Name it "BloodVignette"
   - Set Anchor to stretch full screen
   - Source Image: Use a vignette texture (dark edges, transparent center)
   - Or create one: Red radial gradient image
   - Set Color tint to blood red `RGBA(180, 0, 0, 128)`
   - Move to top of hierarchy (render last/on top)

### Step 3: Connect Everything

1. **Attach GameOverBannerUI Script**
   - Select the GameOverBanner Panel
   - Add Component → Scripts → Game Over Banner UI
   - Assign references:
     - **Game Over Title Text**: Drag GameOverTitle here
     - **Death Message Text**: Drag DeathMessage here
     - **Restart Button**: Drag RestartButton here
     - **Main Menu Button**: (optional) Drag MainMenuButton
     - **Quit Button**: (optional) Drag QuitButton
     - **Blood Vignette Overlay**: (optional) Drag BloodVignette image
   - Configure settings:
     - **Animate On Show**: ✅ Checked (fade-in effect)
     - **Fade In Duration**: 0.8 seconds (slower for dramatic effect)
     - **Shake On Show**: ✅ Checked (screen shake for impact)
     - **Shake Duration**: 0.3 seconds
     - **Shake Intensity**: 10-20
     - **Vignette Alpha**: 0.5 (if using blood vignette)

2. **Connect GameOverTrigger to UI**
   - Select your GameOverTrigger GameObject(s)
   - Drag the GameOverBanner Panel into the "Game Over Banner UI" field

3. **Hide the Game Over Banner by Default**
   - Select GameOverBanner Panel
   - Uncheck the checkbox at top of Inspector to disable it
   - It will automatically show when triggered

4. **Make Sure Player Has Tag**
   - Select your Player GameObject
   - Set Tag to "Player"

### Step 4: Optional Enhancements

#### Add Game Over Sound
1. Import a death/game over sound effect
2. Select your GameOverTrigger GameObject
3. Drag sound clip into "Game Over Sound" field
4. Sound plays when player dies

#### Add Camera Shake (Dramatic Impact)
1. Select GameOverTrigger GameObject
2. **Enable "Shake Camera"**: ✅ Checked
3. **Shake Duration**: 0.3 seconds (quick shake)
4. **Shake Intensity**: 0.5 (moderate shake) or higher for more impact
5. Works great for spikes, explosions, impacts

#### Add Death Effect Particles (Blood, Explosion, etc.)
1. **Create Particle System:**
   - GameObject → Effects → Particle System
   - Customize (blood splatter, smoke, explosion, etc.)
   - Make it a Prefab (drag to Assets folder)
   - Add script to auto-destroy after playing
2. **Assign to GameOverTrigger:**
   - Drag particle prefab into "Death Effect Prefab" field
   - Spawns at death location when triggered

#### Add Death Delay (For Dramatic Effect)
1. Select GameOverTrigger GameObject
2. Set **"Delay Before Banner"**:
   - **0** = Instant game over (good for spikes, traps)
   - **0.5** = Brief pause (good for dramatic deaths)
   - **1.5** = Long pause (good for fall deaths, allows animation)

#### Multiple Death Triggers
- Create multiple GameOverTrigger objects for different death causes
- Each can have a unique death message
- All can reference the same GameOverBanner UI

#### Custom Death Messages by Enemy
```csharp
// In enemy script:
GameOverTrigger trigger = GetComponent<GameOverTrigger>();
trigger.deathMessage = "Killed by Zombie";
trigger.TriggerGameOver();
```

---

## 🎮 Testing

1. **Test Setup:**
   - Press Play
   - Walk into the death trigger zone
   - Or trigger death condition

2. **Expected Behavior:**
   - Game Over banner appears with fade-in animation
   - Screen shakes briefly (if enabled)
   - Blood vignette fades in (if added)
   - Game pauses (Time.timeScale = 0)
   - Cursor becomes visible
   - Death message displays
   - Game Over sound plays (if assigned)
   - Restart button reloads scene

3. **Troubleshooting:**
   - **Banner doesn't show**: Check GameOverBanner assigned in trigger
   - **Trigger doesn't activate**: Make sure Player has "Player" tag
   - **No collision detected**: Check if collider is set to "Is Trigger"
   - **Buttons don't work**: Verify button references in GameOverBannerUI
   - **Text missing/broken**: Install TextMesh Pro essentials
   - **Trigger activates on wrong objects**: Only Player should have "Player" tag

---

## 🎨 Use Cases & Examples

### 1. Death Zone (Fall off Edge)
```
GameObject: DeathZone (below map)
Collider: Box Collider (Is Trigger = true, Large size)
Trigger Type: OnTriggerEnter
Death Message: "You fell to your death"
```

### 2. Enemy Contact
```
GameObject: Enemy
Collider: Already has collider
Trigger Type: OnCollisionEnter or OnTriggerEnter
Death Message: "Killed by [Enemy Name]"
```

### 3. Environmental Hazard
```
GameObject: Spikes, Fire, Acid
Collider: Box/Sphere Collider (Is Trigger = true)
Trigger Type: OnTriggerEnter
Death Message: "Impaled by spikes" / "Burned alive" / "Dissolved in acid"
```

### 4. Health System Integration
```csharp
// In PlayerHealth.cs:
public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    private GameOverTrigger gameOverTrigger;
    
    void Start()
    {
        gameOverTrigger = FindObjectOfType<GameOverTrigger>();
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameOverTrigger.deathMessage = "You ran out of health";
            gameOverTrigger.TriggerGameOver();
        }
    }
}
```

### 5. Time Limit
```csharp
// In GameManager.cs:
public class GameManager : MonoBehaviour
{
    public float timeLimit = 300f; // 5 minutes
    private GameOverTrigger gameOverTrigger;
    
    void Start()
    {
        gameOverTrigger = FindObjectOfType<GameOverTrigger>();
    }
    
    void Update()
    {
        timeLimit -= Time.deltaTime;
        if (timeLimit <= 0)
        {
            gameOverTrigger.deathMessage = "Time ran out";
            gameOverTrigger.TriggerGameOver();
        }
    }
}
```

---

## 🎨 Horror-Themed Customization

### Visual Enhancements
- **Blood splatter animation** on screen
- **Screen desaturation** (gray out the scene)
- **Distortion effects** (chromatic aberration)
- **Glitch effects** when dying
- **Camera shake** on death
- **Slow motion** death sequence

### Audio Enhancements
- **Death scream** sound effect
- **Heartbeat** fading out sound
- **Dramatic horror music** on game over
- **Ambient horror sounds** (whispers, echoes)

### Text Variations
```
"You Died"
"Game Over"
"No Escape"
"They Got You"
"Death Has Found You"
"The Darkness Consumed You"
"Your Journey Ends Here"
```

---

## 📋 Checklist

- [ ] GameOverTrigger created at death location(s)
- [ ] Collider added and configured (Is Trigger if needed)
- [ ] Player GameObject has "Player" tag
- [ ] Canvas created with GameOverBanner panel
- [ ] Game Over title and death message text added
- [ ] Restart button (and optional Menu/Quit buttons) created
- [ ] GameOverBannerUI script attached to panel
- [ ] All UI references assigned in GameOverBannerUI
- [ ] GameOverBanner assigned to GameOverTrigger(s)
- [ ] GameOverBanner disabled by default in hierarchy
- [ ] Death message customized for each trigger
- [ ] Game over sound added (optional)
- [ ] Blood vignette effect added (optional)
- [ ] Tested in Play mode

---

## 🔧 Script Reference

### GameOverTrigger Public Methods
```csharp
TriggerGameOver()  // Manually trigger game over
RestartGame()      // Reloads current scene
GoToMainMenu()     // Loads main menu scene
QuitGame()         // Exits application
```

### GameOverTrigger Properties
- `triggerType` - How game over is triggered (Trigger/Collision/Manual)
- `deathMessage` - Custom death message for this trigger
- `pauseGameOnGameOver` - Pause game when player dies
- `gameOverSound` - Audio clip to play on death

### GameOverBannerUI Public Methods
```csharp
SetDeathMessage(string message)  // Change the death message dynamically
```

### GameOverBannerUI Properties
- `gameOverTitle` - Main title (default: "GAME OVER")
- `defaultDeathMessage` - Fallback death message
- `animateOnShow` - Enable fade-in animation
- `shakeOnShow` - Enable screen shake effect
- `vignetteAlpha` - Opacity of blood vignette overlay

---

**Need help?** Check the inline comments in the scripts or Unity's documentation on Triggers and Collisions.
