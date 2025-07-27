# Refugee and Bureaucratic Injustice - Game Design Document

## Game Overview
A serious game that simulates the challenges faced by refugees navigating bureaucratic systems, highlighting social injustice and systemic barriers.

## Core Game Mechanics

### 1. Resource Management
- **Money**: Limited financial resources
- **Health**: Physical and mental well-being
- **Time**: Day/night cycle and appointment scheduling
- **Documents**: Various papers and permits needed for progression

### 2. Location System
- **Home Affairs Office**: Central bureaucratic hub
- **Bank**: Financial transactions and services
- **Campsite**: Basic living and survival
- **Embassy**: Additional bureaucratic processes
- **Police Station**: Legal documentation and security
- **Public Facilities**: Basic needs and services
- **Shops**: Essential supplies and food

### 3. Weather System
- Dynamic weather affecting gameplay
- Impacts on character's health and mobility
- Visual feedback through weather sprites

### 4. Inventory System
- Limited inventory space
- Essential items management
- Document organization

## Game Systems

### 1. Event System
- **Random Events**: 
  - Robberies
  - Attacks
  - Weather-related incidents
- **Scripted Events**:
  - NPC interactions
  - Queue management
  - Document processing

### 2. Time Management
- Day/night cycle
- Appointment scheduling
- Queue waiting times
- Activity duration tracking

### 3. UI Systems
- Inventory interface
- Health and status indicators
- Map navigation
- Quest/objective tracking
- Time display

## Scene Structure
1. **Main Menu**
   - New game
   - Load game
   - Settings
   - Credits

2. **Game Scenes**
   - Home Affairs Office
   - Bank
   - Campsite
   - Embassy
   - Police Station
   - Public Bathrooms
   - Shops
   - Map (Navigation hub)

## Art Style
- 2D pixel art
- Weather effects
- Dynamic lighting
- Parallax backgrounds

## 2D-to-3D Visual Techniques

### 1. Depth Layering
- Multiple background layers with different scroll speeds
- Foreground elements with higher detail
- Mid-ground elements for main gameplay
- Background elements for atmosphere
- Use of alpha channels for depth perception

### 2. Lighting and Shadows
- Dynamic lighting system for time of day
- Cast shadows from characters and objects
- Light sources affect multiple layers
- Ambient occlusion simulation
- Dynamic weather effects affecting lighting

### 3. Perspective Techniques
- Isometric-style elements where appropriate
- Height offset for distant objects
- Scale variation based on distance
- Overlapping elements for depth
- Perspective camera movements

### 4. Visual Effects
- Particle systems with depth sorting
- Rain and weather effects on multiple layers
- Fog and atmosphere effects
- Depth-based blur effects
- Dynamic reflections

### 5. Animation Techniques
- Parallax scrolling for background layers
- Scale-based movement speeds
- Perspective-based animation timing
- Shadow movement coordination
- Depth-aware particle systems

### 6. Technical Implementation
- Layer-based sorting system
- Custom shader effects for depth
- Camera system with perspective simulation
- Dynamic object scaling
- Depth-based color and contrast adjustment

## Audio Design
- Ambient sounds
- UI feedback
- Weather effects
- Background music

## Player Progression

### 1. Document Collection
- Birth certificates
- Identity documents
- Permits
- Applications

### 2. Resource Building
- Money management
- Essential supplies
- Safe housing
- Healthcare access

### 3. Story Progression
- Multiple paths based on choices
- Various outcomes based on success/failure
- Time-sensitive decisions

## Technical Requirements

### 1. Core Systems
- Unity 2D
- Save/Load functionality
- Scene management
- Event system
- Weather system
- Inventory system

### 2. Performance Considerations
- Optimized asset loading
- Efficient UI updates
- Scene transition management

## Future Enhancements
1. Multiple language support
2. Accessibility features
3. Additional storylines
4. More random events
5. Extended weather system

## Development Priorities
1. Core gameplay mechanics
2. Essential UI systems
3. Basic scene implementation
4. Event system
5. Weather and environmental effects
6. Audio implementation
7. Testing and balancing
8. Polish and optimization 