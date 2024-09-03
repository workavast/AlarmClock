using UnityEngine;

namespace AlarmClock.Scripts
{
    public class ClockTimeProvider : MonoBehaviour
    {
        public readonly ClockTime ClockTime = new();

        private float _secondsCounter;

        private async void Awake() 
            => ClockTime.SetTime(await NtpTime.GetNetworkTime());

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
        }
    }
}