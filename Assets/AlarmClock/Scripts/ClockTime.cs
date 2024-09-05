using System;

namespace AlarmClock.Scripts
{
    public class ClockTime
    {
        public const int SecondsInDay = 24 * 60 * 60;
        public const int SecondsInHour = 60 * 60;
        public const int SecondsInMinute = 60;
        
        public long UnixSeconds { get; private set; }
        public int Hours { get; private set; }
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }

        public float FullDayPercentage => (float)TotalSeconds / SecondsInDay;
        public float HourPercentage => (float)(SecondsInMinutes + Seconds) / SecondsInHour;
        public float MinutePercentage => (float)Seconds / SecondsInMinute;
        
        public int TotalSeconds => SecondsInHours + SecondsInMinutes + Seconds;
        public int SecondsInHours => Hours * SecondsInHour;
        public int SecondsInMinutes => Minutes * SecondsInMinute;
        
        public event Action OnTick;

        public ClockTime()
        {
            
        }
        
        public ClockTime(DateTime dateTime)
        {
            UnixSeconds = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
            Hours = dateTime.Hour;
            Minutes = dateTime.Minute;
            Seconds = dateTime.Second;
        }

        public void SetTime(long unixSeconds, ClockTime clockTime)
        {
            UnixSeconds = unixSeconds;
            Hours = clockTime.Hours;
            Minutes = clockTime.Minutes;
            Seconds = clockTime.Seconds;
            OnTick?.Invoke();
        }
        
        public void SetTime(ClockTime clockTime)
        {
            UnixSeconds = clockTime.UnixSeconds;
            Hours = clockTime.Hours;
            Minutes = clockTime.Minutes;
            Seconds = clockTime.Seconds;
            OnTick?.Invoke();
        }

        public void ChangeSeconds(int seconds)
        {
            if (seconds == 0)
                return;

            if (seconds > 0) 
                AddSeconds(seconds);
            else
                RemoveSeconds(-seconds);
        }

        private void AddSeconds(int seconds)
        {
            UnixSeconds += seconds;
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
        
        private void AddMinutes(int minutes)
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

        private void AddHours(int hours)
        {
            Hours += hours;
            if (Hours >= 24)
                Hours %= 24;

            OnTick?.Invoke();
        }
        
        private void RemoveSeconds(int seconds)
        {
            UnixSeconds -= seconds;
            Seconds -= seconds;
            if (Seconds < 0)
            {
                var minutes = -Seconds / 60;
                if (Seconds % 60 != 0) 
                    minutes += 1;

                Seconds += 60 * minutes;
                RemoveMinutes(minutes);
            }
            else
            {
                OnTick?.Invoke();
            }
        }

        private void RemoveMinutes(int minutes)
        {
            Minutes -= minutes;
            if (Minutes < 0)
            {
                var hours = -Minutes / 60;
                if (Minutes % 60 != 0) 
                    hours += 1;

                Minutes += 60 * hours;
                RemoveHours(hours);
            }
            else
            {
                OnTick?.Invoke();
            }
        }
        
        private void RemoveHours(int hours)
        {
            Hours -= hours;
            if (Hours < 0)
            {
                var days = -Hours / 24;
                if (Hours % 24 != 0) 
                    days += 1;
                
                Hours += days * 24;
            }

            OnTick?.Invoke();
        }
        
        public override string ToString() 
            => $"{Hours:d2}:{Minutes:d2}:{Seconds:d2}";
    }
}