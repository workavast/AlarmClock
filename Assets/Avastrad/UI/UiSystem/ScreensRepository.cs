using System;
using System.Collections.Generic;
using AlarmClock.Scripts.Ui;
using AlarmClock.Scripts.Ui.AlarmClock;
using AlarmClock.Scripts.Ui.MainClock;
using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    [DisallowMultipleComponent]
    public class ScreensRepository : MonoBehaviour
    {
        private readonly Dictionary<Type, ScreenBase> _screens = new();
    
        public IEnumerable<ScreenBase> Screens => _screens.Values;

        private void Awake()
        {
            var screens = GetComponentsInChildren<ScreenBase>(true);
            foreach (var screen in screens) 
                _screens.Add(screen.GetType(), screen);
        }

        public TScreen GetScreen<TScreen>() 
            where TScreen : ScreenBase
        {
            if(_screens.TryGetValue(typeof(TScreen), out var screen)) 
                return (TScreen)screen;

            return default;
        }
        
        public ScreenBase GetScreen(ScreenType screenType)
        {
            switch (screenType)
            {
                case ScreenType.ClockScreen:
                    return GetScreen<ClockScreen>();
                case ScreenType.AlarmScreen:
                    return GetScreen<AlarmClockScreen>();
                default:
                    throw new ArgumentOutOfRangeException($"invalid parameter: {screenType}");
            }
        }
    }
}