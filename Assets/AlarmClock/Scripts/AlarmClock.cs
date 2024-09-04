using UnityEngine;

namespace AlarmClock.Scripts
{
    public class AlarmClock : MonoBehaviour
    {
        [SerializeField] private int hours;
        [SerializeField] private int minutes;

        private ClockTimeProvider _clockTimeProvider;
        private long _targetUnixTime;
        
        public bool IsActive { get; private set; }
        
        private void Awake()
        {
            _clockTimeProvider = FindObjectOfType<ClockTimeProvider>();

            if (_clockTimeProvider.IsInitialized)
                Initialize();
            else
                _clockTimeProvider.OnInitialized += Initialize;
        }

        private void Initialize()
        {
            _clockTimeProvider.OnInitialized -= Initialize;
            _clockTimeProvider.ClockTime.OnTick += CheckAlarm;

            var alarm = new ClockTime();
            alarm.AddSeconds(hours * 60 * 60 + minutes * 60);
            SetAlarm(alarm);
        }
        
        public void SetAlarm(ClockTime alarmClock)
        {
            IsActive = true;
            var difference = alarmClock.TotalSeconds - _clockTimeProvider.ClockTime.TotalSeconds;
            if (difference < 0) 
                difference += ClockTime.SecondsInDay;
            
            _targetUnixTime = _clockTimeProvider.ClockTime.CurrentUnixSeconds + difference;
        }
        
        public void SetAlarm(long targetUnixTime)
        {
            IsActive = true;
            _targetUnixTime = targetUnixTime;
        }
        
        private void CheckAlarm()
        {
            if (!IsActive)
                return;

            if (_clockTimeProvider.ClockTime.CurrentUnixSeconds >= _targetUnixTime)
            {
                IsActive = false;
                Debug.Log("ALARM");
            }
        }
    }
}