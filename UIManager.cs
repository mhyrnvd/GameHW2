using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject RedAlarmPrefab;

    [SerializeField] 
    private AudioSource alertAudio;

    private AudioSource _alarmAudio;

    private bool _alertActive = false;
    private float _timer = 0f;
    private float _interval = 0.5f;

    void Start()
    {
        _alarmAudio = RedAlarmPrefab.GetComponent<AudioSource>();
        if (_alarmAudio == null)
            Debug.LogError("The Alarm Audio is NULL");
    }

    void Update()
    {
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
}
