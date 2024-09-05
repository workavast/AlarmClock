using System;
using UnityEngine;

namespace AlarmClock.Scripts
{
    public class AlarmClockProvider : MonoBehaviour
    {
        private ClockTimeProvider _clockTimeProvider;
        public readonly ClockTime TargetTime = new();
        public long TargetUnixTime => TargetTime.UnixSeconds;
        
        public bool IsActive { get; private set; }
        public event Action OnActivationStateChange;
        public event Action OnAlarm;
        
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
        }
        
        public void SetAlarm(ClockTime alarmClock)
        {
            var prevActiveState = IsActive;
            IsActive = true;
            var difference = alarmClock.TotalSeconds - _clockTimeProvider.ClockTime.TotalSeconds;
            if (difference < 0) 
                difference += ClockTime.SecondsInDay;

            var targetUnixTime = _clockTimeProvider.ClockTime.UnixSeconds + difference;
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

            if (_clockTimeProvider.ClockTime.UnixSeconds >= TargetUnixTime)
            {
                SetAlarm(TargetTime);
                OnAlarm?.Invoke();
            }
        }
    }
}