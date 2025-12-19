using UnityEngine;

/// <summary>
/// Interface for all Batman states (State Pattern).
/// Defines lifecycle methods for each state.
/// </summary>
public interface IBatmanState
{
    /// <summary>Called when the state is entered.</summary>
    void Enter();

    /// <summary>Called every frame while the state is active.</summary>
    void Update();

    /// <summary>Called when exiting the state.</summary>
    void Exit();
}

/// <summary>
/// Main Batman controller handling movement, input, and state transitions.
/// </summary>
public class Batman : MonoBehaviour
{
    /// <summary>Base movement speed.</summary>
    public float normalSpeed = 5f;

    /// <summary>Current movement speed affected by states and vehicles.</summary>
    [HideInInspector] public float currentSpeed;

    /// <summary>BatMobile game object.</summary>
    [SerializeField] private GameObject _batMobile;

    /// <summary>Speed multiplier when inside BatMobile.</summary>
    public float batMobileSpeedMultiplier = 2f;

    /// <summary>Indicates whether Batman is inside BatMobile.</summary>
    private bool _isInBatMobile = false;

    /// <summary>Current active state.</summary>
    private IBatmanState _currentState;

    /// <summary>Initializes Batman with Normal state.</summary>
    private void Start()
    {
        ChangeState(new NormalState(this));
    }

    /// <summary>Handles movement, global input, and state updates.</summary>
    private void Update()
    {
        HandleMovement();
        HandleGlobalInput();
        _currentState.Update();
    }

    /// <summary>Changes the current state and triggers lifecycle methods.</summary>
    public void ChangeState(IBatmanState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    /// <summary>Handles player movement and Shift speed boost.</summary>
    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float speed = currentSpeed;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            speed *= 1.5f;

        Vector3 move = new(h, v, 0);
        transform.Translate(speed * Time.deltaTime * move, Space.World);
    }

    /// <summary>Handles global inputs such as BatMobile and Bat-Signal.</summary>
    private void HandleGlobalInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
            ToggleBatMobile();

        if (Input.GetKeyDown(KeyCode.B))
            EventBus.Publish(new BatSignalToggleEvent());
    }

    /// <summary>Toggles BatMobile on/off and updates movement speed.</summary>
    private void ToggleBatMobile()
    {
        _isInBatMobile = !_isInBatMobile;
        _batMobile.SetActive(_isInBatMobile);

        currentSpeed = _isInBatMobile
            ? currentSpeed * batMobileSpeedMultiplier
            : normalSpeed;
    }
}

/// <summary>
/// Normal movement state with default speed.
/// </summary>
public class NormalState : IBatmanState
{
    private Batman _batman;

    /// <summary>Creates a Normal state instance.</summary>
    public NormalState(Batman batman)
    {
        _batman = batman;
    }

    /// <summary>Sets normal speed and resets visuals.</summary>
    public void Enter()
    {
        _batman.currentSpeed = _batman.normalSpeed;
        EventBus.Publish(new AlertStoppedEvent());
        EventBus.Publish(new OpacityChangedEvent(1f));
    }

    /// <summary>Listens for state transition inputs.</summary>
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            _batman.ChangeState(new StealthState(_batman));

        if (Input.GetKeyDown(KeyCode.Space))
            _batman.ChangeState(new AlertState(_batman));
    }

    /// <summary>No cleanup required when exiting Normal state.</summary>
    public void Exit() { }
}

/// <summary>
/// Stealth state with reduced speed and opacity.
/// </summary>
public class StealthState : IBatmanState
{
    private Batman _batman;

    /// <summary>Creates a Stealth state instance.</summary>
    public StealthState(Batman batman)
    {
        _batman = batman;
    }

    /// <summary>Applies stealth movement speed and transparency.</summary>
    public void Enter()
    {
        _batman.currentSpeed = _batman.normalSpeed * 0.5f;
        EventBus.Publish(new AlertStoppedEvent());
        EventBus.Publish(new OpacityChangedEvent(0.7f));
    }

    /// <summary>Listens for transitions to other states.</summary>
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            _batman.ChangeState(new NormalState(_batman));

        if (Input.GetKeyDown(KeyCode.Space))
            _batman.ChangeState(new AlertState(_batman));
    }

    /// <summary>No cleanup required when exiting Stealth state.</summary>
    public void Exit() { }
}

/// <summary>
/// Alert state triggering alarms and aggressive behavior.
/// </summary>
public class AlertState : IBatmanState
{
    private Batman _batman;

    /// <summary>Creates an Alert state instance.</summary>
    public AlertState(Batman batman)
    {
        _batman = batman;
    }

    /// <summary>Starts alert effects and notifications.</summary>
    public void Enter()
    {
        EventBus.Publish(new AlertStartedEvent());
        EventBus.Publish(new OpacityChangedEvent(1f));
    }

    /// <summary>Handles state transitions while alert is active.</summary>
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            _batman.ChangeState(new NormalState(_batman));

        if (Input.GetKeyDown(KeyCode.C))
            _batman.ChangeState(new StealthState(_batman));
    }

    /// <summary>Stops alert effects when leaving state.</summary>
    public void Exit()
    {
        EventBus.Publish(new AlertStoppedEvent());
    }
}
