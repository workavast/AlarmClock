using System;

namespace AlarmClock.Scripts
{
    public interface IAlarmClockProvider
    {
        public long TargetUnixTime => TargetTime.UnixSeconds;
        public ClockTime TargetTime { get; }
        public bool IsActive { get; }
        public event Action OnActivationStateChange;
        public event Action OnAlarm;

        public void SetAlarm(ClockTime alarmClock);
        public void CancelAlarm();
    }
}