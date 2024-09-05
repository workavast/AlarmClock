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
        
        private void Awake()
        {
            _alarmClockProvider = FindObjectOfType<Scripts.AlarmClockProvider>();
            _prepareAlarmClockProvider = FindObjectOfType<PrepareAlarmClockProvider>();
            
            applyAlarm.onClick.AddListener(OnApplyAlarm);
            cancelAlarm.onClick.AddListener(OnCancelAlarm);
            alarmInputFields.Initialize();
        }

        private void OnEnable()
        {
            ToggleButtons(_alarmClockProvider.IsActive);
            _alarmClockProvider.OnActivationStateChange += UpdateView;
        }

        private void OnDisable()
        {
            _alarmClockProvider.OnActivationStateChange -= UpdateView;
        }

        private void UpdateView()
        {
            ToggleButtons(_alarmClockProvider.IsActive);
        }

        private void ToggleButtons(bool alarmIsActive)
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