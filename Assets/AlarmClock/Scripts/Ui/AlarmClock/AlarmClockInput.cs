using UnityEngine;

namespace AlarmClock.Scripts.Ui.AlarmClock
{
    public class AlarmClockInput : MonoBehaviour
    {
        [SerializeField] private ClockArrow hourArrow;
        [SerializeField] private ClockArrow minuteArrow;
        [SerializeField] private ClockArrow secondArrow;
        
        private AlarmClockProvider _alarmClockProvider;
        private PrepareAlarmClockProvider _prepareAlarmClockProvider;
        private bool IsInputState;
        
        public void Initialize()
        {
            _alarmClockProvider = FindObjectOfType<AlarmClockProvider>();
            _prepareAlarmClockProvider = FindObjectOfType<PrepareAlarmClockProvider>();
            
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
            if (IsInputState == inputState)
                return;

            IsInputState = inputState;
            hourArrow.Interactable = inputState;
            minuteArrow.Interactable = inputState;
            secondArrow.Interactable = inputState;
            UpdateView();
        }
        
        private void UpdateView()
        {
            ClockTime clockTime;
            if (IsInputState)
                clockTime = _prepareAlarmClockProvider.PreparedAlarmTime;
            else
                clockTime = _alarmClockProvider.TargetTime;
            
            RotateArrow(hourArrow.gameObject, clockTime.FullDayPercentage * 2);
            RotateArrow(minuteArrow.gameObject, clockTime.HourPercentage);
            RotateArrow(secondArrow.gameObject, clockTime.MinutePercentage);
        }

        private static void RotateArrow(GameObject arrow, float clockPercentage)
        {
            var currentAngle = 360 * clockPercentage;
            arrow.transform.rotation = Quaternion.Euler(0,0, 360 - currentAngle);
        }
    }
}