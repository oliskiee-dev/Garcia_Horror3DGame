# Player Movement & Third Person Camera Setup

This folder contains scripts for third-person player movement and camera control using Unity's **Input System package**.

## Scripts

### `PlayerMovement.cs`
CharacterController-based movement with walking, running (sprint), jumping, and gravity.

### `ThirdPersonCamera.cs` (formerly FirstPersonCamera.cs)
Third-person camera that follows the player with mouse orbit controls, smooth following, and camera collision detection.

---

## Setup Instructions

### 1. Player GameObject Setup

1. **Create/Select Player GameObject**
   - In your scene, create a new GameObject or select your existing player.
   - Name it "Player" (or similar).

2. **Add CharacterController Component**
   - Select the Player GameObject.
   - Add Component → Physics → Character Controller.
   - Adjust the controller's height and radius to match your player model.
   - Set the center position (typically Y = 1 for a 2-unit tall character).

3. **Attach PlayerMovement Script**
   - Select the Player GameObject.
   - Add Component → Scripts → Player Movement.
   - Configure the inspector:
     - **Walk Speed**: 5 (default, adjust to preference)
     - **Run Speed**: 8 (default, adjust to preference)
     - **Gravity**: -9.81 (default physics gravity)
     - **Jump Height**: 1.5 (adjust for higher/lower jumps)
     - **Ground Check**: Leave empty (auto-created) or assign a child Transform at the player's feet
     - **Ground Distance**: 0.4 (sphere radius for ground detection)
     - **Ground Mask**: Set to layers that represent ground (e.g., Default, Terrain)

4. **Assign Input Actions**
   - In the PlayerMovement inspector under "Input Actions":
     - **Move Action**: Click the circle → Select `InputSystem_Actions > Player > Move`
     - **Jump Action**: Click the circle → Select `InputSystem_Actions > Player > Jump`
     - **Sprint Action**: Click the circle → Select `InputSystem_Actions > Player > Sprint`

### 2. Camera Setup

1. **Create Independent Camera GameObject**
   - In your scene hierarchy, create a new Camera GameObject (NOT as a child of the Player).
   - Name it "Third Person Camera" or use the existing Main Camera.
   - Position it anywhere in the scene (it will automatically follow the player).

2. **Attach ThirdPersonCamera Script**
   - Select the Camera GameObject.
   - Add Component → Scripts → Third Person Camera.
   - Configure the inspector:
     - **Mouse Sensitivity**: 100 (default, adjust to preference)
     - **Invert Y**: Unchecked (or check if you prefer inverted Y-axis)
     - **Min Pitch**: -40 (how far down you can look)
     - **Max Pitch**: 80 (how far up you can look)
     - **Camera Distance**: 5 (how far behind the player)
     - **Camera Height**: 2 (height offset above player position)
     - **Min Distance**: 1 (closest the camera can get)
     - **Max Distance**: 10 (farthest the camera can be)
     - **Rotation Smooth Time**: 0.12 (smoothness of rotation, lower = snappier)
     - **Position Smooth Time**: 0.12 (smoothness of following, lower = snappier)
     - **Enable Collision**: Checked (prevents camera clipping through walls)
     - **Collision Layers**: Set to all layers EXCEPT the player layer
     - **Collision Radius**: 0.3 (sphere radius for collision detection)
     - **Player Body**: Drag the Player GameObject here (REQUIRED)

3. **Assign Input Actions**
   - In the ThirdPersonCamera inspector under "Input Actions":
     - **Look Action**: Click the circle → Select `InputSystem_Actions > Player > Look`

