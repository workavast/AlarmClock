namespace AlarmClock.Scripts
{
    public class PrepareAlarmClockProvider : IPrepareAlarmClockProvider
    {
        public ClockTime PreparedAlarmTime { get; }  = new();

        public void Reset() 
            => PreparedAlarmTime.SetTime(new ClockTime());
    }
}