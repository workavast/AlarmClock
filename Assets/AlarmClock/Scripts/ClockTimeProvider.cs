using System;
using UnityEngine;

namespace AlarmClock.Scripts
{
    public class ClockTimeProvider : MonoBehaviour
    {
        public readonly ClockTime ClockTime = new();

        private float _secondsCounter;
        private long _targetTimeToUpdateNtpTime;

        public bool IsInitialized { get; private set; }
        public event Action OnInitialized;

        private void Awake()
            => UpdateTime();

        private async void UpdateTime()
        {
            ClockTime.SetTime(await NtpTime.GetNetworkClockTime());
            _targetTimeToUpdateNtpTime = ClockTime.CurrentUnixSeconds + ClockTime.SecondsInHour;
            if (!IsInitialized)
            {
                IsInitialized = true;
                OnInitialized?.Invoke();
            }
        }
        
        private void Update()
        {
            _secondsCounter += Time.unscaledDeltaTime;

            if (_secondsCounter > 1)
            {
                var secs = (int)_secondsCounter;
                _secondsCounter -= secs;
                ClockTime.ChangeSeconds(secs);

                if (IsInitialized && _targetTimeToUpdateNtpTime <= ClockTime.CurrentUnixSeconds) 
                    UpdateTime();
            }
        }
    }
}