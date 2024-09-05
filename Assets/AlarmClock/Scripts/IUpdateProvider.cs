using System;

namespace AlarmClock.Scripts
{
    public interface IUpdateProvider
    {
        public event Action OnUpdate;
    }
}