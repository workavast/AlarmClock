using UnityEngine;
using UnityEngine.UI;

namespace AlarmClock.Scripts.Ui.AlarmClock
{
    public class AlarmViewManager : MonoBehaviour
    {
        [SerializeField] private Button applyAlarm;
        [SerializeField] private Button cancelAlarm;
        
        [SerializeField] private AlarmInputFields alarmInputFields;
        [SerializeField] private AlarmClockInput alarmClockInput;
     
        private PrepareAlarmClockProvider _prepareAlarmClockProvider;
        private Scripts.AlarmClockProvider _alarmClockProvider;
        
        public void Initialize()
        {
            _alarmClockProvider = FindObjectOfType<AlarmClockProvider>();
            _prepareAlarmClockProvider = FindObjectOfType<PrepareAlarmClockProvider>();
            
            applyAlarm.onClick.AddListener(OnApplyAlarm);
            cancelAlarm.onClick.AddListener(OnCancelAlarm);
        
            alarmInputFields.Initialize();
            alarmClockInput.Initialize();
        }

        private void OnEnable()
        {
            ToggleStates(_alarmClockProvider.IsActive);
            _alarmClockProvider.OnActivationStateChange += ToggleStates;
        }

        private void OnDisable()
        {
            _alarmClockProvider.OnActivationStateChange -= ToggleStates;
        }

        private void ToggleStates() 
            => ToggleStates(_alarmClockProvider.IsActive);

        private void ToggleStates(bool alarmIsActive)
        {
            applyAlarm.gameObject.SetActive(!alarmIsActive);
            cancelAlarm.gameObject.SetActive(alarmIsActive);
            
            alarmInputFields.ToggleState(!alarmIsActive);
            alarmClockInput.ToggleState(!alarmIsActive);
        }
        
        private void OnApplyAlarm()
        {
            _alarmClockProvider.SetAlarm(_prepareAlarmClockProvider.PreparedAlarmTime);
        }
        
        private void OnCancelAlarm()
        {
            _alarmClockProvider.CancelAlarm();
        }
    }
}