using Unity.Notifications.Android;
using UnityEngine;

namespace AlarmClock.Scripts.Ui
{
    public class ExternalAlarmNotification : MonoBehaviour
    {
        private const string ChanelId = "channel_id";
        private const int NotificationId = 1;
        private bool _notificationIsSend;

        private ClockTimeProvider _clockTimeProvider;
        private AlarmClockProvider _alarmClockProvider;
        
        private void Awake()
        {
            _clockTimeProvider = FindObjectOfType<ClockTimeProvider>();
            _alarmClockProvider = FindObjectOfType<AlarmClockProvider>();

            if (_notificationIsSend)
                CancelExternalNotification();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                if (!_notificationIsSend) 
                    TrySendExternalNotification();
            }
            else
            {
                if (_notificationIsSend) 
                    CancelExternalNotification();
            }
        }

        private void OnApplicationQuit()
        {
            if(!_notificationIsSend)
                TrySendExternalNotification();
        }

        private void TrySendExternalNotification()
        {
            if (!_alarmClockProvider.IsActive)
                return;
            
            SendExternalNotification();
        }
        
        private void SendExternalNotification()
        {
            var channel = new AndroidNotificationChannel()
            {
                Id = ChanelId,
                Name = "Default Channel",
                Importance = Importance.High,
                Description = "Generic notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);
            
            var notification = new AndroidNotification
            {
                Title = "Будильник!",
                Text = "Будильник сработал!",
                
                FireTime = System.DateTime.Now.AddSeconds(_alarmClockProvider.TargetTime.CurrentUnixSeconds -
                                                          _clockTimeProvider.ClockTime.CurrentUnixSeconds)
            };

            AndroidNotificationCenter.SendNotificationWithExplicitID(notification, ChanelId, NotificationId);
            _notificationIsSend = true;
        }

        private void CancelExternalNotification()
        {
            AndroidNotificationCenter.CancelNotification(NotificationId);
            _notificationIsSend = false;
        }
    }
}