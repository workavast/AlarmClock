using UnityEngine;

namespace AlarmClock.Scripts.Ui.MainClock
{
    public class ClockView : MonoBehaviour
    {
        [SerializeField] private GameObject hourArrow;
        [SerializeField] private GameObject minuteArrow;
        [SerializeField] private GameObject secondArrow;
        
        private ClockTimeProvider _clockTimeProvider;

        private void Awake() 
            => _clockTimeProvider = FindObjectOfType<ClockTimeProvider>();

        private void OnEnable()
        {
            _clockTimeProvider.ClockTime.OnTick += UpdateView;
            UpdateView();
        }

        private void OnDisable() 
            => _clockTimeProvider.ClockTime.OnTick -= UpdateView;

        private void OnDestroy() 
            => _clockTimeProvider.ClockTime.OnTick -= UpdateView;

        private void UpdateView()
        {
            RotateArrow(hourArrow, _clockTimeProvider.ClockTime.FullDayPercentage * 2);
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