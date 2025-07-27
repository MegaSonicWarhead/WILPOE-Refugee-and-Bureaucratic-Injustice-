# Campsite Scene Implementation Plan

## Phase 1: Scene Setup (2-3 hours)

### 1. Create Development Scene
```
- Duplicate Campsite.unity to Campsite_3D.unity
- Keep original as backup
- Clear test/temporary objects
```

### 2. Camera Configuration
```
1. Add PerspectiveCamera.cs to main camera
2. Camera settings for outdoor space:
   - Position: (0, 12, -12)
   - Rotation: X=20 degrees (more top-down for camp view)
   - Orthographic size: 6 (wider view for camp area)
   - Clear Flags: Solid Color (for sky)
```

### 3. Layer Organization
```
Sorting Layers (Edit > Project Settings > Tags and Layers):
1. Sky_Background (1000)
2. Distant_Environment (800)
3. Far_Trees (600)
4. Camp_Background (400)
5. Main_Camp (0)
6. Front_Camp (-400)
7. Foreground_Details (-600)
```

## Phase 2: Environment Preparation (4-5 hours)

### 1. Sky and Background
```
1. Create layers for:
   - Sky gradient
   - Distant mountains/hills
   - Far treeline
   - Environmental fog
2. Save each as separate PNG with transparency
3. Extend edges for parallax scrolling
```

### 2. Camp Elements
```
1. Separate existing camp assets:
   - Ground/terrain base
   - Tent layers (back to front)
   - Props and facilities
   - Character areas
   - Campfire and effects
2. Create shadow maps for:
   - Tents
   - Trees
   - Large props
```

### 3. Asset Organization
```
Assets/Scenes/Campsite/
├── Environment/
│   ├── Sky/
│   ├── Terrain/
│   └── Vegetation/
├── Structures/
│   ├── Tents/
│   └── Props/
├── Effects/
│   ├── Weather/
│   ├── Fire/
│   └── Ambient/
└── Characters/
    ├── Refugees/
    └── Shadows/
```

## Phase 3: Scene Construction (3-4 hours)

### 1. Environment Setup
```
Scene Hierarchy:
- EnvironmentContainer
  |- SkyLayer (z=15)
  |- MountainsLayer (z=12)
  |- TreelineLayer (z=10)
  |- CampBackgroundLayer (z=5)
  |- MainCampLayer (z=0)
  |- ForegroundLayer (z=-5)
```

### 2. Parallax Configuration
```
ParallaxBackground.cs settings:
1. Sky: 0.05 parallax effect
2. Mountains: 0.1 effect
3. Treeline: 0.2 effect
4. Camp Background: 0.4 effect
5. Main Camp: 0.7 effect
6. Foreground: 0.9 effect
```

### 3. Ground System
```
1. Create terrain layers:
   - Base ground texture
   - Detail overlays
   - Path markers
   - Grass/dirt transitions
2. Add ground shadows
3. Setup terrain colliders
```

## Phase 4: Lighting System (2-3 hours)

### 1. Time of Day Lighting
```
1. Add DynamicLighting.cs
2. Configure light settings:
   - Dawn: Warm orange (0.8 intensity)
   - Day: Bright white (1.0 intensity)
   - Dusk: Deep orange (0.8 intensity)
   - Night: Deep blue (0.4 intensity)
```

### 2. Campfire Lighting
```
1. Create point light for fire
2. Add flickering effect
3. Setup light falloff
4. Configure shadow interaction
```

### 3. Ambient Lighting
```
1. Setup ambient light for scene
2. Add rim lighting for tents
3. Configure shadow softness
4. Add light probes for props
```

## Phase 5: Weather and Effects (3-4 hours)

### 1. Weather System
```
1. Setup weather layers:
   - Rain particles (3 depths)
   - Wind effects
   - Dust particles
   - Fog system
2. Configure weather transitions
```

### 2. Environmental Effects
```
1. Campfire effects:
   - Particle smoke
   - Fire animation
   - Heat distortion
2. Ambient effects:
   - Floating dust
   - Grass movement
   - Tent fabric motion
```

### 3. Time-Based Effects
```
1. Morning effects:
   - Dawn mist
   - Light rays
2. Evening effects:
   - Sunset colors
   - Cricket sounds
3. Night effects:
   - Fireflies
   - Star particles
```

## Phase 6: Character Integration (2-3 hours)

### 1. Character Placement
```
1. Create character zones:
   - Tent areas
   - Campfire gathering
   - Resource collection
2. Setup pathing system
3. Add depth-based sorting
```

### 2. Shadow System
```
1. Dynamic character shadows:
   - Length varies with time
   - Opacity changes with weather
   - Interaction with terrain
2. Static prop shadows
```

### 3. Character Effects
```
1. Footprint effects
2. Interaction particles
3. Weather reactions
```

## Phase 7: UI and Interaction (2-3 hours)

### 1. World Space UI
```
1. Setup interaction markers
2. Add floating indicators
3. Configure depth scaling
```

### 2. Environmental UI
```
1. Time of day indicator
2. Weather status
3. Temperature system
4. Resource markers
```

## Phase 8: Optimization (2-3 hours)

### 1. Performance Checks
```
1. Batch similar sprites
2. Optimize particle systems
3. Setup object pooling
4. Configure LOD system
```

### 2. Quality Settings
```
1. Adjust draw distances
2. Configure effect densities
3. Setup quality tiers
4. Optimize shadow resolution
```

## Testing Checklist

### Visual Tests
- [ ] Day/night cycle smooth
- [ ] Weather effects convincing
- [ ] Character shadows correct
- [ ] Parallax movement natural
- [ ] Campfire effects realistic

### Performance Tests
- [ ] Stable frame rate
- [ ] Memory usage optimal
- [ ] Particle system performance
- [ ] Shadow performance
- [ ] Weather system impact

### Gameplay Tests
- [ ] Character movement natural
- [ ] Interaction points clear
- [ ] UI elements readable
- [ ] Weather affects gameplay
- [ ] Time system functional

## Common Issues and Solutions

### 1. Tent Shadow Issues
```
Problem: Tent shadows not aligning with time of day
Solution:
1. Check shadow anchor points
2. Verify light angle calculations
3. Adjust shadow softness
```

### 2. Weather Clipping
```
Problem: Weather effects showing through tents
Solution:
1. Adjust particle collision
2. Check sorting layers
3. Modify particle Z-depth
```

### 3. Performance Drops
```
Problem: Frame rate issues during weather
Solution:
1. Reduce particle count
2. Optimize shadow resolution
3. Adjust effect distances
```

## Next Steps
1. Begin with sky and background setup
2. Add basic parallax movement
3. Implement time of day system
4. Add character placement
5. Integrate weather effects
6. Polish and optimize

Would you like to start with Phase 1 and begin setting up the Campsite_3D scene? 