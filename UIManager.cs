using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject RedAlarmPrefab;

    [SerializeField]
    private GameObject BatmanPrefab;

    [SerializeField]
    private GameObject GothamCity;

    [SerializeField]
    private GameObject BatSignalPrefab;

    private SpriteRenderer _batmanSprite;
    private SpriteRenderer _gothamCitySprite;
    private SpriteRenderer _batSignalSprite;

    [SerializeField] 
    private AudioSource alertAudio;

    private AudioSource _alarmAudio;

    private bool _alertActive = false;
    private float _timer = 0f;
    private float _interval = 0.5f;
    private float _rotationAmount = 5f;
    private float _rotationSpeed = 1f;   

    void Start()
    {
        _batmanSprite = BatmanPrefab.GetComponent<SpriteRenderer>();
        if (_batmanSprite == null)
            Debug.LogError("SpriteRenderer not found on BatmanPrefab");

        _gothamCitySprite = GothamCity.GetComponent<SpriteRenderer>();
        if (_gothamCitySprite == null)
            Debug.LogError("SpriteRenderer not found on GothamCity");

        _batSignalSprite = BatSignalPrefab.GetComponent<SpriteRenderer>();
        if (_batSignalSprite == null)
            Debug.LogError("SpriteRenderer not found on BatSignal");

        _alarmAudio = RedAlarmPrefab.GetComponent<AudioSource>();
        if (_alarmAudio == null)
            Debug.LogError("The Alarm Audio is NULL");
    }

    void Update()
    {
        if (BatSignalPrefab.activeSelf)
        {
            _timer += Time.deltaTime * _rotationSpeed;

            float angle = Mathf.Sin(_timer) * _rotationAmount;

            BatSignalPrefab.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (_alertActive)
        {
            _timer += Time.deltaTime;

            if (_timer >= _interval)
            {
                RedAlarmPrefab.SetActive(!RedAlarmPrefab.activeSelf);
                _timer = 0f;
            }
        }
    }

    public void StartAlert()
    {
        _alertActive = true;
        _timer = 0f;
        RedAlarmPrefab.SetActive(true);

        if (!alertAudio.isPlaying)
            alertAudio.Play();
    }

    public void StopAlert()
    {
        _alertActive = false;
        RedAlarmPrefab.SetActive(false);

        if (alertAudio.isPlaying)
            alertAudio.Stop();
    }

    public void SetObjectOpacity(float alpha)
    {
        if (_gothamCitySprite == null) return;
        if (_batmanSprite == null) return;

        Color gothamColor = _gothamCitySprite.color;
        gothamColor.a = alpha;
        _gothamCitySprite.color = gothamColor;

        Color batmanColor = _batmanSprite.color;
        batmanColor.a = alpha;
        _batmanSprite.color = batmanColor;

        Color batmanSignalColor = _batSignalSprite.color;
        batmanSignalColor.a = alpha;
        _batSignalSprite.color = batmanSignalColor;
    }

    public void ResetObejctOpacity()
    {
        SetObjectOpacity(1f);
    }

    public void ToggleBatSignal()
    {
        if (BatSignalPrefab.activeSelf)
        {
            BatSignalPrefab.SetActive(false);
        }
        else
        {
            BatSignalPrefab.SetActive(true);
        }
    }
}
