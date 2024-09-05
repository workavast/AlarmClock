namespace AlarmClock.Scripts
{
    public interface IPrepareAlarmClockProvider
    {
        public ClockTime PreparedAlarmTime { get; }

        public void Reset();
    }
}