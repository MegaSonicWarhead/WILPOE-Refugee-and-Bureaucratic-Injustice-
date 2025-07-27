# Asset Preparation Guide for 2D-to-3D Implementation

## 1. Asset Organization Structure

```
Assets/
├── Backgrounds/
│   ├── Far/           (Depth 1000)
│   ├── Mid/           (Depth 500)
│   └── Close/         (Depth 0)
├── Environment/
│   ├── Buildings/
│   │   ├── Shadows/
│   │   └── Variants/
│   ├── Props/
│   │   ├── Shadows/
│   │   └── Variants/
│   └── Ground/
│       ├── Base/
│       └── Overlays/
├── Characters/
│   ├── Shadows/
│   ├── Main/
│   └── NPCs/
├── Weather/
│   ├── Rain/
│   ├── Fog/
│   └── Particles/
├── Lighting/
│   ├── Global/
│   ├── Local/
│   └── Shadows/
└── UI/
    ├── HUD/
    └── Menus/
```

## 2. Asset Preparation Guidelines

### Background Assets
1. **Layer Separation**
   - Split existing backgrounds into 3-5 depth layers
   - Save each layer as separate PNG with transparency
   - Maintain high resolution for scaling
   - Name format: `BG_[Scene]_[Layer]_[Depth].png`

2. **Parallax Ready Format**
   - Extended edges for parallax movement
   - Seamless horizontal tiling
   - Buffer space for perspective shift
   - Recommended size: 1920x1080 minimum

### Building and Props
1. **Shadow Templates**
   - Create separate shadow sprites
   - Use soft edges for better blending
   - Multiple sizes for depth variation
   - Name format: `Shadow_[Object]_[Size].png`

2. **Depth Variants**
   - Multiple scales of each asset
   - Detail variations for different depths
   - Consistent light source direction
   - Name format: `[Object]_[Depth]_[Variant].png`

### Character Assets
1. **Base Requirements**
   - Transparent backgrounds
   - Consistent scale across characters
   - Clear silhouettes for shadows
   - Name format: `Char_[Type]_[Action].png`

2. **Shadow Assets**
   - Separate shadow sprites
   - Multiple angles for perspective
   - Varying opacity levels
   - Name format: `Char_[Type]_Shadow_[Angle].png`

### Weather Effects
1. **Rain Layers**
   - Front layer (large drops)
   - Mid layer (medium drops)
   - Back layer (small drops)
   - Name format: `Rain_[Layer]_[Intensity].png`

2. **Fog Elements**
   - Base fog texture
   - Overlay variations
   - Transparency maps
   - Name format: `Fog_[Type]_[Density].png`

### Lighting Assets
1. **Light Sources**
   - Global light sprites
   - Point light sprites
   - Light ray textures
   - Name format: `Light_[Type]_[Intensity].png`

2. **Shadow Assets**
   - Shadow gradients
   - Ambient occlusion maps
   - Shadow masks
   - Name format: `Shadow_[Type]_[Opacity].png`

## 3. Asset Processing Steps

### 1. Existing Asset Audit
- [ ] Review current assets in project
- [ ] Identify assets needing modification
- [ ] List missing required assets
- [ ] Check asset resolutions and formats

### 2. Background Processing
1. **For Each Background:**
   - [ ] Separate into layers
   - [ ] Extend edges for parallax
   - [ ] Create depth variations
   - [ ] Add transparency between layers

### 3. Building and Prop Processing
1. **For Each Building/Prop:**
   - [ ] Create shadow sprite
   - [ ] Make depth variants
   - [ ] Add lighting points
   - [ ] Generate LOD versions

### 4. Character Processing
1. **For Each Character:**
   - [ ] Extract shadow sprite
   - [ ] Create depth variations
   - [ ] Add lighting anchor points
   - [ ] Setup animation sheets

### 5. Effect Processing
1. **For Weather Effects:**
   - [ ] Create particle sprites
   - [ ] Generate fog textures
   - [ ] Make transition effects
   - [ ] Setup animation sheets

## 4. Unity Import Settings

### Background Assets
```
Import Settings:
- Texture Type: Sprite (2D and UI)
- Sprite Mode: Single
- Pixels Per Unit: 100
- Filter Mode: Bilinear
- Compression: Normal Quality
```

### Character Sprites
```
Import Settings:
- Texture Type: Sprite (2D and UI)
- Sprite Mode: Multiple
- Pixels Per Unit: 100
- Filter Mode: Point (no filter)
- Compression: None
```

### Effect Sprites
```
Import Settings:
- Texture Type: Sprite (2D and UI)
- Sprite Mode: Single
- Pixels Per Unit: 100
- Filter Mode: Bilinear
- Alpha: Transparency
```

## 5. Performance Optimization

### 1. Texture Atlasing
- Group similar assets into atlases
- Maintain power-of-two dimensions
- Organize by depth layer
- Keep related animations together

### 2. Asset Compression
- Use appropriate compression per asset type
- Balance quality vs file size
- Consider platform-specific settings
- Implement mip-mapping where needed

### 3. Memory Management
- Setup asset bundles
- Implement LOD system
- Use sprite atlases
- Configure asset loading zones

## 6. Implementation Checklist

### Initial Setup
- [ ] Create folder structure
- [ ] Setup naming conventions
- [ ] Configure import settings
- [ ] Create asset templates

### Asset Processing
- [ ] Process backgrounds
- [ ] Process buildings/props
- [ ] Process characters
- [ ] Process effects
- [ ] Create shadow assets
- [ ] Setup lighting assets

### Quality Control
- [ ] Check asset consistency
- [ ] Verify transparency
- [ ] Test parallax compatibility
- [ ] Validate shadow assets
- [ ] Review performance impact

### Integration
- [ ] Import to Unity project
- [ ] Setup sprite atlases
- [ ] Configure asset bundles
- [ ] Test in sample scene
- [ ] Optimize as needed

## 7. Specific Asset Requirements

### Home Affairs Office Scene
1. **Required Layers:**
   - Building exterior (front)
   - Building interior (back)
   - Queue area
   - Office furniture
   - Character positions

### Campsite Scene
1. **Required Layers:**
   - Ground terrain
   - Tents and structures
   - Background environment
   - Atmospheric effects
   - Character positions

### Shop Scene
1. **Required Layers:**
   - Shop interior
   - Shop exterior
   - Product displays
   - Counter area
   - Character positions

## 8. Tools and Resources

### Recommended Software
- Adobe Photoshop/GIMP for layer separation
- Aseprite for pixel art modification
- TexturePacker for sprite atlasing
- Unity Asset Processor for batch operations

### Unity Packages
- 2D Sprite
- 2D Tilemap Editor
- 2D Pixel Perfect
- Universal Render Pipeline
- 2D Lights and Shadows 