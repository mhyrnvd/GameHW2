using UnityEngine;

public interface IBatmanState
{
    void Enter();
    void Update();
    void Exit();
}

public class Batman : MonoBehaviour
{
    public float normalSpeed = 5f;

    [HideInInspector] public float currentSpeed;
    [SerializeField] private GameObject _batMobile;
    public float batMobileSpeedMultiplier = 2f;

    private bool _isInBatMobile = false;

    private IBatmanState _currentState;

    private void Start()
    {
        ChangeState(new NormalState(this));
    }

    private void Update()
    {
        HandleMovement();
        HandleGlobalInput();
        _currentState.Update();
    }

    public void ChangeState(IBatmanState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float speed = currentSpeed;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speed *= 1.5f;
        }

        Vector3 move = new(h, v, 0);
        transform.Translate(speed * Time.deltaTime * move, Space.World);
    }

    private void HandleGlobalInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleBatMobile();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            EventBus.Publish(new BatSignalToggleEvent());
        }
    }

    private void ToggleBatMobile()
    {
        _isInBatMobile = !_isInBatMobile;

        _batMobile.SetActive(_isInBatMobile);

        if (_isInBatMobile)
        {
            currentSpeed *= batMobileSpeedMultiplier;
        }
        else
        {
            currentSpeed = normalSpeed;
        }
    }
}

public class NormalState : IBatmanState
{
    private Batman _batman;

    public NormalState(Batman batman)
    {
        _batman = batman;
    }

    public void Enter()
    {
        _batman.currentSpeed = _batman.normalSpeed;

        EventBus.Publish(new AlertStoppedEvent());
        EventBus.Publish(new OpacityChangedEvent(1f));
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            _batman.ChangeState(new StealthState(_batman));

        if (Input.GetKeyDown(KeyCode.Space))
            _batman.ChangeState(new AlertState(_batman));
    }

    public void Exit() { }
}

public class StealthState : IBatmanState
{
    private Batman _batman;

    public StealthState(Batman batman)
    {
        _batman = batman;
    }

    public void Enter()
    {
        _batman.currentSpeed = _batman.normalSpeed * 0.5f;

        EventBus.Publish(new AlertStoppedEvent());
        EventBus.Publish(new OpacityChangedEvent(0.7f));
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            _batman.ChangeState(new NormalState(_batman));

        if (Input.GetKeyDown(KeyCode.Space))
            _batman.ChangeState(new AlertState(_batman));
    }

    public void Exit() { }
}

public class AlertState : IBatmanState
{
    private Batman _batman;

    public AlertState(Batman batman)
    {
        _batman = batman;
    }

    public void Enter()
    {
        EventBus.Publish(new AlertStartedEvent());
        EventBus.Publish(new OpacityChangedEvent(1f));
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            _batman.ChangeState(new NormalState(_batman));

        if (Input.GetKeyDown(KeyCode.C))
            _batman.ChangeState(new StealthState(_batman));
    }

    public void Exit()
    {
        EventBus.Publish(new AlertStoppedEvent());
    }
}
