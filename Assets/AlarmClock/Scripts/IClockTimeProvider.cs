using System;

namespace AlarmClock.Scripts
{
    public interface IClockTimeProvider
    {
        public ClockTime ClockTime { get; }
        public bool IsInitialized { get; }
        
        public event Action OnInitialized;
    }
}