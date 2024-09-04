using UnityEngine;

namespace AlarmClock.Scripts.Ui
{
    public class AlarmClockView : MonoBehaviour
    {
        [SerializeField] private GameObject hourArrow;
        [SerializeField] private GameObject minuteArrow;
        [SerializeField] private GameObject secondArrow;
        
        private AlarmClock _clockTimeProvider;

        private void Awake() 
            => _clockTimeProvider = FindObjectOfType<AlarmClock>();

        private void OnEnable()
        {
            _clockTimeProvider.TargetTime.OnTick += UpdateView;
            UpdateView();
        }

        private void OnDisable() 
            => _clockTimeProvider.TargetTime.OnTick -= UpdateView;

        private void OnDestroy() 
            => _clockTimeProvider.TargetTime.OnTick -= UpdateView;

        private void UpdateView()
        {
            RotateArrow(hourArrow, _clockTimeProvider.TargetTime.FullDayPercentage * 2);
            RotateArrow(minuteArrow, _clockTimeProvider.TargetTime.HourPercentage);
            RotateArrow(secondArrow, _clockTimeProvider.TargetTime.MinutePercentage);
        }

        private static void RotateArrow(GameObject arrow, float clockPercentage)
        {
            var currentAngle = 360 * clockPercentage;
            arrow.transform.rotation = Quaternion.Euler(0,0, 360 - currentAngle);
        }
    }
}