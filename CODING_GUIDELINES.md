# Unity C# Coding Guidelines

## 1. Naming Conventions

### General Rules
- Use PascalCase for class names, method names, and properties
- Use camelCase for variables and parameters
- Use UPPER_SNAKE_CASE for constants
- Prefix interfaces with 'I' (e.g., `IInventoryItem`)
- Use meaningful and descriptive names
- Avoid abbreviations unless widely known

### Unity-Specific
- Prefix private serialized fields with '_'
- MonoBehaviour classes should end with the word "Controller", "Manager", or "System" based on their role
- Event handler methods should start with "On" (e.g., `OnCollisionEnter2D`)

```csharp
public class InventoryController : MonoBehaviour
{
    [SerializeField] private int _maxItems = 10;
    private List<IInventoryItem> _items = new List<IInventoryItem>();
    
    public const int MAX_STACK_SIZE = 99;
    
    public void OnItemCollected(IInventoryItem item) { }
}
```

## 2. Code Organization

### File Structure
- One class per file (except for small related classes)
- File name must match class name
- Group related files in appropriate folders
- Keep script files in the Scripts folder, organized by feature

### Class Structure
1. Serialized Fields
2. Constants
3. Public Properties
4. Private Fields
5. Unity Event Methods (Awake, Start, Update)
6. Public Methods
7. Private Methods
8. Event Handlers

```csharp
public class WeatherSystem : MonoBehaviour
{
    [SerializeField] private float _rainIntensity;
    
    public const float MAX_INTENSITY = 1.0f;
    
    public float CurrentIntensity { get; private set; }
    
    private ParticleSystem _rainSystem;
    
    private void Awake() { }
    private void Start() { }
    
    public void SetWeather(WeatherType type) { }
    
    private void UpdateParticles() { }
}
```

## 3. Best Practices

### Unity-Specific
1. **Component References**
   - Cache component references in Awake
   - Use [SerializeField] instead of public for inspector variables
   - Prefer GetComponent<T>() in Awake over Inspector assignments

2. **Performance**
   - Avoid using Find, FindObjectOfType, or FindObjectsOfType in Update
   - Cache transform references
   - Use object pooling for frequently spawned objects
   - Minimize Update method usage

3. **Scene Management**
   - Use ScriptableObjects for shared data
   - Implement proper scene loading/unloading
   - Handle DontDestroyOnLoad objects carefully

### General C#
1. **SOLID Principles**
   - Single Responsibility Principle
   - Open/Closed Principle
   - Liskov Substitution Principle
   - Interface Segregation Principle
   - Dependency Inversion Principle

2. **Code Quality**
   - Keep methods short and focused
   - Avoid deep nesting
   - Use early returns
   - Implement proper exception handling
   - Write clear comments for complex logic

## 4. Documentation

### Comments
- Use XML documentation for public methods and properties
- Keep comments current and meaningful
- Explain "why" rather than "what"
- Document any non-obvious behavior

```csharp
/// <summary>
/// Processes the queue at Home Affairs office.
/// </summary>
/// <param name="waitTime">Time in minutes for processing</param>
/// <returns>True if processing successful, false if failed</returns>
public bool ProcessQueue(float waitTime) { }
```

### Regions
- Use regions sparingly
- Group related functionality
- Keep region names descriptive

## 5. Event Handling

### Unity Events
- Prefer UnityEvent for Inspector-assignable events
- Properly unsubscribe from events in OnDisable/OnDestroy
- Use appropriate event functions (OnEnable, OnDisable, etc.)

### Custom Events
- Implement proper event handling patterns
- Use EventArgs for event data
- Consider using ScriptableObject events for decoupled communication

## 6. Error Handling

### Debug Logs
- Use appropriate log levels (Debug.Log, Warning, Error)
- Remove or comment out debug logs in production
- Include relevant context in log messages

### Exception Handling
- Use try-catch blocks appropriately
- Don't catch general exceptions
- Log exceptions with stack traces in development

## 7. Version Control

### Git Practices
- Write clear commit messages
- Keep commits focused and atomic
- Use meaningful branch names
- Don't commit generated files or temporary files

### Unity-Specific
- Use appropriate .gitignore
- Be careful with scene merges
- Consider using Unity Teams if available

## 8. Testing

### Unit Testing
- Write tests for critical game logic
- Keep tests independent
- Use meaningful test names
- Follow Arrange-Act-Assert pattern

### Play Testing
- Test across different Unity versions
- Verify behavior on target platforms
- Check performance metrics
- Validate edge cases

## 9. Project Settings

### Editor
- Use consistent Unity version
- Configure proper company name and bundle identifier
- Set appropriate player settings
- Configure proper quality settings

### Build
- Set proper build settings
- Configure appropriate platform settings
- Optimize build size
- Set up proper versioning

## 10. Asset Guidelines

### Prefabs
- Use prefab variants when appropriate
- Keep prefab hierarchy clean
- Break down complex prefabs into smaller ones
- Use prefab overrides carefully

### ScriptableObjects
- Use for configuration data
- Create proper asset menus
- Implement proper validation
- Use for event systems when appropriate 