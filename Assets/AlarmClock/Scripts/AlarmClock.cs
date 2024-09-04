using System;
using UnityEngine;

namespace AlarmClock.Scripts
{
    public class AlarmClock : MonoBehaviour
    {
        [SerializeField] private int hours;
        [SerializeField] private int minutes;

        private ClockTimeProvider _clockTimeProvider;
        public readonly ClockTime TargetTime = new();
        public long TargetUnixTime => TargetTime.CurrentUnixSeconds;
        
        public bool IsActive { get; private set; }
        public event Action OnActivationStateChange;
        
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

            // var alarm = new ClockTime();
            // alarm.AddSeconds(hours * 60 * 60 + minutes * 60);
            // SetAlarm(alarm);
        }
        
        public void SetAlarm(ClockTime alarmClock)
        {
            var prevActiveState = IsActive;
            IsActive = true;
            var difference = alarmClock.TotalSeconds - _clockTimeProvider.ClockTime.TotalSeconds;
            if (difference < 0) 
                difference += ClockTime.SecondsInDay;

            var targetUnixTime = _clockTimeProvider.ClockTime.CurrentUnixSeconds + difference;
            TargetTime.SetTime(targetUnixTime, alarmClock);

            if (prevActiveState != IsActive) 
                OnActivationStateChange?.Invoke();
        }

        public void CancelAlarm()
        {
            var prevActiveState = IsActive;
            IsActive = false;
            
            TargetTime.SetTime(new ClockTime());
            
            if (prevActiveState != IsActive) 
                OnActivationStateChange?.Invoke();
        }
        
        private void CheckAlarm()
        {
            if (!IsActive)
                return;

            if (_clockTimeProvider.ClockTime.CurrentUnixSeconds >= TargetUnixTime)
            {
                IsActive = false;
                Debug.Log("ALARM");
                OnActivationStateChange?.Invoke();
            }
        }
    }
}