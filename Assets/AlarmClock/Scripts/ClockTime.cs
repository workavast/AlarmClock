using System;

namespace AlarmClock.Scripts
{
    public class ClockTime
    {
        public const int SecondsInDay = 24 * 60 * 60;
        public const int SecondsInHour = 60 * 60;
        public const int SecondsInMinute = 60;
        
        public long InitialUnixSeconds { get; private set; }
        public long CurrentUnixSeconds { get; private set; }
        public int Hours;
        public int Minutes;
        public int Seconds;

        public float FullDayPercentage => (float)TotalSeconds / SecondsInDay;
        public float HourPercentage => (float)(SecondsInMinutes + Seconds) / SecondsInHour;
        public float MinutePercentage => (float)Seconds / SecondsInMinute;
        
        public int TotalSeconds => SecondsInHours + SecondsInMinutes + Seconds;
        public int SecondsInHours => Hours * 60 * 60;
        public int SecondsInMinutes => Minutes * 60;
        
        public event Action OnTick;

        public ClockTime() 
            => InitialUnixSeconds = CurrentUnixSeconds = 0;

        public ClockTime(long initialUnixSeconds) 
            => InitialUnixSeconds = CurrentUnixSeconds = initialUnixSeconds;

        public void SetTime(ClockTime clockTime)
        {
            InitialUnixSeconds = clockTime.InitialUnixSeconds;
            CurrentUnixSeconds = clockTime.CurrentUnixSeconds;
            Hours = clockTime.Hours;
            Minutes = clockTime.Minutes;
            Seconds = clockTime.Seconds;
            OnTick?.Invoke();
        }

        public void AddSeconds(int seconds)
        {
            CurrentUnixSeconds += seconds;
            Seconds += seconds;
            if (Seconds >= 60)
            {
                var minutes = Seconds / 60;
                Seconds -= 60 * minutes;
                AddMinutes(minutes);
            }
            else
            {
                OnTick?.Invoke();
            }
        }

        public void AddMinutes(int minutes)
        {
            Minutes += minutes;
            if (Minutes >= 60)
            {
                var hours = Minutes / 60;
                Minutes -= 60 * hours;
                AddHours(hours);
            }
            else
            {
                OnTick?.Invoke();
            }
        }

        public void AddHours(int hours)
        {
            Hours += hours;
            if (Hours >= 24)
                Hours %= 24;

            OnTick?.Invoke();
        }
        
        public override string ToString() 
            => $"{Hours}:{Minutes}:{Seconds}";

        public static bool operator >=(ClockTime clockTimeLeft, ClockTime clockTimeRight)
        {
            if (clockTimeLeft is null && clockTimeRight is null)
                return true;
            if (clockTimeLeft is null)
                return false;
            if (clockTimeRight is null)
                return true;

            return clockTimeLeft.CurrentUnixSeconds >= clockTimeRight.CurrentUnixSeconds &&
                   clockTimeLeft.Hours >= clockTimeRight.Hours &&
                   clockTimeLeft.Minutes == clockTimeRight.Minutes &&
                   clockTimeLeft.Seconds == clockTimeRight.Seconds;
        }

        public static bool operator <=(ClockTime clockTimeLeft, ClockTime clockTimeRight) 
            => !(clockTimeLeft >= clockTimeRight);
    }
}