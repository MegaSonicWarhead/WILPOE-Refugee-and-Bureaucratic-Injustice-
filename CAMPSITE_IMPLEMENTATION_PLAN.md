# Campsite Scene Implementation Plan

## Phase 1: Unity Scene Setup (2-3 hours)

### 1. Create Development Scene
```
1. In Unity Editor:
   - Duplicate Campsite.unity to Campsite_3D.unity
   - Keep original as backup
   - Clear test/temporary objects
```

### 2. Unity Camera Setup
```
1. Select Main Camera in hierarchy
2. Add PerspectiveCamera.cs component
3. Configure Transform component:
   - Position: (0, 12, -12)
   - Rotation: X=20, Y=0, Z=0
   - Scale: (1, 1, 1)
4. Camera component settings:
   - Projection: Orthographic
   - Size: 6
   - Clear Flags: Solid Color
   - Background: Set sky color
```

### 3. Unity Layer Setup
```
1. Open Edit > Project Settings > Tags and Layers
2. Add Sorting Layers (in this order):
   - Sky_Background
   - Distant_Environment
   - Far_Trees
   - Camp_Background
   - Main_Camp
   - Front_Camp
   - Foreground_Details
```

### 4. Scene Hierarchy Setup
```
1. Create base structure:
   CampsiteEnvironment (Empty GameObject)
   ├── SkyPanel (Z=15)
   ├── DistantEnvironmentPanel (Z=12)
   ├── TreeLinePanel (Z=10)
   ├── CampBackgroundPanel (Z=5)
   ├── MainCampPanel (Z=0)
   ├── FrontCampPanel (Z=-5)
   └── ForegroundPanel (Z=-10)

2. For each panel:
   - Add Sprite Renderer component
   - Set corresponding Sorting Layer
   - Add ParallaxBackground script
```

## Phase 2: Unity Asset Preparation (4-5 hours)

### 1. Project Window Organization
```
Create folders in Project window:
Assets/
└── Scenes/
    └── Campsite/
        ├── Sprites/
        │   ├── Background/
        │   ├── Environment/
        │   ├── Props/
        │   └── Characters/
        ├── Prefabs/
        ├── Materials/
        └── Scripts/
```

### 2. Sprite Setup
```
1. Import/prepare sprites:
   - Set Texture Type: Sprite (2D and UI)
   - Set Pixels Per Unit: 100
   - Enable Read/Write if needed
   - Apply settings

2. For each depth layer:
   - Create sprite assets
   - Set appropriate Sprite Renderer settings
   - Configure transparency
```

### 3. Prefab Creation
```
1. Create prefabs for repeated elements:
   - Tents
   - Trees
   - Props
   - Character templates
2. Set Z-positions within prefabs
3. Configure Sprite Renderer components
```

## Phase 3: Unity Scene Assembly (3-4 hours)

### 1. Background Implementation
```
1. For each panel in hierarchy:
   - Assign sprites to Sprite Renderer
   - Set Sorting Layer
   - Configure ParallaxBackground values:
     * SkyPanel: 0.05
     * DistantEnvironmentPanel: 0.1
     * TreeLinePanel: 0.2
     * CampBackgroundPanel: 0.4
     * MainCampPanel: 0.7
     * FrontCampPanel: 0.8
     * ForegroundPanel: 0.9
```

### 2. Environment Setup
```
1. Create environment prefabs:
   - Drag sprites to scene
   - Set Z-positions
   - Configure components
   - Create prefabs
   - Delete scene objects
   - Instantiate from prefabs
```

### 3. Collider Setup
```
1. Add appropriate 2D colliders:
   - Box Collider 2D for boundaries
   - Polygon Collider 2D for terrain
   - Circle Collider 2D for interaction zones
2. Set collider Z-positions to match visuals
```

## Phase 4: Unity Lighting (2-3 hours)

### 1. URP Setup (if not done)
```
1. Install Universal RP package
2. Create URP Asset
3. Set Graphics Settings
4. Configure 2D Renderer
```

### 2. Light Components
```
1. Add Unity 2D lights:
   - Global Light 2D for sun/moon
   - Point Light 2D for campfire
   - Freeform Light 2D for ambient
2. Configure light properties:
   - Intensity
   - Color
   - Falloff
   - Blend Style
```

### 3. Shadow Configuration
```
1. Enable 2D shadows in URP settings
2. Add Shadow Caster 2D components
3. Set shadow parameters:
   - Self Shadow
   - Cast Shadows
   - Shadow Distance
```

## Phase 5: Unity Effects (3-4 hours)

### 1. Particle Systems
```
1. Create particle systems:
   - Right-click > Effects > Particle System
2. Configure for:
   - Rain (3 layers with different Z positions)
   - Campfire smoke
   - Ambient dust
   - Fireflies
3. Set Particle System Renderer:
   - Set sorting layer
   - Adjust Z position
```

### 2. Weather Implementation
```
1. Add WeatherSystem script
2. Configure weather transitions
3. Setup particle triggers
4. Add weather sound effects
```

## Phase 6: Character Setup (2-3 hours)

### 1. Character Prefab Setup
```
1. Create character prefab:
   - Add Sprite Renderer
   - Add Rigidbody 2D
   - Add Collider 2D
   - Add character controller script
2. Configure Z-position handling
3. Setup sorting layer changes
```

### 2. Movement System
```
1. Implement character movement:
   - Add input handling
   - Setup movement boundaries
   - Configure collision detection
2. Add depth-based movement scaling
```

## Phase 7: Unity UI Setup (2-3 hours)

### 1. Canvas Configuration
```
1. Create canvases:
   - World Space canvas for floating UI
   - Screen Space canvas for HUD
2. Set proper sorting layers
3. Configure canvas scaling
```

### 2. UI Elements
```
1. Add UI elements:
   - Time display
   - Weather indicators
   - Interaction prompts
2. Set proper Z-positions for world space elements
```

## Phase 8: Testing & Optimization

### Unity Profiler Checks
```
1. Open Window > Analysis > Profiler
2. Check for:
   - Sprite batch breaks
   - Particle system performance
   - Shadow performance
   - Memory usage
```

### Scene View Tests
```
1. Test parallax in Scene view
2. Verify Z-position sorting
3. Check lighting in all angles
4. Verify particle system depths
```

## Common Unity Issues

### 1. Sprite Sorting Issues
```
Fix: 
1. Check Sprite Renderer sorting layer
2. Verify Z position
3. Check parent object positions
```

### 2. Light Bleeding
```
Fix:
1. Adjust light parameters in URP settings
2. Check shadow caster positions
3. Verify light layer masks
```

### 3. Particle Depth Issues
```
Fix:
1. Check Particle System Renderer sorting layer
2. Adjust particle system Z position
3. Verify particle scaling over lifetime
```

Would you like to start with Phase 1 and begin setting up the Unity scene? 