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

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : normalSpeed;

        Vector3 move = new Vector3(horizontal, vertical, 0);

        transform.Translate(move * currentSpeed * Time.deltaTime, Space.World);
    }

    void HandleStateInput()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            _currentState = BatmanState.Normal;
            _uiManager.StopAlert();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _currentState = BatmanState.Stealth;
            _uiManager.StopAlert();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _currentState = BatmanState.Alert;
            _uiManager.StartAlert();
        }
    }
}
