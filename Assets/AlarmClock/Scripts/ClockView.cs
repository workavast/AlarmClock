using UnityEngine;

namespace AlarmClock.Scripts
{
    public class ClockView : MonoBehaviour
    {
        [SerializeField] private GameObject hourArrow;
        [SerializeField] private GameObject minuteArrow;
        [SerializeField] private GameObject secondArrow;
        
        private ClockTimeProvider _clockTimeProvider;

        private void Start()
        {
            _clockTimeProvider = FindObjectOfType<ClockTimeProvider>();
            _clockTimeProvider.ClockTime.OnTick += UpdateView;
        }

        private void UpdateView()
        {
            RotateArrow(hourArrow, _clockTimeProvider.ClockTime.FullDayPercentage / 2);
            RotateArrow(minuteArrow, _clockTimeProvider.ClockTime.HourPercentage);
            RotateArrow(secondArrow, _clockTimeProvider.ClockTime.MinutePercentage);
        }

        private static void RotateArrow(GameObject arrow, float clockPercentage)
        {
            var currentAngle = 360 * clockPercentage;
            arrow.transform.rotation = Quaternion.Euler(0,0, 360 - currentAngle);
        }
    }
}