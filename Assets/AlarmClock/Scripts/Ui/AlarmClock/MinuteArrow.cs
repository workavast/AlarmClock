using UnityEngine;

namespace AlarmClock.Scripts.Ui.AlarmClock
{
    public class MinuteArrow : ClockArrow
    {
        protected override int SecondsCount => ClockTime.SecondsInHour;
        
        protected override void TryUpdateView()
        {
            if (IsDrag)
                return;
        
            PrevFullSeconds = PrepareAlarmClockProvider.PreparedAlarmTime.SecondsInMinutes + PrepareAlarmClockProvider.PreparedAlarmTime.Seconds;
            var angle = GetRoundAngle(PrevFullSeconds);
            
            transform.rotation = Quaternion.Euler(0,0, 360 - angle);
        }
    }
}