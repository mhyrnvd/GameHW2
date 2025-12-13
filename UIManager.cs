using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject RedAlarmPrefab;
    [SerializeField] private GameObject GothamCity;
    [SerializeField] private GameObject BatmanPrefab;
    [SerializeField] private GameObject BatSignalPrefab;

    [Header("Audio")]
    [SerializeField] private AudioSource alertAudio;

    private SpriteRenderer _batmanSprite;
    private SpriteRenderer _gothamSprite;
    private SpriteRenderer _batSignalSprite;

    private bool _alertActive;
    private float _blinkTimer;
    private float _blinkInterval = 0.5f;

    private float _rotationTimer;
    private float _rotationAmount = 5f;
    private float _rotationSpeed = 1f;

    private void Awake()
    {
        _batmanSprite = BatmanPrefab.GetComponent<SpriteRenderer>();
        _gothamSprite = GothamCity.GetComponent<SpriteRenderer>();
        _batSignalSprite = BatSignalPrefab.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<AlertStartedEvent>(OnAlertStarted);
        EventBus.Subscribe<AlertStoppedEvent>(OnAlertStopped);
        EventBus.Subscribe<BatSignalToggleEvent>(OnBatSignalToggle);
        EventBus.Subscribe<OpacityChangedEvent>(OnOpacityChanged);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<AlertStartedEvent>(OnAlertStarted);
        EventBus.Unsubscribe<AlertStoppedEvent>(OnAlertStopped);
        EventBus.Unsubscribe<BatSignalToggleEvent>(OnBatSignalToggle);
        EventBus.Unsubscribe<OpacityChangedEvent>(OnOpacityChanged);
    }

    private void Update()
    {
        HandleAlarmBlink();
        HandleBatSignalRotation();
    }

    // ================= ALERT =================

    private void OnAlertStarted(AlertStartedEvent e)
    {
        _alertActive = true;
        RedAlarmPrefab.SetActive(true);

        SetOpacity(1f);

        if (!alertAudio.isPlaying)
            alertAudio.Play();
    }

    private void OnAlertStopped(AlertStoppedEvent e)
    {
        _alertActive = false;
        RedAlarmPrefab.SetActive(false);

        if (alertAudio.isPlaying)
            alertAudio.Stop();
    }

    private void OnOpacityChanged(OpacityChangedEvent e)
    {
        SetOpacity(e.Alpha);
    }

    private void HandleAlarmBlink()
    {
        if (!_alertActive) return;

        _blinkTimer += Time.deltaTime;

        if (_blinkTimer >= _blinkInterval)
        {
            RedAlarmPrefab.SetActive(!RedAlarmPrefab.activeSelf);
            _blinkTimer = 0f;
        }
    }

    // ================= BAT SIGNAL =================

    private void OnBatSignalToggle(BatSignalToggleEvent e)
    {
        BatSignalPrefab.SetActive(!BatSignalPrefab.activeSelf);
    }

    private void HandleBatSignalRotation()
    {
        if (!BatSignalPrefab.activeSelf) return;

        _rotationTimer += Time.deltaTime * _rotationSpeed;
        float angle = Mathf.Sin(_rotationTimer) * _rotationAmount;

        BatSignalPrefab.transform.localRotation =
            Quaternion.Euler(0f, 0f, angle);
    }

    // ================= OPACITY =================

    private void SetOpacity(float alpha)
    {
        if (_batmanSprite != null)
            SetSpriteAlpha(_batmanSprite, alpha);

        if (_gothamSprite != null)
            SetSpriteAlpha(_gothamSprite, alpha);

        if (_batSignalSprite != null)
            SetSpriteAlpha(_batSignalSprite, alpha);
    }

    private void SetSpriteAlpha(SpriteRenderer sr, float alpha)
    {
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}
