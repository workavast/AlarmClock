using UnityEngine;

namespace AlarmClock.Scripts.Ui.AlarmClock
{
    public class AlarmClockInput : MonoBehaviour
    {
        [SerializeField] private ClockArrow hourArrow;
        [SerializeField] private ClockArrow minuteArrow;
        [SerializeField] private ClockArrow secondArrow;
        
        private IAlarmClockProvider _alarmClockProvider;
        private IPrepareAlarmClockProvider _prepareAlarmClockProvider;
        private bool _isInputState;
        
        public void Initialize()
        {
            _alarmClockProvider = AppData.AlarmClockProvider;
            _prepareAlarmClockProvider = AppData.PrepareAlarmClockProvider;
            
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
            if (_isInputState == inputState)
                return;

            _isInputState = inputState;
            hourArrow.Interactable = inputState;
            minuteArrow.Interactable = inputState;
            secondArrow.Interactable = inputState;
            UpdateView();
        }
        
        private void UpdateView()
        {
            var clockTime = _isInputState
                ? _prepareAlarmClockProvider.PreparedAlarmTime
                : _alarmClockProvider.TargetTime;
            
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