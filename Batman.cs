using UnityEngine;

enum BatmanState { Normal, Stealth, Alert }

public class Batman : MonoBehaviour
{
    private UIManager _uiManager;
    private BatmanState _currentState = BatmanState.Normal;

    public float normalSpeed = 5f;
    public float runSpeed = 10f;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
            Debug.LogError("The UI Manager is NULL");
    }

    void Update()
    {
        HandleMovement();
        HandleStateInput();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float currentSpeed = normalSpeed;

        if (_currentState == BatmanState.Stealth)
            currentSpeed = normalSpeed * 0.5f;

        if (Input.GetKey(KeyCode.LeftShift) && _currentState != BatmanState.Stealth)
            currentSpeed = runSpeed;

        Vector3 move = new Vector3(horizontal, vertical, 0);
        transform.Translate(move * currentSpeed * Time.deltaTime, Space.World);
    }

    void HandleStateInput()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            _currentState = BatmanState.Normal;
            _uiManager.StopAlert();
            _uiManager.ResetObejctOpacity();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _currentState = BatmanState.Stealth;
            _uiManager.StopAlert();
            _uiManager.SetObjectOpacity(0.7f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _currentState = BatmanState.Alert;
            _uiManager.StartAlert();
            _uiManager.ResetObejctOpacity();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            _uiManager.ToggleBatSignal();
        }
    }

}
