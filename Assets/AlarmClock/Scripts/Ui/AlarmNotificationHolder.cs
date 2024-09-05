using UnityEngine;

namespace AlarmClock.Scripts.Ui
{
    public class AlarmNotificationHolder : MonoBehaviour
    {
        [SerializeField] private GameObject alarmNotification;
        
        private AlarmClockProvider _alarmClockProvider;
        
        private void Awake()
        {
            _alarmClockProvider = FindObjectOfType<AlarmClockProvider>();
            _alarmClockProvider.OnAlarm += Show;
        }

        private void Show()
        {
            alarmNotification.SetActive(true);
        }

        public void Hide()
        {
            alarmNotification.SetActive(false);
        }
    }
}