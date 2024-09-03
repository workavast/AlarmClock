using UnityEngine;

namespace AlarmClock.Scripts
{
    public class ClockTimeProvider : MonoBehaviour
    {
        public ClockTime ClockTime { get; private set; } = new();

        private float _secCounter;

        private async void Awake() 
            => ClockTime.UpdateTime(await NtpTime.GetNetworkTime());

        private void Update()
        {
            _secCounter += Time.unscaledDeltaTime;

            if (_secCounter > 1)
            {
                var secs = (int)_secCounter;
                _secCounter -= secs;
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