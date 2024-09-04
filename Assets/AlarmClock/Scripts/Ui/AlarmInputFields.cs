using TMPro;
using UnityEngine;

namespace AlarmClock.Scripts.Ui
{
    public class AlarmInputFields : MonoBehaviour
    {
        [SerializeField] private TMP_InputField hoursInput;
        [SerializeField] private TMP_InputField minutesInput;
        [SerializeField] private TMP_InputField secondsInput;

        private AlarmClock _alarmClock;
        private PrepareAlarmClock _prepareAlarmClock;
        private InputState _inputState;
        private ViewState _viewState;

        public void Initialize()
        {
            _alarmClock = FindObjectOfType<AlarmClock>();
            _prepareAlarmClock = FindObjectOfType<PrepareAlarmClock>();
            _inputState = new InputState(_prepareAlarmClock, hoursInput, minutesInput, secondsInput);
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
            private readonly AlarmClock _alarmClock;

            public ViewState(AlarmClock alarmClock, TMP_InputField hoursInput, TMP_InputField minutesInput, TMP_InputField secondsInput) 
                : base( hoursInput,  minutesInput, secondsInput)
            {
                _alarmClock = alarmClock;
            }

            public override void ToggleActivity(bool isActive)
            {
                if (IsActive == isActive)
                    return;

                IsActive = isActive;
                if (isActive)
                {
                    _alarmClock.TargetTime.OnTick += PrintTime;
                
                    ToggleInteractive(false);
                    PrintTime();
                }
                else
                {
                    _alarmClock.TargetTime.OnTick -= PrintTime;
                }
            }
            
            private void PrintTime()
            {
                PrintTime(HoursInput, _alarmClock.TargetTime.Hours);
                PrintTime(MinutesInput, _alarmClock.TargetTime.Minutes);
                PrintTime(SecondsInput, _alarmClock.TargetTime.Seconds);
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
            private readonly PrepareAlarmClock _prepareAlarmClock;

            public InputState(PrepareAlarmClock prepareAlarmClock, TMP_InputField hoursInput, 
                TMP_InputField minutesInput, TMP_InputField secondsInput) 
                : base( hoursInput,  minutesInput, secondsInput)
            {
                _prepareAlarmClock = prepareAlarmClock;
            }
            
            public override void ToggleActivity(bool isActive)
            {
                if (IsActive == isActive)
                    return;

                IsActive = isActive;
                
                if (isActive)
                {
                    _prepareAlarmClock.PreparedAlarmTime.OnTick += PrintTime;
                
                    HoursInput.onValueChanged.AddListener(OnUpdateInput);
                    MinutesInput.onValueChanged.AddListener(OnUpdateInput);
                    SecondsInput.onValueChanged.AddListener(OnUpdateInput);
                
                    ToggleInteractive(true);
                    OnUpdateInput(null);
                    PrintTime();
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
                PrintTime(HoursInput, _prepareAlarmClock.PreparedAlarmTime.Hours);
                PrintTime(MinutesInput, _prepareAlarmClock.PreparedAlarmTime.Minutes);
                PrintTime(SecondsInput, _prepareAlarmClock.PreparedAlarmTime.Seconds);
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
                    HoursInput.SetTextWithoutNotify("00");

                if (!int.TryParse(MinutesInput.text, out var minuteValue) || minuteValue >= 60)
                    MinutesInput.SetTextWithoutNotify("00");

                if (!int.TryParse(SecondsInput.text, out var secondsValue) || secondsValue >= 60)
                    SecondsInput.SetTextWithoutNotify("00");
                
                ApplyInput();
            }

            private void ApplyInput()
            {
                var clockTime = new ClockTime();
                clockTime.AddHours(int.Parse(HoursInput.text));
                clockTime.AddMinutes(int.Parse(MinutesInput.text));
                clockTime.AddSeconds(int.Parse(SecondsInput.text));
                _prepareAlarmClock.PreparedAlarmTime.SetTime(clockTime);
            }
        }
        #endregion
    }
}