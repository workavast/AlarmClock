using System;

namespace AlarmClock.Scripts
{
    public class ClockTime
    {
        public int Hours;
        public int Minutes;
        public int Seconds;

        public event Action OnTick;

        public void UpdateTime(ClockTime clockTime)
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
        {
            return $"{Hours}:{Minutes}:{Seconds}";
        }
    }
}