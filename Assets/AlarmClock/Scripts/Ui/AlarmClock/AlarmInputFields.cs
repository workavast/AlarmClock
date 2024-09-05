using TMPro;
using UnityEngine;

namespace AlarmClock.Scripts.Ui.AlarmClock
{
    public class AlarmInputFields : MonoBehaviour
    {
        [SerializeField] private TMP_InputField hoursInput;
        [SerializeField] private TMP_InputField minutesInput;
        [SerializeField] private TMP_InputField secondsInput;

        private AlarmClockProvider _alarmClock;
        private PrepareAlarmClockProvider _prepareAlarmClockProvider;
        private InputState _inputState;
        private ViewState _viewState;

        public void Initialize()
        {
            _alarmClock = FindObjectOfType<AlarmClockProvider>();
            _prepareAlarmClockProvider = FindObjectOfType<PrepareAlarmClockProvider>();
            _inputState = new InputState(_prepareAlarmClockProvider, hoursInput, minutesInput, secondsInput);
            _viewState = new ViewState(_alarmClock, hoursInput, minutesInput, secondsInput);
        }

        public void ToggleState(bool inputState)
        {
            _inputState.ToggleActivity(inputState);
            _viewState.ToggleActivity(!inputState);
        }

        #region States
        private abstract class AlarmInputFieldsState
        {
            protected bool IsActive;
            protected readonly TMP_InputField HoursInput;
            protected readonly TMP_InputField MinutesInput;
            protected readonly TMP_InputField SecondsInput;

            protected AlarmInputFieldsState(TMP_InputField hoursInput, TMP_InputField minutesInput, TMP_InputField secondsInput)
            {
                HoursInput = hoursInput;
                MinutesInput = minutesInput;
                SecondsInput = secondsInput;
            }

            public abstract void ToggleActivity(bool isActive);
            
            protected void ToggleInteractive(bool isInteractive)
            {
                HoursInput.interactable = isInteractive;
                MinutesInput.interactable = isInteractive;
                SecondsInput.interactable = isInteractive;
            }
        } 
        
        private class ViewState : AlarmInputFieldsState
        {
            private readonly AlarmClockProvider _alarmClockProvider;

            public ViewState(AlarmClockProvider alarmClockProvider, TMP_InputField hoursInput, TMP_InputField minutesInput, TMP_InputField secondsInput) 
                : base( hoursInput,  minutesInput, secondsInput)
            {
                _alarmClockProvider = alarmClockProvider;
            }

            public override void ToggleActivity(bool isActive)
            {
                if (IsActive == isActive)
                    return;

                IsActive = isActive;
                if (isActive)
                {
                    _alarmClockProvider.TargetTime.OnTick += PrintTime;
                
                    ToggleInteractive(false);
                    PrintTime();
                }
                else
                {
                    _alarmClockProvider.TargetTime.OnTick -= PrintTime;
                }
            }
            
            private void PrintTime()
            {
                PrintTime(HoursInput, _alarmClockProvider.TargetTime.Hours);
                PrintTime(MinutesInput, _alarmClockProvider.TargetTime.Minutes);
                PrintTime(SecondsInput, _alarmClockProvider.TargetTime.Seconds);
            }

            private static void PrintTime(TMP_InputField inputField, int value)
            {
                var valueText = "";
                if (value < 10)
                    valueText = "0";

                inputField.SetTextWithoutNotify(valueText + value);
            }
        }

        private class InputState : AlarmInputFieldsState
        {
            private readonly PrepareAlarmClockProvider _prepareAlarmClockProvider;

            public InputState(PrepareAlarmClockProvider prepareAlarmClockProvider, TMP_InputField hoursInput, 
                TMP_InputField minutesInput, TMP_InputField secondsInput) 
                : base( hoursInput,  minutesInput, secondsInput)
            {
                _prepareAlarmClockProvider = prepareAlarmClockProvider;
            }
            
            public override void ToggleActivity(bool isActive)
            {
                if (IsActive == isActive)
                    return;

                IsActive = isActive;
                
                if (isActive)
                {
                    _prepareAlarmClockProvider.PreparedAlarmTime.OnTick += PrintTime;
                    PrintTime();
                
                    HoursInput.onValueChanged.AddListener(OnUpdateInput);
                    MinutesInput.onValueChanged.AddListener(OnUpdateInput);
                    SecondsInput.onValueChanged.AddListener(OnUpdateInput);
                
                    ToggleInteractive(true);
                    OnUpdateInput(null);
                }
                else
                {
                    HoursInput.onValueChanged.RemoveListener(OnUpdateInput);
                    MinutesInput.onValueChanged.RemoveListener(OnUpdateInput);
                    SecondsInput.onValueChanged.RemoveListener(OnUpdateInput);
                }
            }
            
            private void PrintTime()
            {
                PrintTime(HoursInput, _prepareAlarmClockProvider.PreparedAlarmTime.Hours);
                PrintTime(MinutesInput, _prepareAlarmClockProvider.PreparedAlarmTime.Minutes);
                PrintTime(SecondsInput, _prepareAlarmClockProvider.PreparedAlarmTime.Seconds);
            }

            private static void PrintTime(TMP_InputField inputField, int value)
            {
                if (inputField.isFocused)
                    return;
                
                var valueText = "";
                if (value < 10)
                    valueText = "0";

                inputField.SetTextWithoutNotify(valueText + value);
            }
            
            private void OnUpdateInput(string str)
            {
                if (!int.TryParse(HoursInput.text, out var hoursValue) || hoursValue >= 24)
                    HoursInput.SetTextWithoutNotify("23");

                if (!int.TryParse(MinutesInput.text, out var minuteValue) || minuteValue >= 60)
                    MinutesInput.SetTextWithoutNotify("59");

                if (!int.TryParse(SecondsInput.text, out var secondsValue) || secondsValue >= 60)
                    SecondsInput.SetTextWithoutNotify("59");
                
                ApplyInput();
            }

            private void ApplyInput()
            {
                var clockTime = new ClockTime();
                clockTime.AddHours(int.Parse(HoursInput.text));
                clockTime.AddMinutes(int.Parse(MinutesInput.text));
                clockTime.ChangeSeconds(int.Parse(SecondsInput.text));
                _prepareAlarmClockProvider.PreparedAlarmTime.SetTime(clockTime);
            }
        }
        #endregion
    }
}