# Key Collection System Setup Guide

This guide explains how to set up a collectible key system where players must collect all 3 keys before they can win the game.

## 📦 Components

### `CollectibleKey.cs`
Individual key objects that players can collect. Includes rotation, floating animation, and visual/audio feedback.

### `KeyManager.cs`
Manages the key collection, tracks progress, and updates the UI counter.

### `WinTrigger.cs` (Modified)
Now checks if player has all required keys before allowing them to win.

---

## 🎯 Complete Setup Instructions

### Step 1: Create the KeyManager

1. **Create Empty GameObject**
   - Right-click in Hierarchy → Create Empty
   - Name it "KeyManager"
   - Position doesn't matter (can be at 0, 0, 0)

2. **Add KeyManager Script**
   - Select KeyManager GameObject
   - Add Component → Scripts → Key Manager
   - Set **Keys Required To Win**: 3

### Step 2: Create the Key Counter UI

1. **Create Canvas** (if you don't have one)
   - Right-click Hierarchy → UI → Canvas
   - Set Canvas Scaler to "Scale With Screen Size"
   - Reference Resolution: 1920x1080

2. **Create Key Counter Text**
   - Right-click Canvas → UI → Text - TextMeshPro
   - Name it "KeyCounterText"
   - Position at **top-left corner** of screen
   - Set Anchor preset: Top-Left (hold Alt, click top-left)
   - Set Position: X=20, Y=-20 (from top-left)
   - Set text to "Keys: 0/3"
   - Font Size: 30-40
   - Color: White or Gold
   - Add outline for visibility (Component → Effects → Outline)

3. **Create Key Collect Message Panel** (Optional but recommended)
   - Right-click Canvas → UI → Panel
   - Name it "KeyCollectMessage"
   - Position at **center** of screen
   - Set Anchor: Center
   - Resize to Width=400, Height=100
   - Set Color: Semi-transparent black `RGBA(0, 0, 0, 180)`
   - Add RectTransform: Position Y=100 (slightly above center)

4. **Add Message Text**
   - Right-click KeyCollectMessage Panel → UI → Text - TextMeshPro
   - Name it "KeyMessageText"
   - Set Anchor to stretch (Alt+Shift + bottom-right)
   - Set text to "🔑 Key Collected!"
   - Font Size: 35
   - Color: Gold/Yellow `RGB(255, 215, 0)`
   - Alignment: Center (horizontal and vertical)
   - Font Style: Bold

5. **Hide Message Panel by Default**
   - Select KeyCollectMessage Panel
   - Uncheck the checkbox at top of Inspector to disable it

6. **Connect UI to KeyManager**
   - Select KeyManager GameObject
   - Drag **KeyCounterText** into "Key Counter Text" field
   - Drag **KeyCollectMessage** Panel into "Key Collect Message UI" field
   - Drag **KeyMessageText** into "Key Collect Message Text" field
   - Verify settings:
     - Keys Required To Win: 3
     - Key Counter Format: "Keys: {0}/{1}"
     - Key Collect Message Format: "🔑 {0} Collected!"
     - Message Display Duration: 2 seconds

### Step 3: Create Collectible Keys

1. **Create First Key**
   - Right-click Hierarchy → 3D Object → Cube (or import a key model)
   - Name it "Key_1"
   - Scale it down: X=0.3, Y=0.8, Z=0.1 (key shape)
   - Position it somewhere in your level
   - Add a material/color (e.g., Gold color)

2. **Add Collider**
   - Select Key_1
   - Should already have a Box Collider
   - Check **"Is Trigger"** ✅

3. **Add Rigidbody** (Optional but recommended)
   - Add Component → Physics → Rigidbody
   - Check **"Is Kinematic"** ✅
   - This prevents physics from affecting the key

4. **Add CollectibleKey Script**
   - Add Component → Scripts → Collectible Key
   - Configure settings:
     - **Key Name**: "Key 1" (or "Red Key", "Basement Key", etc.)
     - **Rotate Key**: ✅ Checked (spins the key)
     - **Rotation Speed**: 50
     - **Float Key**: ✅ Checked (bobs up and down)
     - **Float Amplitude**: 0.3
     - **Float Speed**: 2
     - **Show Collect Message**: ✅ Checked

5. **Duplicate for More Keys**
   - Select Key_1
   - Duplicate (Ctrl+D or Cmd+D)
   - Name it "Key_2"
   - Move to a different location
   - Change **Key Name** to "Key 2" in inspector
   - Repeat for Key_3

6. **Optional: Visual Distinction**
   - Give each key a different color/material
   - Use different models for each key
   - Add different particle effects

### Step 4: Update Win Trigger

1. **Select Your Win Trigger GameObject**
   - Find the FinalDoor_WinTrigger (or whatever you named it)

2. **Configure Key Requirement**
   - In WinTrigger script:
     - **Require Keys**: ✅ Checked
     - **Blocked Message**: "🔒 You need all keys to escape!"
     - **Blocked Message Duration**: 3

3. **Optional: Add Blocked Sound**
   - Import a "door locked" sound effect
   - Drag it into "Blocked Sound" field
   - This plays when player tries to win without all keys

### Step 5: Make Sure Player Has Tag

1. **Select Player GameObject**
2. **Set Tag to "Player"** (top of Inspector)
3. If "Player" tag doesn't exist:
   - Click Tag dropdown → Add Tag
   - Create new tag named "Player"
   - Go back and assign it to Player GameObject

---

## 🎮 Testing

1. **Start Play Mode**
2. **Check Key Counter**
   - Should show "Keys: 0/3" at top-left

3. **Collect First Key**
   - Walk into Key_1
   - Key should disappear with animation
   - Message popup: "🔑 Key 1 Collected!"
   - Counter updates: "Keys: 1/3"

4. **Collect Remaining Keys**
   - Collect Key_2: "Keys: 2/3"
   - Collect Key_3: "Keys: 3/3" (turns green)
   - Console message: "✅ All keys collected!"

5. **Try to Win Without Keys** (Before collecting all)
   - Walk to final door trigger
   - Should see blocked message: "🔒 You need all keys to escape!"
   - Door locked sound plays (if assigned)

6. **Win With All Keys**
   - After collecting all 3 keys
   - Walk to final door trigger
   - Win banner appears! 🎉

---

## 🎨 Customization Ideas

### Visual Enhancements

#### Key Models
- Import 3D key models from Unity Asset Store
- Use different colored keys (Red, Blue, Gold)
- Add key icons/symbols to differentiate them

#### Particle Effects
```
1. Create particle system (GameObject → Effects → Particle System)
2. Make it a prefab
3. Assign to "Collect Particles" in CollectibleKey script
4. Spawns when key is collected
```

#### Glowing Keys
```
1. Add Point Light as child of key
2. Set color to match key (gold, blue, etc.)
3. Range: 3, Intensity: 2
4. Optional: Animate intensity for pulsing effect
```

### Audio Enhancements

#### Key Collect Sound
1. Import a key pickup sound (jingle, chime, etc.)
2. Drag into "Collect Sound" in CollectibleKey
3. Plays when collected

#### All Keys Collected Sound
1. In KeyManager script, add `public AudioClip allKeysSound;`
2. In `OnAllKeysCollected()` method, play the sound
3. Use dramatic/victory sound

### UI Enhancements

#### Key Icons Instead of Text
```
1. Create 3 key icon images (empty/locked and full/unlocked versions)
2. Display them visually
3. Fill in icons as keys are collected
```

#### Progress Bar
```
1. Add a Slider UI element
2. Update value: keysCollected / keysRequiredToWin
3. Fill color changes from red → yellow → green
```

### Gameplay Features

#### Named Keys
Give each key a specific name matching a door:
- "Red Key" → unlocks Red Door
- "Basement Key" → unlocks Basement
- "Master Key" → unlocks Exit

#### Key Hints
Show where keys are:
- Add markers on minimap
- Add hints in the environment
- Show distance to nearest key

#### Optional Keys
Make some keys optional but give bonuses:
- Collect all 5 keys → secret ending
- 3 keys minimum → normal ending

---

## 🔧 Advanced Features

### Different Win Requirements

#### Easy Mode (1 Key)
```csharp
// In KeyManager:
public int keysRequiredToWin = 1;
```

#### Hard Mode (5 Keys)
```csharp
public int keysRequiredToWin = 5;
// Place 5 keys around the map
```

### Key Persistence (Save System)

```csharp
// In KeyManager, add:
void SaveKeys()
{
    PlayerPrefs.SetInt("KeysCollected", keysCollected);
    PlayerPrefs.Save();
}

void LoadKeys()
{
    keysCollected = PlayerPrefs.GetInt("KeysCollected", 0);
    UpdateKeyCounterUI();
}

// Call LoadKeys() in Start()
// Call SaveKeys() after CollectKey()
```

### Multiple Key Types

```csharp
// Create different key types:
public enum KeyType { Red, Blue, Gold }

// In WinTrigger, check specific keys:
public bool HasKeyType(KeyType type)
{
    // Check if specific key collected
}
```

---

## 📋 Checklist

- [ ] KeyManager GameObject created in scene
- [ ] KeyManager script attached with Keys Required = 3
- [ ] Canvas created with Key Counter Text (top-left)
- [ ] Key Collect Message Panel created (center popup)
- [ ] Message text created inside panel
- [ ] Message panel disabled by default
- [ ] All UI elements assigned to KeyManager
- [ ] 3 Key GameObjects created and positioned in level
- [ ] Each key has Box Collider (Is Trigger = true)
- [ ] Each key has CollectibleKey script
- [ ] Each key has unique name (Key 1, Key 2, Key 3)
- [ ] Player GameObject has "Player" tag
- [ ] WinTrigger has "Require Keys" enabled
- [ ] Blocked message configured in WinTrigger
- [ ] Tested: Collecting keys updates counter
- [ ] Tested: Message popup appears on collect
- [ ] Tested: Can't win without all keys
- [ ] Tested: Can win with all keys

---

## 🐛 Troubleshooting

### Keys don't get collected
- **Solution**: Make sure Player has "Player" tag
- Check that key collider is set to "Is Trigger"
- Verify Player has a collider (CharacterController counts)

### Key counter doesn't update
- **Solution**: Check KeyManager UI references are assigned
- Make sure TextMeshPro is installed
- Verify Key Counter Text is not null

### Message doesn't show
- **Solution**: Check KeyCollectMessage panel is assigned
- Verify panel is a child of Canvas
- Make sure panel has CanvasGroup or Image component

### Can win without keys
- **Solution**: In WinTrigger, check "Require Keys" is enabled
- Verify KeyManager exists in scene
- Check keysRequiredToWin value

### Keys fall through floor
- **Solution**: Add Rigidbody to key, set "Is Kinematic" = true
- Or parent key to an empty GameObject as holder

### Keys don't rotate/float
- **Solution**: Check "Rotate Key" and "Float Key" are enabled
- Verify rotation speed and float amplitude are not 0

---

## 🎬 Quick Start Summary

1. **Create KeyManager** → Add script → Set required keys to 3
2. **Create UI** → Key counter (top-left) + Collect message popup (center)
3. **Create 3 Keys** → Add cube → Scale → Add CollectibleKey script → Set trigger
4. **Connect** → Assign UI to KeyManager
5. **Update WinTrigger** → Enable "Require Keys"
6. **Test** → Collect keys → Try to win → Success! 🎉

---

**Need help?** Check inline comments in scripts or the other README files (WIN_SYSTEM_README.md, GAMEOVER_SYSTEM_README.md).
