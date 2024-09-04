using UnityEngine;

namespace AlarmClock.Scripts.Ui
{
    public class PrepareAlarmClock : MonoBehaviour
    {
        public readonly ClockTime PreparedAlarmTime = new();
        
        public void SetAlarmPrepareTime(ClockTime alarmClock) 
            => PreparedAlarmTime.SetTime(alarmClock);

        public void Reset() 
            => SetAlarmPrepareTime(new ClockTime());
    }
}