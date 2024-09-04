using UnityEngine;
using UnityEngine.UI;

namespace AlarmClock.Scripts.Ui
{
    public class AlarmViewManager : MonoBehaviour
    {
        [SerializeField] private Button applyAlarm;
        [SerializeField] private Button cancelAlarm;
        
        [SerializeField] private AlarmInputFields alarmInputFields;
        [SerializeField] private AlarmClockView alarmClockView;
     
        private PrepareAlarmClock _prepareAlarmClock;
        private AlarmClock _alarmClock;
        
        private void Awake()
        {
            _alarmClock = FindObjectOfType<AlarmClock>();
            _prepareAlarmClock = FindObjectOfType<PrepareAlarmClock>();
            
            applyAlarm.onClick.AddListener(OnApplyAlarm);
            cancelAlarm.onClick.AddListener(OnCancelAlarm);
            alarmInputFields.Initialize();
        }

        private void OnEnable()
        {
            ToggleButtons(_alarmClock.IsActive);
            _alarmClock.OnActivationStateChange += UpdateView;
        }

        private void OnDisable()
        {
            _alarmClock.OnActivationStateChange -= UpdateView;
        }

        private void UpdateView()
        {
            ToggleButtons(_alarmClock.IsActive);
        }

        private void ToggleButtons(bool alarmIsActive)
        {
            applyAlarm.gameObject.SetActive(!alarmIsActive);
            cancelAlarm.gameObject.SetActive(alarmIsActive);
            
            alarmInputFields.ToggleState(!alarmIsActive);
        }
        
        private void OnApplyAlarm()
        {
            _alarmClock.SetAlarm(_prepareAlarmClock.PreparedAlarmTime);
        }
        
        private void OnCancelAlarm()
        {
            _alarmClock.CancelAlarm();
        }
    }
}