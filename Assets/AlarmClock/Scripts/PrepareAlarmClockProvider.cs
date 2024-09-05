using UnityEngine;

namespace AlarmClock.Scripts
{
    public class PrepareAlarmClockProvider : MonoBehaviour
    {
        public readonly ClockTime PreparedAlarmTime = new();
        
        public void Reset() 
            => PreparedAlarmTime.SetTime(new ClockTime());
    }
}