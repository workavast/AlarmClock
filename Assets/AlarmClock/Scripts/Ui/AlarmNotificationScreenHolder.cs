using UnityEngine;

namespace AlarmClock.Scripts.Ui
{
    public class AlarmNotificationScreenHolder : MonoBehaviour
    {
        [SerializeField] private GameObject alarmNotificationScreen;
        
        private AlarmClockProvider _alarmClockProvider;
        
        private void Awake()
        {
            _alarmClockProvider = FindObjectOfType<AlarmClockProvider>();
            _alarmClockProvider.OnAlarm += Show;
        }

        private void Show() 
            => alarmNotificationScreen.SetActive(true);

        public void Hide() 
            => alarmNotificationScreen.SetActive(false);
    }
}