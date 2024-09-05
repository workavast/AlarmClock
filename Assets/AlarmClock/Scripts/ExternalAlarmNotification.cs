using Unity.Notifications.Android;
using UnityEngine;

namespace AlarmClock.Scripts
{
    public class ExternalAlarmNotification : MonoBehaviour
    {
        private const string ChanelId = "channel_id";
        private const int NotificationId = 1;
        private bool _notificationIsSend;

        private IClockTimeProvider _clockTimeProvider;
        private IAlarmClockProvider _alarmClockProvider;
        
        private void Awake()
        {
            _clockTimeProvider = AppData.ClockTimeProvider;
            _alarmClockProvider = AppData.AlarmClockProvider;

            CancelNotification();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                if (!_notificationIsSend) 
                    TrySendNotification();
            }
            else
            {
                if (_notificationIsSend) 
                    CancelNotification();
            }
        }

        private void OnApplicationQuit()
        {
            if(!_notificationIsSend)
                TrySendNotification();
        }

        private void TrySendNotification()
        {
            if (!_alarmClockProvider.IsActive)
                return;
            
            SendNotification();
        }
        
        private void SendNotification()
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
                
                FireTime = System.DateTime.Now.AddSeconds(_alarmClockProvider.TargetTime.UnixSeconds -
                                                          _clockTimeProvider.ClockTime.UnixSeconds)
            };

            AndroidNotificationCenter.SendNotificationWithExplicitID(notification, ChanelId, NotificationId);
            _notificationIsSend = true;
        }

        private void CancelNotification()
        {
            AndroidNotificationCenter.CancelNotification(NotificationId);
            _notificationIsSend = false;
        }
    }
}