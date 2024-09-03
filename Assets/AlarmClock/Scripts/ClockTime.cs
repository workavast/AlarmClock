using System;

namespace AlarmClock.Scripts
{
    public class ClockTime
    {
        private const int SecondsInDay = 24 * 60 * 60;
        private const int SecondsInHour = 60 * 60;
        private const int SecondsInMinute = 60;
        
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

        public void SetTime(ClockTime clockTime)
        {
            Hours = clockTime.Hours;
            Minutes = clockTime.Minutes;
            Seconds = clockTime.Seconds;
            OnTick?.Invoke();
        }

        public void AddSeconds(int seconds)
        {
            Seconds += seconds;
            if (Seconds >= 60)
            {
                var minutes = Seconds / 60;
                Seconds -= 60;
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
                Minutes -= 60;
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
    }
}