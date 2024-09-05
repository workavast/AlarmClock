using UnityEngine;

namespace AlarmClock.Scripts.Ui.AlarmClock
{
    public class HourArrow : ClockArrow
    {
        protected override int SecondsCount => ClockTime.SecondsInDay / 2;
        
        protected override void TryUpdateView()
        {
            if (IsDrag)
                return;
        
            PrevFullSeconds = PrepareAlarmClockProvider.PreparedAlarmTime.TotalSeconds;
            var angle = RoundAngle(PrevFullSeconds);
            
            transform.rotation = Quaternion.Euler(0,0, 360 - angle);
        }
    }
}