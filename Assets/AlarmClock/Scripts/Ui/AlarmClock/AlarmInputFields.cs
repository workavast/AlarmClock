using TMPro;
using UnityEngine;

namespace AlarmClock.Scripts.Ui.AlarmClock
{
    public class AlarmInputFields : MonoBehaviour
    {
        [SerializeField] private TMP_InputField hoursInput;
        [SerializeField] private TMP_InputField minutesInput;
        [SerializeField] private TMP_InputField secondsInput;

        private IAlarmClockProvider _alarmClock;
        private IPrepareAlarmClockProvider _prepareAlarmClockProvider;
        private InputState _inputState;
        private ViewState _viewState;

        public void Initialize()
        {
            _alarmClock = AppData.AlarmClockProvider;
            _prepareAlarmClockProvider = AppData.PrepareAlarmClockProvider;
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
            private readonly IAlarmClockProvider _alarmClockProvider;

            public ViewState(IAlarmClockProvider alarmClockProvider, TMP_InputField hoursInput, TMP_InputField minutesInput, TMP_InputField secondsInput) 
                : base(hoursInput,  minutesInput, secondsInput)
            {
                _alarmClockProvider = alarmClockProvider;
            }

            public override void ToggleActivity(bool isActive)
            {
                if (IsActive == isActive)
                    return;

                IsActive = isActive;
                if (IsActive)
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
                => inputField.SetTextWithoutNotify($"{value:d2}");
        }

        private class InputState : AlarmInputFieldsState
        {
            private readonly IPrepareAlarmClockProvider _prepareAlarmClockProvider;

            public InputState(IPrepareAlarmClockProvider prepareAlarmClockProvider, TMP_InputField hoursInput, 
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
                
                    HoursInput.onValueChanged.AddListener(OnUpdateInput);
                    MinutesInput.onValueChanged.AddListener(OnUpdateInput);
                    SecondsInput.onValueChanged.AddListener(OnUpdateInput);
                
                    HoursInput.onDeselect.AddListener(ValidateInputsFormats);
                    MinutesInput.onDeselect.AddListener(ValidateInputsFormats);
                    SecondsInput.onDeselect.AddListener(ValidateInputsFormats);
                    
                    PrintTime();
                    OnUpdateInput(null);
                    ToggleInteractive(true);
                }
                else
                {
                    _prepareAlarmClockProvider.PreparedAlarmTime.OnTick -= PrintTime;

                    HoursInput.onValueChanged.RemoveListener(OnUpdateInput);
                    MinutesInput.onValueChanged.RemoveListener(OnUpdateInput);
                    SecondsInput.onValueChanged.RemoveListener(OnUpdateInput);
                    
                    HoursInput.onDeselect.RemoveListener(ValidateInputsFormats);
                    MinutesInput.onDeselect.RemoveListener(ValidateInputsFormats);
                    SecondsInput.onDeselect.RemoveListener(ValidateInputsFormats);
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

                inputField.SetTextWithoutNotify($"{value:d2}");
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

            private void ValidateInputsFormats(string str)
            {
                HoursInput.SetTextWithoutNotify($"{int.Parse(HoursInput.text):d2}");
                MinutesInput.SetTextWithoutNotify($"{int.Parse(MinutesInput.text):d2}");
                SecondsInput.SetTextWithoutNotify($"{int.Parse(SecondsInput.text):d2}");
            }
            
            private void ApplyInput()
            {
                var clockTime = new ClockTime();
                clockTime.ChangeSeconds(int.Parse(HoursInput.text) * ClockTime.SecondsInHour);
                clockTime.ChangeSeconds(int.Parse(MinutesInput.text) * ClockTime.SecondsInMinute);
                clockTime.ChangeSeconds(int.Parse(SecondsInput.text));
                _prepareAlarmClockProvider.PreparedAlarmTime.SetTime(clockTime);
            }
        }
        #endregion
    }
}