using System;
using UnityEngine;

namespace AlarmClock.Scripts
{
    public class ClockTimeProvider : IClockTimeProvider
    {
        private float _secondsCounter;
        private long _targetTimeToUpdateNtpTime;

        public ClockTime ClockTime { get; } = new();
        public bool IsInitialized { get; private set; }
        public event Action OnInitialized;

        public ClockTimeProvider()
        {
            UpdateTime();
        }
        
        private async void UpdateTime()
        {
            ClockTime.SetTime(await NtpTime.GetNetworkClockTime());
            _targetTimeToUpdateNtpTime = ClockTime.UnixSeconds + ClockTime.SecondsInHour;
            if (!IsInitialized)
            {
                IsInitialized = true;
                OnInitialized?.Invoke();
            }
        }
        
        public void Update()
        {
            _secondsCounter += Time.unscaledDeltaTime;

            if (_secondsCounter > 1)
            {
                var seconds = (int)_secondsCounter;
                _secondsCounter -= seconds;
                ClockTime.ChangeSeconds(seconds);

                if (IsInitialized && _targetTimeToUpdateNtpTime <= ClockTime.UnixSeconds) 
                    UpdateTime();
            }
        }
    }
}