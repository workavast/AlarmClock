using System;
using UnityEngine;

namespace AlarmClock.Scripts
{
    public class ClockTimeProvider : MonoBehaviour
    {
        public readonly ClockTime ClockTime = new();

        private float _secondsCounter;

        public bool IsInitialized { get; private set; }
        public event Action OnInitialized;

        private void Awake()
            => Initialize();

        private async void Initialize()
        {
            ClockTime.SetTime(await NtpTime.GetNetworkTime());
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
                ClockTime.AddSeconds(secs);
            }
        }

        [ContextMenu("TakeTime")]
        private async void TakeTime()
        {
            var res = await NtpTime.GetNetworkTime();
            Debug.Log(res.ToString());

            if (!IsInitialized)
            {
                IsInitialized = true;
                OnInitialized?.Invoke();
            }
        }
    }
}