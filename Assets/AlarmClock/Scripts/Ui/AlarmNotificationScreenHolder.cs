using UnityEngine;

namespace AlarmClock.Scripts.Ui
{
    public class AlarmNotificationScreenHolder : MonoBehaviour
    {
        [SerializeField] private GameObject alarmNotificationScreen;
        
        private IAlarmClockProvider _alarmClockProvider;
        
        private void Awake()
        {
            _alarmClockProvider = AppData.AlarmClockProvider;
            _alarmClockProvider.OnAlarm += Show;
        }

        private void Show() 
            => alarmNotificationScreen.SetActive(true);

        public void Hide() 
            => alarmNotificationScreen.SetActive(false);
    }
}