using UnityEngine;

namespace AlarmClock.Scripts.Ui.AlarmClock
{
    public class AlarmClockInput : MonoBehaviour
    {
        [SerializeField] private ClockArrow hourArrow;
        [SerializeField] private ClockArrow minuteArrow;
        [SerializeField] private ClockArrow secondArrow;
        
        private Scripts.AlarmClockProvider _alarmClockProvider;

        private void Awake()
        {
            _alarmClockProvider = FindObjectOfType<Scripts.AlarmClockProvider>();
            hourArrow.Initialize();
            minuteArrow.Initialize();
            secondArrow.Initialize();
        }

        private void OnEnable()
        {
            _alarmClockProvider.TargetTime.OnTick += UpdateView;
            UpdateView();
        }

        private void OnDisable() 
            => _alarmClockProvider.TargetTime.OnTick -= UpdateView;

        private void OnDestroy() 
            => _alarmClockProvider.TargetTime.OnTick -= UpdateView;

        public void ToggleState(bool inputState)
        {
            hourArrow.Interactable = inputState;
            minuteArrow.Interactable = inputState;
            secondArrow.Interactable = inputState;
        }
        
        private void UpdateView()
        {
            RotateArrow(hourArrow.gameObject, _alarmClockProvider.TargetTime.FullDayPercentage * 2);
            RotateArrow(minuteArrow.gameObject, _alarmClockProvider.TargetTime.HourPercentage);
            RotateArrow(secondArrow.gameObject, _alarmClockProvider.TargetTime.MinutePercentage);
        }

        private static void RotateArrow(GameObject arrow, float clockPercentage)
        {
            var currentAngle = 360 * clockPercentage;
            arrow.transform.rotation = Quaternion.Euler(0,0, 360 - currentAngle);
        }
    }
}