4. **Configure Layer Collision (Important!)**
   - To prevent the camera from colliding with the player itself:
     - Select your Player GameObject
     - Set its Layer to "Player" (create this layer if it doesn't exist)
     - In ThirdPersonCamera's "Collision Layers", uncheck the "Player" layer

### 3. Input System Configuration

Your project already has `InputSystem_Actions.inputactions` configured with:
- **Move** (Vector2) → WASD, Arrow Keys, Gamepad Left Stick
- **Look** (Vector2) → Mouse Delta, Gamepad Right Stick
- **Jump** (Button) → Space, Gamepad South Button
- **Sprint** (Button) → Left Shift, Gamepad Left Stick Press

**No additional setup needed** unless you want to customize bindings.

To edit bindings:
1. Open `Assets/InputSystem_Actions.inputactions`
2. Double-click to open the Input Actions editor
3. Modify bindings under the "Player" action map
4. Click "Save Asset"

---

## Controls (Default)

| Action | Keyboard & Mouse | Gamepad |
|--------|------------------|---------|
| Move Forward/Back | W/S or Up/Down Arrow | Left Stick Y |
| Strafe Left/Right | A/D or Left/Right Arrow | Left Stick X |
| Look (Rotate Camera) | Mouse Movement | Right Stick |
| Jump | Space | South Button (A on Xbox) |
| Sprint | Hold Left Shift | Left Stick Press (L3) |
| Unlock Cursor (Debug) | Escape | - |

---

## Troubleshooting

### "InvalidOperationException: You are trying to read Input using UnityEngine.Input class..."
- **Solution**: This error means you're using the old Input class while the project is set to use the Input System package. The scripts have been updated to use `InputActionReference` instead.

### Camera doesn't follow the player
- **Solution**: Make sure the `Player Body` field in ThirdPersonCamera is assigned to the Player GameObject. The camera must NOT be a child of the player.

### Camera clips through walls
- **Solution**: 
  - Ensure "Enable Collision" is checked in ThirdPersonCamera.
  - Verify "Collision Layers" includes the layers your walls are on.
  - Make sure the player layer is EXCLUDED from Collision Layers to prevent self-collision.
  - Adjust "Collision Radius" if needed (larger = more padding from walls).

### Player rotates incorrectly or camera is jumpy
- **Solution**: 
  - The player now rotates to face the camera's forward direction automatically.
  - Adjust "Rotation Smooth Time" and "Position Smooth Time" for smoother/snappier camera.
  - Ensure the Player GameObject's forward direction (blue arrow) points forward initially.

### Player can't jump
- **Solution**: 
  - Check that the Ground Mask in PlayerMovement includes the layer your ground is on.
  - Verify the Ground Check position is at the player's feet.
  - Ensure the Jump Action is assigned in the inspector.

### Movement doesn't work
- **Solution**: 
  - Verify all three Input Action References are assigned in PlayerMovement inspector.
  - Check that the Input Actions asset is enabled (it should be by default).
  - Ensure the CharacterController component is attached.

### Cursor is visible/unlocked
- **Solution**: The cursor locks automatically on Start(). Press Escape to unlock (for debugging). To re-lock, you can add a click-to-lock mechanic or restart the scene.

---

## Customization Ideas

- **Zoom In/Out**: Add mouse scroll wheel input to adjust `cameraDistance` dynamically
- **Camera Shake**: Add screen shake effects during impacts or actions
- **Target Lock**: Add ability to lock camera onto enemies
- **Crouch**: Use the "Crouch" action already in your Input Actions asset
- **Footstep Sounds**: Trigger audio clips based on movement speed and ground check
- **Stamina System**: Limit sprint duration with a stamina variable
- **Smooth Acceleration**: Add lerp to movement instead of instant velocity changes

---

## Notes

- These scripts use `InputActionReference` and `InputActionAsset` which require the Input System package (already installed).
- The cursor locks on play and unlocks with Escape for debugging.
- Ground detection uses a sphere check at the player's feet. Adjust `groundDistance` if needed.
- The camera now follows the player from behind and rotates around them with mouse movement.
- Camera collision prevents clipping through walls using SphereCast detection.
- The player automatically rotates to face the camera's forward direction (horizontal only).

---

**Need help?** Check the inline comments in the scripts or Unity's Input System documentation.
