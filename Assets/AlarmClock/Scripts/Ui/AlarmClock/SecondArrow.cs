using UnityEngine;

namespace AlarmClock.Scripts.Ui.AlarmClock
{
    public class SecondArrow : ClockArrow
    {
        protected override int SecondsCount => ClockTime.SecondsInMinute;
        
        protected override void TryUpdateView()
        {
            if (IsDrag)
                return;
        
            PrevFullSeconds = PrepareAlarmClockProvider.PreparedAlarmTime.Seconds;
            var angle = RoundAngle(PrevFullSeconds);
            
            transform.rotation = Quaternion.Euler(0,0, 360 - angle);
        }
    }
}