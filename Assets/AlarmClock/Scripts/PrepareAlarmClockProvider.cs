using UnityEngine;

namespace AlarmClock.Scripts
{
    public class PrepareAlarmClockProvider : MonoBehaviour
    {
        public readonly ClockTime PreparedAlarmTime = new();
        
        public void SetAlarmPrepareTime(ClockTime alarmClock) 
            => PreparedAlarmTime.SetTime(alarmClock);

        public void Reset() 
            => SetAlarmPrepareTime(new ClockTime());
    }
}