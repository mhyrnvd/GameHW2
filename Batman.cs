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
    public float runSpeed = 10f;

    [HideInInspector] public UIManager uiManager;
    [HideInInspector] public float currentSpeed;

    private IBatmanState _currentState;

    private void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        ChangeState(new NormalState(this));
    }

    private void Update()
    {
        HandleMovement();
        _currentState.Update();

        if (Input.GetKeyDown(KeyCode.B))
        {
            uiManager.ToggleBatSignal();
        }
    }

    public void ChangeState(IBatmanState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, v, 0);
        transform.Translate(move * currentSpeed * Time.deltaTime, Space.World);
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
        _batman.uiManager.StopAlert();
        _batman.uiManager.ResetObejctOpacity();
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
        _batman.uiManager.StopAlert();
        _batman.uiManager.SetObjectOpacity(0.7f);
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
        _batman.currentSpeed = _batman.runSpeed;
        _batman.uiManager.StartAlert();
        _batman.uiManager.ResetObejctOpacity();
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
        _batman.uiManager.StopAlert();
    }
}
